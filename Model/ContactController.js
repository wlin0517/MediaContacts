

function ContactController($scope, $http) {

    // initialize data
    $scope.Contacts = [];
    $scope.sortOptions = { fields: ['ContactName'], directions: ['asc'] }   //default sorting


    // paging
    $scope.pagination = {
        pageSize: 25,
        pageNumber: 1,
        totalRecords: null,
        totalPages: null,
        nextPage: function () {
            if (this.pageNumber < this.totalPages) {
                this.pageNumber++;
                $scope.loadContactPage();
            }
        },
        previousPage: function () {
            if (this.pageNumber > 1) {
                this.pageNumber--;
                $scope.loadContactPage(t);
            }
        }
    }


    // sorting
    // 1. on sorting event fill out sortOptions in scope
    $scope.$on('ngGridEventSorted', function (event, sortColumns) {

        $scope.sortOptions.sortfield = sortColumns.fields[0];
        $scope.sortOptions.sortdir = sortColumns.directions[0];
    });

    // 2. when sortOption changes, refresh contact page
    $scope.$watch('sortOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            // refresh page
            $scope.loadContactPage();
        }
    }, true);


    // grid
    $scope.gridOptions = {
        data: 'Contacts',
        sortInfo: $scope.sortOptions,
        enableSorting: true,
        useExternalSorting: true, // use server-side paging
        multiSelect: false,
       
        columnDefs:
        [{ field: "ContactName", displayName: "Contact Name", width: "*"},
         { field: "ContactTitle", displayName: "Contact Title", width: "*"},
         { field: "OutletName", displayName: "Outlet Name", width: "**" },

         //display Profile details in tooltip when mounse on this column
         { field: 'ContactProfile', displayName: 'Contact Profile', width: "***",
           cellTemplate: '<div style="word-wrap: normal" class="ngCellText" title="{{row.getProperty(col.field)}}">{{row.getProperty(col.field)}}</div>' 
         }]
    };


    // webapi call with paging and sorting
    $scope.loadContactPage = function () {
        $http({
            method: 'GET',
            url: "http://localhost:2169/api/Contact",
            params: {
                pageNumber: $scope.pagination.pageNumber,
                pageSize: $scope.pagination.pageSize,
                sortField: $scope.sortOptions.sortfield,
                sortDir: $scope.sortOptions.sortdir
            },
        }).success(function (data) {
            $scope.pagination.totalRecords = data.TotalRecords;
            $scope.pagination.totalPages = Math.ceil($scope.pagination.totalRecords / $scope.pagination.pageSize);
            $scope.Contacts = data.Contacts;
        }).error(function(data, status) {
            alert("error getting contact data...")
        })
    };

    // initial load contact page
    $scope.loadContactPage();
}