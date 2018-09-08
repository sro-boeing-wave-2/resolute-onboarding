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
        // GET: api/OrganisationSignUp
        [HttpGet]
        public IEnumerable<Organisation> GetOrganisationSignUp()
        {
            return _service.GetAllSignUp();
        }
        // GET: api/OrganisationSignUp/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganisationSignUp([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Organisation OrganisationSignUp = await _service.GetSignUp(id);
            if (OrganisationSignUp == null)
            {
                return NotFound();
            }
            return Ok(OrganisationSignUp);
        }

        [HttpGet("query")]
        public async Task<IActionResult> GetorganisationByQuery([FromQuery(Name = "organisationName")] string organisationName, [FromQuery(Name = "Email")] string Email)
        {
            var result = _service.GetAllorganisation(organisationName, Email);
            return Ok(result);
        }

        // POST: api/OrganisationSignUp
        [HttpPost]
        public async Task<IActionResult> PostOrganisationSignUp([FromBody] Organisation organisation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _service.CreateCredentials(organisation);
            return CreatedAtAction("GetOrganisationSignUp", new { id = organisation.Id }, organisation);
        }
    }
}