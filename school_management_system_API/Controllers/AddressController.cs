using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using school_management_system_API.Models;
using school_management_system_API.Services;
using System;

namespace school_management_system_API.Controllers
{
    [EnableCors("AllowAll")]
    public class AddressController : BaseController
    {

        private readonly AddressService _addressService;

        public AddressController(AddressService studentService)
        {
            this._addressService = studentService;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult Get()
        {
            return Ok(_addressService.GetAll());
        }

        [HttpGet]
        [Route("[controller]/{key}")]
        [Route("[controller]({key})")]
        public ActionResult GetById(int key)
        {
            var result = _addressService.GetById(key);

            if (result.Failure) return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public ActionResult Post([FromBody]AddressBase address)
        {
            if(!ModelState.IsValid) return BadRequest();

            var result = _addressService.Create(address);

            if (result.Failure) return BadRequest(result.Error);

            return Created(new Uri($"{Request.Path}/{result.Value.Id}", UriKind.Relative), result.Value);

        }

        [HttpPut]
        [Authorize]
        public ActionResult Put([FromBody] AddressBase  address, int key)
        {
            if (!ModelState.IsValid) return BadRequest();

            address.Id = key;

            var result = _addressService.Update(address, SchoolId);

            if (result.Failure) return BadRequest(result.Error);


            return Ok();

        }

        [HttpDelete]
        [Authorize]
        public ActionResult Delete( int key)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _addressService.RemoveById(key, SchoolId);

            if (result.Failure) return BadRequest(result.Error);

            return NoContent();

        }


    }
}
