using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnBoarding.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace OnBoarding.Services
{
    public class AgentService : IAgentService
    {
        public static readonly HttpClient _client = new HttpClient();
        private readonly OnBoardingContext _context;
        public AgentService(OnBoardingContext context)
        {
            _context = context;
        }

        public async Task ExtractData(Organisation Organisation)
        {
            string filePathCSV = @"./wwwroot/Upload/agent.csv";
            Task<string> fileData = ReadFileAsync(filePathCSV);
            await fileData;
            string[] contents = fileData.Result.Split('\n');


            string[] header = contents[0].Split(',');
            for (int i = 0; i < header.Length; i++)
            {
                header[i] = header[i].Replace("\r", string.Empty).Trim('\"');
            }
            int indexOfName = Array.IndexOf(header, "Name");
            int indexOfEmail = Array.IndexOf(header, "Email");
            int indexOfPhoneNumber = Array.IndexOf(header, "PhoneNumber");
            int indexOfProfileImage = Array.IndexOf(header, "ProfileImg");
            int indexOfDepartment = Array.IndexOf(header, "Department");

            for (int i = 1; i <= contents.Count() - 1; i++)
            {
                string[] info = contents[i].Split(',');

                Agent agent = new Agent
                {
                    Name = info[indexOfName].Trim('\"'),
                    Email = info[indexOfEmail].Trim('\"'),
                    PhoneNumber = info[indexOfPhoneNumber].Trim('\"'),
                    ProfileImgUrl = info[indexOfProfileImage].Trim('\"'),
                    Department = _context.Department.FirstOrDefault(x => x.DepartmentName == info[indexOfDepartment].Replace("\r", string.Empty).Trim('\"')) ?? new Department { DepartmentName = info[indexOfDepartment].Replace("\r", string.Empty).Trim('\"'), CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now },
                    Organization = _context.Organisation.FirstOrDefault(x => x.OrganisationName == Organisation.OrganisationName) ?? Organisation,
                    UpdatedOn = DateTime.Now
                };

                AuthDto agentDetails = new AuthDto
                {
                    Username = info[indexOfEmail].Trim('\"'),
                    Password = "TestPassword@123"
                };

               
                HttpRequestMessage postMessage = new HttpRequestMessage(HttpMethod.Post, "http://35.221.125.153:8081/api/Auth/user/add")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(agentDetails), UnicodeEncoding.UTF8, "application/json")
                };
                var response = await _client.SendAsync(postMessage);
                var responseString = await response.Content.ReadAsStringAsync();

                _context.Agent.Add(agent);
                await _context.SaveChangesAsync();
            }
        }

        public string GetUserName(long id)
        {
            return _context.Agent.FirstOrDefault(x => x.Id == id).Name;
        }

        public IEnumerable<Agent> RetrieveAgent()
        {
            return _context.Agent.Include(x => x.Department).Include(x => x.Organization);
        }

        public async Task<Agent> RetrieveAgentById(long id)
        {
            return await _context.Agent.Include(x => x.Department).Include(x => x.Organization).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AgentDto> RetrieveAgentDto(string email, string name, string phoneNumber)
        {
            Agent agent = await _context.Agent.Include(x => x.Department)
                .Include(x => x.Organization).FirstOrDefaultAsync(AgentMatches(email, name, phoneNumber));
            AgentDto agentDto = new AgentDto
            {
                AgentId = agent.Id,
                Name = agent.Name,
                ProfileImageUrl = agent.ProfileImgUrl,
                OrganisationId = agent.Organization.Id,
                DepartmentName = agent.Department.DepartmentName,
                OrganisationName = agent.Organization.OrganisationName
            };

            return agentDto;
        }


        public string TrimInput(string Input)
        {
            return (Input == null) ? string.Empty : Input.Trim('\"').Trim('\\');
        }

        private static async Task<string> ReadFileAsync(string filepath)
        {
            string fileData = "";
            using (StreamReader streamReader = new StreamReader(filepath))
            {
                fileData = await streamReader.ReadToEndAsync();
            }
            return fileData;
        }

        private System.Linq.Expressions.Expression<Func<Agent, bool>> AgentMatches(string email, string name, string phoneNumber)
        {
            return element => element.Name == name
                                     || element.Email == email
                                     || element.PhoneNumber == phoneNumber;
        }
    }
}
