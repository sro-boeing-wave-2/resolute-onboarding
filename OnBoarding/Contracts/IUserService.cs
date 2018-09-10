using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnBoarding.Models;

namespace OnBoarding.Services
{
    public interface IUserService
    {
        //Agent Services

        Task<string> ReadFileAsync(string filepath);
        Task ExtractData(Organisation organisation);
        IEnumerable<Agent> GetAgent();
        Task<Agent> GetAgent(int id);
        Task PostAgent(Organisation organisation);
        Agent GetAllAgents(string Name, string Email, string phonenumber);

        //EndUser Services

        IEnumerable<EndUser> GetEndUser();
        Task<EndUser> GetEndUser(int id);
        Task PostEndUser(Organisation organisation);
        Task ExtractDataEndUser(Organisation organisation);
        Task<string> ReadFileEndUserAsync(string filepath);
        EndUser GetAllEndUser(string Name, string Email, string phonenumber);
    }
}

