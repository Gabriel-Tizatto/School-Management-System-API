using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using school_management_system_API.Models;
using school_management_system_API.Services;
using System;

namespace school_management_system_API.Controllers
{
    [EnableCors("AllowAll")]
    public class SchoolController : BaseController
    {

        private readonly SchoolService _schoolService;

        public SchoolController(SchoolService schoolService)
        {
            this._schoolService = schoolService;
        }

        //[HttpGet]
        //public ActionResult Get()
        //{
        //    return Ok(_schoolService.GetAll());
        //}

        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            var result = _schoolService.GetById(SchoolId);

            if (result.Failure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        //[HttpGet]
        //[Route("[controller]/{key}")]
        //[Route( "[controller]({key})")]

        //public ActionResult GetById(int key)
        //{
        //    var result = _schoolService.GetById(key);

        //    if (result.Failure) return BadRequest(result.Error);

        //    return Ok(result.Value);
        //}

        [HttpPost]
        public ActionResult Post([FromBody]School school)
        {
            if(!ModelState.IsValid) return BadRequest();

            var result = _schoolService.Create(school);

            if (result.Failure) return BadRequest(result.Error);

            return Created(new Uri($"{Request.Path}/{result.Value.Id}", UriKind.Relative), result.Value);

        }

        [HttpPut]
        public ActionResult Put([FromBody] School school)
        {
            if (!ModelState.IsValid) return BadRequest();

            school.Id = SchoolId;

            var result = _schoolService.Update(school);

            if (result.Failure) return BadRequest(result.Error);


            return Ok();

        }

        [HttpDelete]
        public ActionResult Delete()
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _schoolService.RemoveById(SchoolId);

            if (result.Failure) return BadRequest(result.Error);

            return NoContent();

        }


    }
}
