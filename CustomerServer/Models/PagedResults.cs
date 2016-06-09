using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerServer.Models
{

    public class PagedResults
    {
        #region properties

        public int TotalRecords { get; set; } // total number of contacts
        public List<ContactWithOutletInfo> Contacts { get; set; } // one page contacts 

        #endregion
    }

}