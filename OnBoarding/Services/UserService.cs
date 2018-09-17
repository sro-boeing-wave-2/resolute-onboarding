﻿using System;
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
            try
            {
                return _context.Agent.Include(x => x.Department).Include(x => x.Organization);
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Data not found");
                throw e;
            }
        }

        public async Task<Agent> GetAgent(int id)
        {
            try
            {
                return await _context.Agent.FindAsync(id);
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Data not found");
                throw e;
            }
        }
        public Agent GetAllAgents(string Name, string Email, string phonenumber)
        {
            try
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
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Data not found");
                throw e;
            }
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
            //int indexOfPassword = Array.IndexOf(header, "Password");

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

                AuthDto agentDetails = new AuthDto
                {
                    Username = info[indexOfEmail].Trim('\"'),
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
        public async Task<string> ReadFileAsync(string filepath)
        {
            try
            {
                string fileData = "";
                using (StreamReader streamReader = new StreamReader(filepath))
                {
                    fileData = await streamReader.ReadToEndAsync();
                }
                return fileData;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("File not found");
                throw ex;
            }
        }

        // EndUser Services
        public IEnumerable<EndUser> GetEndUser()
        {
            try
            {
                return _context.EndUser.Include(x => x.SocialId).Include(x => x.Organization);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("data not found");
                throw ex;
            }

        }

        // GET: api/EndUsers/5

        public async Task<EndUser> GetEndUser(int id)
        {
            try
            {
                return await _context.EndUser.Include(x => x.SocialId).Include(x => x.Organization).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("data not found");
                throw ex;
            }
        }

        // POST: api/EndUsers
        public async Task PostEndUser([FromBody] Organisation organisation)
        {
            await ExtractDataEndUser(organisation);
        }

        public EndUser GetAllEndUser(string Name, string Email, string phonenumber)
        {
            try
            {
                string email = (Email == null) ? string.Empty : Email.Trim('\"').Trim('\\');
                string name = (Name == null) ? string.Empty : Name.Trim('\"').Trim('\\');
                string phoneNumber = (phonenumber == null) ? string.Empty : phonenumber.Trim('\"').Trim('\\');
                return _context.EndUser.Where(
                    element => element.Name == name
                   || element.Email == email
                   || element.Phonenumber == phoneNumber
                    ).Include(x => x.SocialId).ToList()[0];
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("data not found");
                throw ex;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("duplicate data found");
                throw ex;
            }
        }
        public async Task ExtractDataEndUser(Organisation organisation)
        {
            try
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
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
            catch (IndexOutOfRangeException ex)
            {
                throw ex;
            }
            catch (ArrayTypeMismatchException ex)
            {
                throw ex;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }

        }
        public async Task<string> ReadFileEndUserAsync(string filepath)
        {
            try
            {
                string fileData = "";
                using (StreamReader streamReader = new StreamReader(filepath))
                {
                    fileData = await streamReader.ReadToEndAsync();
                }
                return fileData;
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
        }
    }
}