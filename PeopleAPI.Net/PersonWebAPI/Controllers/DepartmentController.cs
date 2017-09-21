using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PersonWebAPI.Models;

namespace PersonWebAPI.Controllers
{
    public class DepartmentController : ApiController
    {
        public IEnumerable<Department> GetAllDepartments()
        { 
            return Setup.departments;
        }

        public IHttpActionResult GetDepartment(int id)
        {
            Department thisDepartment = Setup.departments.Where(dept => dept.Id == id).FirstOrDefault();
            if (thisDepartment == null)
                return NotFound();
            return Ok(thisDepartment);
        }
    }
}
