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

            //do stuff
        }
    }
}
