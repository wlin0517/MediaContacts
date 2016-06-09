using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CustomerServer.Models;
using System.IO;
using System.Web;
using System.Linq.Dynamic;
using Newtonsoft.Json;
using CustomerServer.Extensions;

namespace CustomerServer.Controllers
{
    public class ContactController : ApiController
    {
        #region private fields

        private List<Contact> contacts = new List<Contact>(); // list of contacts
        private List<Outlet> outlets = new List<Outlet>();    // list of outlets

        #endregion


        #region constructor

        // initialize private class fields
        public ContactController()
        {
            // load list of contacts from json file
            string contactJsonFile = HttpContext.Current.Server.MapPath("~/App_Data/Contacts.json");
            contacts = LoadJson<Contact>(contactJsonFile);

            // load list of outlets from json file
            string outletJsonFile = HttpContext.Current.Server.MapPath("~/App_Data/Outlets.json");
            outlets = LoadJson<Outlet>(outletJsonFile);
        }

        #endregion


        #region web api methods

        // GET: api/Contact
        public PagedResults Get(int pageNumber = 1, int pageSize = 25, string sortField = null, string sortDir = null)
        {
            var contactPage = GetContactPage(pageNumber, pageSize, sortField, sortDir);
            return contactPage;
        }

        #endregion


        #region private methods


        // Get list of contacts given the paging/sorting
        private PagedResults GetContactPage(int pageNumber, int pageSize, string sortField, string sortDir)
        {
            // if data error, return
            if (contacts == null || contacts.Count == 0 || outlets == null || outlets.Count == 0)
                return null;


            var pagedResults = new PagedResults();           

            try
            {
                // left join contact list with outlet lists in order to find the contact's outlet name
                var query = (from contact in contacts
                             join outlet in outlets
                                    on contact.OutletId equals outlet.Id
                                           into a
                             from b in a.DefaultIfEmpty(new Outlet())
                             select new ContactWithOutletInfo
                             {
                                 ContactName = contact.FirstName + " " + contact.LastName,
                                 ContactTitle = contact.Title,
                                 OutletName = b.Name,
                                 ContactProfile = contact.Profile
                             }).ToList<ContactWithOutletInfo>().AsQueryable();

                // get the total number of records
                int totalRecords = query.Count();


                // Sorting
                string[] sort = !string.IsNullOrEmpty(sortField) ? new string[] {sortField + " " + sortDir} : null;

                if (sort != null && sort.Length != 0)
                {
                     // using the LINQ Dynamic Query Library to sort
                     // ref: Dynamic LINQ (ScottGu's Blog)
                     // --- use sort array here in order to support multiple columns sorting in the future release
                     // --- example: string[] = sort new string[] { "ContactName DESC", "ContactTitle DESC" };
                    query = query.ApplySorting(sort); 
                }               
                else
                {
                    // default sorting by ContactName Asc
                    query = query.OrderBy(c => c.ContactName);
                }
               
 
                // Paging                        
                int skip = (pageNumber - 1) * pageSize; // the number of records to skip

                var contactList = query
                               .Skip(skip)
                               .Take(pageSize)
                               .ToList<ContactWithOutletInfo>();

                // results
                pagedResults.TotalRecords = totalRecords;
                pagedResults.Contacts = contactList;
            }
            catch(Exception ex)
            {
                // log error
            }

            return pagedResults;
        }


        // read and parse json file to the specified type
        private List<T> LoadJson<T>(string filePath)
        {
            List<T> objects = null;

            try
            {
                // read json file
                StreamReader r = new StreamReader(filePath);
                string json = r.ReadToEnd();

           
                // parse json file to List<T>
                objects = JsonConvert.DeserializeObject<List<T>>(json);
                
            }
            catch(Exception ex)
            {
                // log exception
            }

            return objects;
        }

        #endregion

    }
}
