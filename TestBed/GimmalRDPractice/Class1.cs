using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Administration;

namespace GimmalRDPractice
{
    public class Changelog
    {
        static List<ChangelogItem> m_ChangeLog { get; set; } = new List<ChangelogItem>();
        static List<ChangeLogCollection> m_changelogs { get; set; } = new List<ChangeLogCollection>();

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
    }
}
