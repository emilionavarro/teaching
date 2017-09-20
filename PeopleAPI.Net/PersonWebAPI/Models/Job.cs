using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonWebAPI.Models
{
    public class Job : Identity
    {
        public Job() : base()
        {
            Name = "Job " + Id.ToString();
        }

        public Job(int inID, string inName) : base(inID, inName)
        {
        }
    }
}