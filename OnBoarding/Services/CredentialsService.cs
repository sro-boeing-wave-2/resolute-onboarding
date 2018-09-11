using OnBoarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnBoarding.Services
{
    public class CredentialsService : ICredentialsService
    {
        private readonly OnBoardingContext _context;

        public CredentialsService(OnBoardingContext context)
        {
            _context = context;
        }
        public IEnumerable<Organisation> GetAllSignUp()
        {
            return _context.Organisation;
        }
        public async Task<Organisation> GetSignUp(long id)
        {
            return await _context.Organisation.FindAsync(id);
        }

        public async Task CreateCredentials(Organisation organisation)
        {
            _context.Organisation.Add(organisation);
            await _context.SaveChangesAsync();
        }
        public Organisation GetAllOrganisation(string organisationName, string Email)
        {
            return  _context.Organisation.Where(

                element => element.OrganisationName == organisationName 
                || element.Email == Email
                ).ToList()[0];
                //.Where(x => ((Organisation_name == null || x.Organisation_name == Organisation_name) && (Email == null || x.Email == Email)));
        }
    }
}
