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
        public IEnumerable<Person> GetAllPeople()
        {
            Setup.BuildDepartments();
            return Setup.people;
        }

        public IHttpActionResult GetPerson (int id)
        {
            Person thisPerson = Setup.people.Where(person => person.Id == id).FirstOrDefault();
            if (thisPerson == null)
                return NotFound();
            return Ok(thisPerson);
        }
    }
}
