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
            try
            {
                return _context.Organisation;
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("data not found");
                throw ex;
            }
        }
        public async Task<Organisation> GetSignUp(long id)
        {
            try
            {
                return await _context.Organisation.FindAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("data not found");
                throw ex;
            }
        }

        public async Task CreateCredentials(Organisation organisation)
        {
            try
            {
                _context.Organisation.Add(organisation);
                await _context.SaveChangesAsync();
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("null reference exception occured");
                throw ex;
            }
        }
        public Organisation GetAllOrganisation(string organisationName, string Email)
        {
            try
            {
                Organisation temp;
                return temp = _context.Organisation.Where(element => element.OrganisationName == (organisationName ?? element.OrganisationName)
                         && element.Email == (Email ?? element.Email)).ToList()[0];
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
