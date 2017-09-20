using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonWebAPI.Models
{
    public static class Setup
    {
        /// <summary>
        /// Variables
        /// </summary>
        private const int MAX_RANDOM_DEPT = 5;
        public static List<Department> departments = new List<Department>();
        public static List<Job> jobs = new List<Job>();
        public static List<Person> people = new List<Person>();

        /// <summary>
        /// Methods
        /// </summary>
        public static void BuildRandomDepartments()
        {
            if (departments.Count == MAX_RANDOM_DEPT)
                return;

            for( int counter = 0; counter < MAX_RANDOM_DEPT; counter++)
            {
                departments.Add(new Department());
            }
        }

        public static void BuildDepartments()
        {
            if (departments.Count > 0)
                return;

            // Create jobs
            jobs.Add(new Job(1, "Programmer"));
            jobs.Add(new Job(2, "Dev"));
            jobs.Add(new Job(3, "Yes man"));

            // Create people with jobs
            people.Add(new Person(1, "Phil", jobs.Where(job => job.Name == "Programmer").FirstOrDefault()));
            people.Add(new Person(2, "Emilio", jobs.Where(job => job.Name == "Yes man").FirstOrDefault()));
            people.Add(new Person(3, "Matt", jobs.Where(job => job.Name == "Dev").FirstOrDefault()));
            people.Add(new Person(4, "Zach", jobs.Where(job => job.Name == "Dev").FirstOrDefault()));

            // Create Departments 
            departments.Add(new Department(1, "Dev", "Dev Desc", people.Where(person => person.job.Name == "Dev").ToList()));
            departments.Add(new Department(2, "Code Monkeys", "Code Monkeys Desc", people.Where(person => person.job.Name == "Programmer").ToList()));
            departments.Add(new Department(3, "Yes Men", "Yes Men", people.Where(person => person.job.Name == "Yes man").ToList()));
        }
    }
}