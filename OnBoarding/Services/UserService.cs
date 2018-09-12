using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnBoarding.Models;
using System.Text.RegularExpressions;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;


namespace OnBoarding.Services
{
    public class UserService : IUserService
    {
        private readonly ResoluteContext _context;
        public static readonly HttpClient _client = new HttpClient();
        public UserService(ResoluteContext context)
        {
            _context = context;
        }
        //Agent Services
        public IEnumerable<Agent> GetAgent()
        {
            return _context.Agent.Include(x => x.Department).Include(x => x.Organization);
        }

        public async Task<Agent> GetAgent(int id)
        {
            return await _context.Agent.FindAsync(id);
        }
        public Agent GetAllAgents(string Name, string Email, string phonenumber)
        {
            string email = (Email == null) ? string.Empty : Email.Trim('\"').Trim('\\');
            string name = (Name == null) ? string.Empty : Name.Trim('\"').Trim('\\');
            string phoneNumber = (phonenumber == null) ? string.Empty : phonenumber.Trim('\"').Trim('\\');
            return _context.Agent.Where(
              element => element.Name == name
             || element.Email == email
             || element.Phonenumber == phoneNumber
              ).Include(x => x.Department).ToList()[0];
        }
        public async Task PostAgent([FromBody] Organisation organisation)
        {
            await ExtractData(organisation);
            
        }
        public async Task ExtractData(Organisation organisation)
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
            int indexOfPassword = Array.IndexOf(header, "Password");

            for (int i = 1; i <= contents.Count() - 1; i++)
            {
                string[] info = contents[i].Split(',');

                Agent agent = new Agent
                {
                    Name = info[indexOfName].Trim('\"'),
                    Email = info[indexOfEmail].Trim('\"'),
                    Phonenumber = info[indexOfPhoneNumber].Replace("\r", string.Empty).Trim('\"'),
                    ProfileImgUrl = info[indexOfProfileImage].Trim('\"'),
                    Department = _context.Department.FirstOrDefault(x => x.DepartmentName == info[indexOfDepartment].Replace("\r", string.Empty).Trim('\"')) ?? new Department { DepartmentName = info[indexOfDepartment].Replace("\r", string.Empty).Trim('\"'), CreatedOn = DateTime.Now, UpdatedOn = DateTime.Now },
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };
                string email = info[indexOfEmail].Trim('\"');
                string password = info[indexOfPassword].Trim('\"');

                AuthDto agentDetails = new AuthDto
                {
                    Email = info[indexOfEmail].Trim('\"'),
                    Password = "TestPassword@123"
                };

                HttpRequestMessage postMessage = new HttpRequestMessage(HttpMethod.Post, "http://35.189.155.116:8081/api/Auth/user/add")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(agentDetails), UnicodeEncoding.UTF8, "application/json")
                };
                var response = await _client.SendAsync(postMessage);
                var responseString = await response.Content.ReadAsStringAsync();
                _context.Agent.Add(agent);
                await _context.SaveChangesAsync();
            }
        }
        //public async Task PostUserInfoAsync(string email, string Password)
        //{
        //    Dictionary<string, string> auth = new Dictionary<string, string>();
        //    auth.Add("Username", email);
        //    auth.Add("TestPassword@123", Password);
        //    Console.WriteLine(JsonConvert.SerializeObject(auth));
        //    var response = await _client.PostAsync("http://35.189.155.116:8081/api/Auth/user/add", new StringContent(JsonConvert.SerializeObject(auth), UnicodeEncoding.UTF8, "application/json"));
        //}
        public async Task<string> ReadFileAsync(string filepath)
        {
            string fileData = "";
            using (StreamReader streamReader = new StreamReader(filepath))
            {
                fileData = await streamReader.ReadToEndAsync();
            }
            return fileData;
        }
       
        // EndUser Services
        public IEnumerable<EndUser> GetEndUser()
        {
            return _context.EndUser.Include(x => x.SocialId).Include(x => x.Organization);
        }

        // GET: api/EndUsers/5

        public async Task<EndUser> GetEndUser(int id)
        {
            return await _context.EndUser.Include(x => x.SocialId).Include(x => x.Organization).FirstOrDefaultAsync(x => x.Id == id);
        }

        // POST: api/EndUsers
        public async Task PostEndUser([FromBody] Organisation organisation)
        {
            await ExtractDataEndUser(organisation);
        }
        public EndUser GetAllEndUser(string Name, string Email, string phonenumber)
        {
            string email = (Email==null)?string.Empty: Email.Trim('\"').Trim('\\');
            string name = (Name == null) ? string.Empty : Name.Trim('\"').Trim('\\');
            string phoneNumber = (phonenumber == null) ? string.Empty : phonenumber.Trim('\"').Trim('\\');
            return _context.EndUser.Where(
                element => element.Name == name
               || element.Email == email
               || element.Phonenumber == phoneNumber
                ).Include(x => x.SocialId).ToList()[0];
        }
        public async Task ExtractDataEndUser(Organisation organisation)
        {
            string filePathCSV = @"./wwwroot/Upload/EndUser.csv";
            Task<string> fileData = ReadFileEndUserAsync(filePathCSV);
            await fileData;
            string[] contents = fileData.Result.Split('\n');
            int countOfSocialIds = Regex.Matches(contents[0], "/Source").Count;
            string[] header = contents[0].Split(',');
            for (int i = 0; i < header.Length; i++)
            {
                header[i] = header[i].Replace("\r", string.Empty).Trim('\"');
            }
            int indexOfName = Array.IndexOf(header, "Name");
            int indexOfEmail = Array.IndexOf(header, "Email");
            int indexOfPhoneNumber = Array.IndexOf(header, "PhoneNumber");
            int indexOfProfileImage = Array.IndexOf(header, "ProfileImgUrl");
            int[] indexOfSocialAccountSource = new int[countOfSocialIds];
            int[] indexOfSocialAccountIdentifier = new int[countOfSocialIds];
            for (int i = 0; i < countOfSocialIds; i++)
            {
                indexOfSocialAccountSource[i] = Array.IndexOf(header, $"SocialId/{i}/Source");
                indexOfSocialAccountIdentifier[i] = Array.IndexOf(header, $"SocialId/{i}/Identifier");
            }

            for (int i = 1; i <= contents.Count() - 1; i++)
            {
                string[] info = contents[i].Split(',');
                EndUser endUser = new EndUser
                {
                    Name = info[indexOfName].Trim('\"'),
                    Email = info[indexOfEmail].Trim('\"'),
                    Phonenumber = info[indexOfPhoneNumber].Trim('\"'),
                    ProfileImgUrl = info[indexOfProfileImage].Replace("\r", string.Empty).Trim('\"'),
                    SocialId = new List<UserSocialId>(),
                    Organization = _context.organisation.FirstOrDefault(x => x.OrganisationName == organisation.OrganisationName) ?? organisation,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };
                for (int j = 0; j < countOfSocialIds; j++)
                {
                    if (info[indexOfSocialAccountSource[j]].Trim('\"') != string.Empty && info[indexOfSocialAccountIdentifier[j]].Trim('\"') != string.Empty)
                    {
                        endUser.SocialId.Add(new UserSocialId
                        {
                            Source = info[indexOfSocialAccountSource[j]].Trim('\"'),
                            Identifier = info[indexOfSocialAccountIdentifier[j]].Trim('\"'),
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now
                        });
                    }
                }
                _context.EndUser.Add(endUser);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<string> ReadFileEndUserAsync(string filepath)
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