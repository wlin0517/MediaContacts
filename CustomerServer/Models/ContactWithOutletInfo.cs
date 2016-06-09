using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerServer.Models
{
    // aggregate class from join contact and outlet on outletId
    public class ContactWithOutletInfo
    {
        #region properties

        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string OutletName { get; set; }
        public string ContactProfile { get; set; }

        #endregion
    }
}