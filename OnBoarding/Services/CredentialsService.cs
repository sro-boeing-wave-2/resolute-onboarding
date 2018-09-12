using OnBoarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnBoarding.Services
{
    public class CredentialsService : ICredentialsService
    {
        private readonly ResoluteContext  _context;

        public CredentialsService(ResoluteContext  context)
        {
            _context = context;
        }
        public IEnumerable<Organisation> GetAllSignUp()
        {
            return _context.organisation;
        }
        public async Task<Organisation> GetSignUp(int id)
        {
            return await _context.organisation.FindAsync(id);
        }

        public async Task CreateCredentials(Organisation organisationSignup)
        {
            _context.organisation.Add(organisationSignup);
            await _context.SaveChangesAsync();
        }
        public Organisation GetAllorganisation(string organisationName, string Email)
        {
            //return _context.organisation.Where(
            //     element => element.organisationName == organisationName
            //    || element.Email == Email
            //    ).ToList()[0];

            Organisation temp;
           
           return temp = _context.organisation.Where(element => element.OrganisationName == (organisationName == null ? element.OrganisationName : organisationName)
                     && element.Email == (Email == null ? element.Email : Email)).ToList()[0];

        }
    }
}
