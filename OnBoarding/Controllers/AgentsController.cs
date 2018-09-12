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
        private readonly IUserService _service;

        public AgentsController(IUserService service)
        {
            _service = service;
        }

        // GET: api/Agents
        [HttpGet]
        public IEnumerable<Agent> GetAgent()
        {
            return _service.GetAgent();
            /*Include(x => x.Department).Include(x => x.Organization);*/
        }

        // GET: api/Agents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAgent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var agent = await _service.GetAgent(id);

            return Ok(agent);
        }

        [HttpGet("query")]
        public async Task<IActionResult> GetAgentByQuery([FromQuery(Name = "Name")] string Name, [FromQuery(Name = "Email")] string Email, [FromQuery(Name = "phonenumber")] string phonenumber)
        {
            try
            {
                var result = _service.GetAllAgents(Name, Email, phonenumber);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Result Not Found");
                return BadRequest(ModelState);
            }
        }

        // POST: api/Agents
        [HttpPost]
        public async Task<IActionResult> PostAgent([FromBody] Organisation organisation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await ExtractData(organisation);
        }
        public async Task<IActionResult> ExtractData(Organisation organisation)
        {
            await _service.ExtractData(organisation);
            return Ok();
        }
        public static async Task<string> ReadFileAsync(string filepath)
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