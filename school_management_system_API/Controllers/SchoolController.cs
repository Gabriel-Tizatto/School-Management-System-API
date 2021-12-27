using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using school_management_system_API.Models;
using school_management_system_API.Services;
using System;
using System.Linq;

namespace school_management_system_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SchoolController : ControllerBase
    {

        private readonly SchoolService _schoolService;

        public SchoolController(SchoolService schoolService)
        {
            this._schoolService = schoolService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_schoolService.GetAllSchools());
        }

        [HttpPost]
        public ActionResult Post([FromBody]School school)
        {
            if(!ModelState.IsValid) return BadRequest();

            if(!_schoolService.CreateSchool(school)) return BadRequest();

            return Created(new Uri($"{Request.Path}/{school.Id}", UriKind.Relative), school);

        }

        [HttpPut]
        public ActionResult Put([FromBody] School school, int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            school.Id = id;

            if (!_schoolService.UpdateSchool(school)) return BadRequest();

            return Ok();

        }

        [HttpDelete]
        public ActionResult Delete( int id)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            if (!_schoolService.RemoveSchoolById(id)) return BadRequest();

            return NoContent();

        }


    }
}
