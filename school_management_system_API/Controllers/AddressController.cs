using Microsoft.AspNetCore.Mvc;
using school_management_system_API.Models;
using school_management_system_API.Services;
using System;

namespace school_management_system_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
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
        [Route("[controller]/{id}")]
        public ActionResult GetById(int id)
        {
            var result = _addressService.GetById(id);

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
        public ActionResult Put([FromBody] AddressBase  address, int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            address.Id = id;

            var result = _addressService.Update(address);

            if (result.Failure) return BadRequest(result.Error);


            return Ok();

        }

        [HttpDelete]
        public ActionResult Delete( int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _addressService.RemoveById(id);

            if (result.Failure) return BadRequest(result.Error);

            return NoContent();

        }


    }
}
