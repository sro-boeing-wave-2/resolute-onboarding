using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.EntityFrameworkCore;
using OnBoarding.Models;
using OnBoarding.Services;

namespace OnBoarding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentService _service;

        public AgentsController(IAgentService service)
        {
            _service = service;
        }

        // GET: api/Agents
        [HttpGet]
        public IEnumerable<Agent> GetAgents()
        {
            return _service.RetrieveAgent();
        }
        [HttpGet("query")]
        public async Task<IActionResult> GetAgentAsync([FromQuery(Name = "Name")] string Name, [FromQuery(Name = "Email")] string Email, [FromQuery(Name = "phonenumber")] string PhoneNumber)
        {
            string email = _service.TrimInput(Email);
            string name = _service.TrimInput(Name);
            string phoneNumber = _service.TrimInput(PhoneNumber);
            return Ok(await _service.RetrieveAgentDto(email, name, phoneNumber));

        }
        [HttpGet("GetName")]
        public string GetAgentName([FromQuery(Name = "Id")] long id)
        {
            return _service.GetUserName(id);
        }
        [HttpGet("Count")]
        public long GetAgentCount([FromQuery] long id)
        {
            return _service.GetUserCount(id);
        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetAgentDto([FromQuery] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AgentDto agent = await _service.RetrieveAgentDtoById(id);


            if (agent == null)
            {
                return NotFound();
            }

            return Ok(agent);
        }

        // GET: api/Agents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAgent([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Agent agent = await _service.RetrieveAgentById(id);


            if (agent == null)
            {
                return NotFound();
            }

            return Ok(agent);
        }

        // POST: api/Agents
        [HttpPost]
        public async Task<IActionResult> PostAgent([FromBody] Organisation Organisation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await ExtractData(Organisation);
        }
        public async Task<IActionResult> ExtractData(Organisation organisation)
        {
            await _service.ExtractData(organisation);
            return Ok();
        }

    }
}