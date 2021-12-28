using Microsoft.AspNetCore.Mvc;
using school_management_system_API.Models;
using school_management_system_API.Services;
using System;

namespace school_management_system_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {

        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            this._studentService = studentService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_studentService.GetAll());
        }

        [HttpGet]
        [Route("[controller]/{id}")]
        public ActionResult GetById(int id)
        {
            var result = _studentService.GetById(id);

            if (result.Failure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public ActionResult Post([FromBody]Student student)
        {
            if(!ModelState.IsValid) return BadRequest();

            var result = _studentService.Create(student);

            if (result.Failure) return BadRequest(result.Error);

            return Created(new Uri($"{Request.Path}/{result.Value.Id}", UriKind.Relative), result.Value);

        }

        [HttpPut]
        public ActionResult Put([FromBody] Student  student, int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            student.Id = id;

            var result = _studentService.Update(student);

            if (result.Failure) return BadRequest(result.Error);


            return Ok();

        }

        [HttpDelete]
        public ActionResult Delete( int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _studentService.RemoveById(id);

            if (result.Failure) return BadRequest(result.Error);

            return NoContent();

        }


    }
}
