using OnBoarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnBoarding.Services
{
    public class CredentialsService : ICredentialsService
    {
        private readonly ResoluteContext _context;

        public CredentialsService(ResoluteContext context)
        {
            _context = context;
        }
        public IEnumerable<Organisation> GetAllSignUp()
        {
            try
            {
                return _context.organisation;
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("data not found");
                throw ex;
            }
        }
        public async Task<Organisation> GetSignUp(int id)
        {
            try
            {
                return await _context.organisation.FindAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("data not found");
                throw ex;
            }
        }

        public async Task CreateCredentials(Organisation organisationSignup)
        {
            try
            {
                _context.organisation.Add(organisationSignup);
                await _context.SaveChangesAsync();
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("null reference exception occured");
                throw ex;
            }
        }
        public Organisation GetAllorganisation(string organisationName, string Email)
        {
            try
            {
                Organisation temp;
                return temp = _context.organisation.Where(element => element.OrganisationName == (organisationName == null ? element.OrganisationName : organisationName)
                         && element.Email == (Email == null ? element.Email : Email)).ToList()[0];
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
    }
}
