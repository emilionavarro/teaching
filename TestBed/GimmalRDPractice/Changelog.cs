﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;

namespace GimmalRDPractice
{
    public class Changelog
    {
        private static List<ChangelogItem> m_ChangeLog { get; set; } = new List<ChangelogItem>();
        private static List<ChangeLogCollection> m_changelogs { get; set; } = new List<ChangeLogCollection>();
        private static System.Object lockThis = new System.Object();

        public Changelog()
        {
            SPWebService webService = SPFarm.Local.Services.OfType<SPWebService>().FirstOrDefault();
            SPJobDefinitionCollection jobDefinitions = webService.WebApplications.ElementAt(0).JobDefinitions;
            SPJobDefinition demoJob = GetDemoJobDefinition(jobDefinitions);
            IEnumerable<SPJobHistory> jobHistory = demoJob == null ? null : demoJob.HistoryEntries;

            if (jobHistory != null)
            {
                BuildChangelogs(webService.WebApplications.ElementAt(0).Sites, jobHistory.ElementAt(0).EndTime);
            }
        }

        public static List<ChangeLogCollection> GetLog()
        {
            lock (lockThis)
            {
                return m_changelogs;
            }               
        }

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

        private void BuildChangelogs(SPSiteCollection asites, DateTime aendTime)
        {
            ChangeLogCollection changelogCollection = null;

            ClearChangelog();

            foreach (SPSite site in asites)
            {
                changelogCollection = new ChangeLogCollection(site.ID, site.Url);
                changelogCollection.Changelog = GetChangelog(site, aendTime); //get the new changelog

                lock(lockThis)
                {
                    m_changelogs.Add(changelogCollection);
                }
                
            }
        }

        private void ClearChangelog()
        {
            lock (lockThis)
            {
                m_changelogs.Clear();
            }
               
        }

        private List<ChangelogItem> GetChangelog(SPSite asite, DateTime aendTime)
        {
            SPChangeCollection changes;
            SPChangeQueryBuilder queryBuilder = new SPChangeQueryBuilder(false, true, true, asite, aendTime);
            Guid uniqueId = Guid.Empty;
            SPFile file = null;
            List<ChangelogItem> currentChangelog = new List<ChangelogItem>();

            changes = asite.GetChanges(queryBuilder.query);

            foreach (SPChange item in changes)
            {
                uniqueId = GetSPUniqueID(item);
                file = asite.RootWeb.GetFile(uniqueId);

                try
                {
                    if (file.TimeLastModified != null && file.TimeLastModified > aendTime)
                        currentChangelog.Add(new ChangelogItem(file));
                }
                catch
                { }
            }

            return currentChangelog;
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
        public int Count
        {
            get
            {
                return this.Changelog.Count;
            }
        }
        public ChangeLogCollection(Guid aguid, string aname)
        {
            ID = aguid.ToString();
            Name = aname;
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
