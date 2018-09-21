using System.Collections.Generic;
using System.Threading.Tasks;
using OnBoarding.Models;

namespace OnBoarding.Services
{
    public interface IUserService
    {
        Task ExtractData(Organisation Organisation);
        string TrimInput(string Input);
        string GetUserName(long id);
    }
    public interface IEndUserService : IUserService
    {
        IEnumerable<EndUser> RetrieveUser();
        Task<EndUser> RetrieveUserById(long id);
        Task<EndUserDto> RetrieveUserDto(string email, string name, string phoneNumber);
    }
    public interface IAgentService : IUserService
    {
        IEnumerable<Agent> RetrieveAgent();
        Task<Agent> RetrieveAgentById(long id);
        Task<AgentDto> RetrieveAgentDtoById(long id);
        Task<AgentDto> RetrieveAgentDto(string email, string name, string phoneNumber);
    }
}