using System.Collections.Generic;
using System.Threading.Tasks;
using OnBoarding.Models;

namespace OnBoarding.Services
{
    public interface ICredentialsService
    {
        Task CreateCredentials(Organisation OrganisationSignUp);
        IEnumerable<Organisation> GetAllSignUp();
        Task<Organisation> GetSignUp(int id);
        Organisation GetAllorganisation(string organisationName, string Email);
    }
}