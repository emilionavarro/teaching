using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonWebAPI.Models
{
    public class Person : Identity
    {
        /// <summary>
        /// Variarbles
        /// </summary>
        public Job job;

        /// <summary>
        /// Constructors
        /// </summary>
        public Person() : base()
        {   
            Name = "Person " + Id.ToString();
            job = new Job();
        }

        public Person(int inID, string inName, Job inJob) : base(inID, inName)
        {
            job = inJob;
        }
    }
}