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

        private readonly IUserService _service;

        public EndUsersController(IUserService service)
        {
            _service = service;
        }
        // GET: api/EndUsers
        [HttpGet]
        public IEnumerable<EndUser> GetEndUser()
        {
            return _service.GetEndUser();
        }

        // GET: api/EndUsers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEndUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var endUser = await _service.GetEndUser(id);
            return Ok(endUser);
        }


        // POST: api/EndUsers
        [HttpPost]
        public async Task<IActionResult> PostEndUser([FromBody] Organisation organisation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await ExtractDataEndUser(organisation);
        }
        [HttpGet("query")]
        public async Task<IActionResult> GetEndUserByQuery([FromQuery(Name = "Name")] string Name, [FromQuery(Name = "Email")] string Email, [FromQuery(Name = "phonenumber")] string phonenumber)
        {
            var result = _service.GetAllEndUser(Name, Email, phonenumber);
            return Ok(result);
        }
        public async Task<IActionResult> ExtractDataEndUser(Organisation organisation)
        {
            await _service.ExtractDataEndUser(organisation);
            return Ok();
        }
        public static async Task<string> ReadFileEndUserAsync(string filepath)
        {
            string fileData = "";
            using (StreamReader streamReader = new StreamReader(filepath))
            {
                fileData = await streamReader.ReadToEndAsync();
            }
            return fileData;
        }
    }
}