using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnBoarding.Models;
using System.IO;
using System.Text.RegularExpressions;
using OnBoarding.Services;

namespace OnBoarding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EndUsersController : ControllerBase
    {

        private IEndUserService _service;
        public EndUsersController(IEndUserService service)
        {
            _service = service;
        }

        // GET: api/EndUsers
        [HttpGet]
        public IEnumerable<EndUser> GetEndUser()
        {
            return _service.RetrieveUser();
        }

       

        // GET: api/EndUsers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEndUser([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            EndUser endUser = await _service.RetrieveUserById(id);

            if (endUser == null)
            {
                return NotFound();
            }

            return Ok(endUser);
        }



        [HttpGet("query")]
        public async Task<IActionResult> GetEndUser([FromQuery(Name = "Name")] string Name, [FromQuery(Name = "Email")] string Email, [FromQuery(Name = "phonenumber")] string PhoneNumber)
        {
            string email = _service.TrimInput(Email);
            string name = _service.TrimInput(Name);
            string phoneNumber = _service.TrimInput(PhoneNumber);
            return Ok(await _service.RetrieveUserDto(email, name, phoneNumber));

        }

       


        // POST: api/EndUsers
        [HttpPost]
        public async Task<IActionResult> PostEndUser([FromBody] Organisation Organisation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _service.ExtractData(Organisation);
            return Ok();
        }

    }
}