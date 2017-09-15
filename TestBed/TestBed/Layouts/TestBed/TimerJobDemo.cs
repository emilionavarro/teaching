using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace TestBed.Layouts
{
    class TimerJobDemo:SPJobDefinition
    {
        public TimerJobDemo():base()
        {

        } 

        public TimerJobDemo (string jobName, SPService service) : base(jobName, service, null, SPJobLockType.None)
        {
            this.Title = "Demo Timer Job";
        }

        public TimerJobDemo(string jobName, SPWebApplication webapp) : base(jobName, webapp, null, SPJobLockType.ContentDatabase)
        {
            this.Title = "Demo Timer Job";
        }

        public override void Execute(Guid targetInstanceId)
        {
            base.Execute(targetInstanceId);

            //((SPWebApplication)this.Parent).Sites[0].WebApplication.RunningJobs
            //SPWebApplication spweb = this.Parent as SPWebApplication;
            //SPListCollection mySPLists = spweb.Sites[0].RootWeb.Lists;

            //for (var i = 0; i < mySPLists.Count; i++)
            //{
            //    var x = mySPLists[i];
            //}
            //do stuff
        }
    }
}
