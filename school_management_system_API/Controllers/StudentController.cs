﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using school_management_system_API.Models;
using school_management_system_API.Services;
using System;

namespace school_management_system_API.Controllers
{
    public class StudentController : ODataController
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
        [Route("[controller]/{key}")]
        [Route("[controller]({key})")]
        public ActionResult GetById(int key)
        {
            var result = _studentService.GetById(key);

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
        public ActionResult Put([FromBody] Student  student, int key)
        {
            if (!ModelState.IsValid) return BadRequest();

            student.Id = key;

            var result = _studentService.Update(student);

            if (result.Failure) return BadRequest(result.Error);


            return Ok();

        }

        [HttpDelete]
        public ActionResult Delete( int key)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _studentService.RemoveById(key);

            if (result.Failure) return BadRequest(result.Error);

            return NoContent();

        }


    }
}
