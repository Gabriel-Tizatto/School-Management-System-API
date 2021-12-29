using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using school_management_system_API.Models;
using school_management_system_API.Services;
using System;

namespace school_management_system_API.Controllers
{
    
    public class AddressController : ODataController
    {

        private readonly AddressService _addressService;

        public AddressController(AddressService studentService)
        {
            this._addressService = studentService;
        }

        [HttpGet]
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
        public ActionResult Put([FromBody] AddressBase  address, int key)
        {
            if (!ModelState.IsValid) return BadRequest();

            address.Id = key;

            var result = _addressService.Update(address);

            if (result.Failure) return BadRequest(result.Error);


            return Ok();

        }

        [HttpDelete]
        public ActionResult Delete( int key)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _addressService.RemoveById(key);

            if (result.Failure) return BadRequest(result.Error);

            return NoContent();

        }


    }
}
