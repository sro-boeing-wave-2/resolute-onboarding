using System.Collections.Generic;
using System.Threading.Tasks;
using OnBoarding.Models;

namespace OnBoarding.Services
{
    public interface ICredentialsService
    {
        Task CreateCredentials(Organisation Organisation);
        IEnumerable<Organisation> GetAllSignUp();
        Task<Organisation> GetSignUp(long id);
        Organisation GetAllOrganisation(string Organisation_name, string Email);
    }
}