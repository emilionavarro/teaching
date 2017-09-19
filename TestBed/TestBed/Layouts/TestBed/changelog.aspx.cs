using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Administration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Services;
using System.Web.Script.Services;

namespace TestBed.Layouts.TestBed
{
    public partial class changelog : LayoutsPageBase
    {
        private const int MAX_LOG_ENTRIES = 2;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager SM = null;

            if (ScriptManager.GetCurrent(this.Page) != null)
            {
                SM = ScriptManager.GetCurrent(this.Page);

                SM.EnablePageMethods = true;
            }
        }

        /// <summary>
        /// Gets static log 
        /// </summary>
        /// <returns>a limited log</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetLog()
        {
            RDChangelog RDLog = new RDChangelog(MAX_LOG_ENTRIES);
            RDLog.CreateRDChangelog();

            return new JavaScriptSerializer().Serialize(RDLog.changelogs);
        }
    }
}
