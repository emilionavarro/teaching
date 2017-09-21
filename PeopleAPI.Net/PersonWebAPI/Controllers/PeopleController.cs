using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PersonWebAPI.Models;

namespace PersonWebAPI.Controllers
{
    public class PeopleController : ApiController
    {
        public IEnumerable<Person> Get()
        {
            return Setup.people;
        }

        public IHttpActionResult Get (int id)
        {
            Person thisPerson = Setup.people.Where(person => person.Id == id).FirstOrDefault();
            if (thisPerson == null)
                return NotFound();
            return Ok(thisPerson);
        }

        public IHttpActionResult Post ([FromBody] Person person, int deptID)
        {
            Setup.people.Add(person);
            Setup.departments.Where(dept => dept.Id == deptID).FirstOrDefault().people.Add(person);
            return Ok(person);
        }

        [HttpPost]
        [Route("PeopleController/test")]
        public Person test ([FromBody] Person person)
        {
            return person;
        }
    }
}
