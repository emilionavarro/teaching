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
        static List<ChangelogItem> m_ChangeLog { get; set; } = new List<ChangelogItem>();

        private SPJobDefinition GetDemoJobDefinition(SPJobDefinitionCollection ajobDefinitions)
        {
            foreach (SPJobDefinition job in ajobDefinitions)
            {
                if (job.Title == "Demo Timer Job")
                {
                    return job;
                }
            }

            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SPWebService webService = SPFarm.Local.Services.OfType<SPWebService>().FirstOrDefault();
            SPJobDefinitionCollection jobDefinitions = webService.WebApplications.ElementAt(0).JobDefinitions;
            SPJobDefinition demoJob = GetDemoJobDefinition(jobDefinitions);
            IEnumerable<SPJobHistory> jobHistory = demoJob == null ? null : demoJob.HistoryEntries;
            ScriptManager SM = null;

            if(ScriptManager.GetCurrent(this.Page) != null)
            {
                SM = ScriptManager.GetCurrent(this.Page);

                SM.EnablePageMethods = true;
            }

            if (jobHistory != null)
                DisplayChangelog(webService.WebApplications.ElementAt(0).Sites[1], jobHistory.ElementAt(0).EndTime);

        }

        private void DisplayChangelog(SPSite asite, DateTime aendTime)
        {
            ClearChangelog();
            GetChangelog(asite, aendTime);
        }

        private void ClearChangelog()
        {
            m_ChangeLog.Clear();
        }

        private void GetChangelog(SPSite asite, DateTime aendTime)
        {
            SPChangeCollection changes;
            SPChangeQueryBuilder queryBuilder = new SPChangeQueryBuilder(false, true, true);
            Guid uniqueId = Guid.Empty;
            SPFile file = null;

            changes = asite.GetChanges(queryBuilder.query);

            foreach (SPChange item in changes)
            {
                uniqueId = GetSPUniqueID(item);
                file = asite.RootWeb.GetFile(uniqueId);

                try
                {
                    if (file.TimeLastModified != null && file.TimeLastModified > aendTime)
                        m_ChangeLog.Add(new ChangelogItem(file));
                }
                catch
                { }
            }
        }

        private Guid GetSPUniqueID(SPChange achangeObject)
        {
            Type type = achangeObject.GetType();

            if (type == typeof(SPChangeItem))
                return ((SPChangeItem)achangeObject).UniqueId;
            else if (type == typeof(SPChangeFile))
                return ((SPChangeFile)achangeObject).UniqueId;
            else if (type == typeof(SPChangeFolder))
                {/*handle folders*/}

            return Guid.Empty;      
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetLog()
        {
            return new JavaScriptSerializer().Serialize(m_ChangeLog);
        }
    }

    public class ChangelogItem
    {
        public string Name { get; set; }
        public string Author { get; set; }

        private DateTime m_date;
        public DateTime Date
        {
            get { return m_date; }
            set
            {
                m_date = value;
                DateString = value.ToString();
            }

        }
        public string DateString { get; private set; }

        public ChangelogItem(SPFile file)
        {
            Name = file.Name;   
            Author = file.Author.ToString();
            Date = file.TimeLastModified;
        }
    }

    public class SPChangeQueryBuilder
    {
        public SPChangeQuery query { get; set; }

        public SPChangeQueryBuilder(bool aincludeFiles, bool aincludeItems, bool aincludeFolders)
        {
            query = new SPChangeQuery(false, false);

            query.File = aincludeFiles;
            query.Item = aincludeItems;
            query.Folder = aincludeFolders;
            query.Add = true;
            query.Delete = true;
            query.Move = true;
            query.FetchLimit = 2000;
        }
    }
}
