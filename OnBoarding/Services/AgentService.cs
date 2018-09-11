using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnBoarding.Models;

namespace OnBoarding.Services
{
    public class AgentService : IAgentService
    {
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
                    Department = _context.Department.FirstOrDefault(x => x.DepartmentName == info[indexOfDepartment].Trim('\"')) ?? new Department { DepartmentName = info[indexOfDepartment].Trim('\"'), CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now },
                    Organization = _context.Organisation.FirstOrDefault(x => x.OrganisationName == Organisation.OrganisationName) ?? Organisation,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                _context.Agent.Add(agent);
                await _context.SaveChangesAsync();
            }
        }
        public IEnumerable<Agent> RetrieveAgent()
        {
            return _context.Agent.Include(x => x.Department).Include(x => x.Organization);
        }

        public async Task<Agent> RetrieveAgentById(long id)
        {
            return await _context.Agent.Include(x=>x.Department).Include(x=>x.Organization).FirstOrDefaultAsync(x=>x.Id==id);
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
                OrganisartionId = agent.Organization.Id
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
