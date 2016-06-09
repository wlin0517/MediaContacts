using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerServer.Models
{
    public class Contact
    {

        #region properties

        public string Id { get; set;}     
        public string OutletId { get; set; }     
        public string FirstName { get; set; }       
        public string LastName { get; set; }        
        public string Title { get; set; }
        public string Profile { get; set; }

        #endregion
    }

}
