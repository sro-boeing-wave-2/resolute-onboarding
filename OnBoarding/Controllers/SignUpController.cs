using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnBoarding.Models;
using OnBoarding.Services;

namespace OnBoarding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        private ICredentialsService _service;
        public SignupController(ICredentialsService service)
        {
            _service = service;
        }
        // GET: api/Organisation_Signup
        [HttpGet]
        public IEnumerable<Organisation> GetOrganisation()
        {
            return _service.GetAllSignUp();
        }
        // GET: api/Organisation_Signup/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganisation([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Organisation Organisation = await _service.GetSignUp(id);
            if (Organisation == null)
            {
                return NotFound();
            }
            return Ok(Organisation);
        }

        // POST: api/Organisation_Signup
        [HttpPost]
        public async Task<IActionResult> PostOrganisation([FromBody] Organisation Organisation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _service.CreateCredentials(Organisation);
            return CreatedAtAction("GetOrganisation", new { id = Organisation.Id }, Organisation);
        }
    }
}