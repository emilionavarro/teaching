using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PersonWebAPI.Models;

namespace PersonWebAPI.Controllers
{
    public class JobsController : ApiController
    {
        public IEnumerable<Job> GetAllDepartments()
        {
            Setup.BuildDepartments();
            return Setup.jobs;
        }

        public IHttpActionResult GetDepartment(int id)
        {
            Job thisJob = Setup.jobs.Where(job => job.Id == id).FirstOrDefault();
            if (thisJob == null)
                return NotFound();
            return Ok(thisJob);
        }
    }
}
