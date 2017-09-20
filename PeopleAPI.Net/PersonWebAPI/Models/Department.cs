using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonWebAPI.Models
{
    public class Department : Identity
    {
        /// <summary>
        /// Variables
        /// </summary>
        public string Desc;
        public List<Person> people = new List<Person>();
        private const int RANDOM_PEOPLE = 5;

        /// <summary>
        /// Constructors
        /// </summary>
        public Department() : base()
        {
            Name = "Department " + Id.ToString();
            Desc = "Department Description";

            // Build random people
            int counter = 0;
            for(; counter < RANDOM_PEOPLE; counter++)
                people.Add(new Person());
        }

        public Department(int inID, string inName, string inDesc, List<Person> inPeople) : base(inID, inName)
        {
            Desc = inDesc;
            people = inPeople;
        }
    }
}