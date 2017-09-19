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

namespace TestBed
{
    public class RDChangelog
    {
        private List<ChangeLogCollection> m_changelogs { get; set; } = new List<ChangeLogCollection>();
        public List<ChangeLogCollection> changelogs
        {
            get
            {
                return m_changelogs;
            }
            private set
            {
                m_changelogs = value;
            }
        }
        public int maxItems { get; set; }

        /// <summary>
        /// constructor that takes limits amount of items in each changelog for sites
        /// </summary>
        /// <param name="maxItems"></param>
        public RDChangelog(int max)
        {
            maxItems = max;
        }

        public RDChangelog()
        {
            maxItems = 0;
        }

        /// <summary>
        /// Creates a size limited collection of changelogs for each site found
        /// </summary>
        /// <returns>True if successful</returns>
        public bool CreateRDChangelog()
        {
            SPWebService webService = GetWebService();
            IEnumerable<SPJobHistory> jobHistory = GetJobHistory(webService);

            if (jobHistory != null)
            {
                BuildChangelogs(webService.WebApplications.ElementAt(0).Sites, jobHistory.ElementAt(0).EndTime);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Creates a size limited collection of changelogs for a specific site
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>True if successful</returns>
        public bool CreateRDChangelog(Guid guid)
        {
            SPWebService webService = GetWebService();
            IEnumerable<SPJobHistory> jobHistory = GetJobHistory(webService);

            if (jobHistory != null)
            {
                IEnumerable<SPSite> sites = webService.WebApplications.ElementAt(0).Sites.Where(m => m.ID == guid);

                BuildChangelogs(sites, jobHistory.ElementAt(0).EndTime);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a timer job history
        /// </summary>
        /// <param name="webService"></param>
        /// <returns>IEnumerable<SPJobHistory></returns>
        private IEnumerable<SPJobHistory> GetJobHistory(SPWebService webService)
        {
            SPJobDefinitionCollection jobDefinitions = webService.WebApplications.ElementAt(0).JobDefinitions;
            SPJobDefinition demoJob = GetDemoJobDefinition(jobDefinitions);

            return demoJob == null ? null : demoJob.HistoryEntries;
        }

        /// <summary>
        /// Gets current web service on the farm
        /// </summary>
        /// <returns>SPWebService</returns>
        private SPWebService GetWebService()
        {
            return SPFarm.Local.Services.OfType<SPWebService>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the job definition for the demo job
        /// </summary>
        /// <param name="ajobDefinitions"></param>
        /// <returns>SPJobDefinition</returns>
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

        /// <summary>
        /// Builds change logs object
        /// </summary>
        /// <param name="asites"></param>
        /// <param name="aendTime"></param>
        private void BuildChangelogs(IEnumerable<SPSite> asites, DateTime aendTime)
        {
            ChangeLogCollection changelogCollection = null;

            foreach (SPSite site in asites)
            {
                changelogCollection = new ChangeLogCollection(site.ID, site.Url);
                changelogCollection.Changelog = GetChangelog(site, aendTime, changelogCollection); //get the new changelog

                m_changelogs.Add(changelogCollection);
            }
        }
        
        /// <summary>
        /// Builds a specific changelog
        /// </summary>
        /// <param name="asite"></param>
        /// <param name="aendTime"></param>
        /// <param name="changeLogCollection"></param>
        /// <returns></returns>
        private List<ChangelogItem> GetChangelog(SPSite asite, DateTime aendTime, ChangeLogCollection changeLogCollection)
        {
            SPChangeCollection changes;
            SPChangeQueryBuilder queryBuilder = new SPChangeQueryBuilder(false, true, true, asite, aendTime);
            Guid uniqueId = Guid.Empty;
            SPFile file = null;
            List<ChangelogItem> currentChangelog = new List<ChangelogItem>();

            changes = asite.GetChanges(queryBuilder.query);

            if (changes.Count > 0)
            {

                if (maxItems > changes.Count || maxItems == 0)
                    maxItems = changes.Count;

                for (int len = changes.Count - maxItems, i = changes.Count - 1; i >= len; i--)
                {
                    uniqueId = GetSPUniqueID(changes[i]);
                    file = asite.RootWeb.GetFile(uniqueId);

                    try
                    {
                        if (file.TimeLastModified != null && file.TimeLastModified > aendTime)
                        {
                            currentChangelog.Add(new ChangelogItem(file));
                        }
                    }
                    catch
                    { }
                }
            }

            changeLogCollection.Count = changes.Count;

            return currentChangelog;
        }

        /// <summary>
        /// Gets the unique id of an spchange
        /// </summary>
        /// <param name="achangeObject"></param>
        /// <returns></returns>
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

    public class ChangeLogCollection
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<ChangelogItem> Changelog { get; set; }
        public string URL
        {
            get
            {
                return string.Format("{0}/_layouts/15/testbed/sitelog.aspx?guid={1}", SPContext.Current.Web.Url, ID);
            }
        }

        private int m_Count;
        public int Count
        {
            get
            {
                return m_Count;
            }
            set
            {
                m_Count = value;
            }
        }

        public ChangeLogCollection(Guid aguid, string aname)
        {
            ID = aguid.ToString();
            Name = aname;
            m_Count = 0;
        }

        public void IncrementCount()
        {
            m_Count++;
        }
    }

    public class SPChangeQueryBuilder
    {
        public SPChangeQuery query { get; set; }

        public SPChangeQueryBuilder(bool aincludeFiles, bool aincludeItems, bool aincludeFolders, SPSite asite, DateTime aendTime)
        {
            query = new SPChangeQuery(false, false);

            query.File = aincludeFiles;
            query.Item = aincludeItems;
            query.Folder = aincludeFolders;
            query.Add = true;
            query.Delete = true;
            query.Move = true;
            query.FetchLimit = 2000;

            this.SetChangeToken(asite, aendTime);
        }

        private void SetChangeToken(SPSite asite, DateTime aendTime)
        {
            string strChangeToken = string.Format("1;1;{0};{1};-1", asite.ID.ToString(), aendTime.Ticks.ToString());
            query.ChangeTokenStart = new SPChangeToken(strChangeToken);
        }
    }
}
