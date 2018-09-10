using System;
using Xunit;
using OnBoarding.Models;
using OnBoarding.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using System.Linq;
using OnBoarding.Services;
using System.Threading.Tasks;


namespace OnBoardingTestCases
{
    public class SignUpTest
    {
        public static SignupController GetController()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ResoluteContext >();
            optionsBuilder.UseInMemoryDatabase<ResoluteContext >(Guid.NewGuid().ToString());
            ResoluteContext  ResoluteContext  = new ResoluteContext (optionsBuilder.Options);
            CredentialsService service = new CredentialsService(ResoluteContext );
            GetOrganisationSignUp(optionsBuilder.Options);
            return new SignupController(service);
        }

        public static void GetOrganisationSignUp(DbContextOptions<ResoluteContext > options)
        {
            using (var signupContext = new ResoluteContext (options))
            {
                var organisation = new List<Organisation>()
                {
                    new Organisation()
                    {
                        Id=1,
                          OrganisationName="Boeing",
                            Email= "Message@Boeing.in",
                            Password= "Boeing",
                            LogoUrl= "www.efgh.Boeing.in"
                    }            ,
                     new Organisation()
                    {
                         Id=2,
                          OrganisationName="Boeing",
                            Email= "Message@Boeing.in",
                            Password= "Boeing",
                            LogoUrl= "www.abcd.Boeing.in"
                    },
                      new Organisation()
                    {
                          Id=3,
                          OrganisationName="Boeing",
                            Email= "organisation@Boeing.in",
                            Password= "Boeing",
                            LogoUrl= "www.logo.Boeing.in"
                    }
                };
                signupContext.organisation.AddRange(organisation);
                var CountOfEntitiesBeingTracked = signupContext.ChangeTracker.Entries().Count();
                signupContext.SaveChanges();
            }
        }
        [Fact]
        public void TestGet()
        {
            var _controller = GetController();
            var res = _controller.GetOrganisationSignUp();
            Console.WriteLine(res.ToList().Count);
            Assert.Equal(3, res.ToList().Count);
        }

        [Fact]
        public async Task PostOrganisationSignUp()
        {
            var organisation = new Organisation
            {
                Id=4,
                OrganisationName = "Stackroute",
                Email = "organisation@Boeing.in",
                Password = "Boeing",
                LogoUrl = "www.logo.Boeing.in"

            };
            var _controller = GetController();
            var result = _controller.PostOrganisationSignUp(organisation).Result as CreatedAtActionResult;
            var item = result.Value as Organisation;
            Assert.Equal("Stackroute", item.OrganisationName);
        }
        [Fact]
        public async Task TestGetByID()
        {
            var _controller = GetController();
            var res = await _controller.GetOrganisationSignUp(1) as OkObjectResult;
            Organisation result = res.Value as Organisation;
            Assert.Equal("Boeing", result.OrganisationName);
        }
        [Fact]
        public async Task TestGetByQuery()
        {
            var _controller = GetController();
            var res = await _controller.GetorganisationByQuery("Boeing", "organisation@Boeing.in") as OkObjectResult;
            Organisation result = res.Value as Organisation;
            Assert.Equal("www.logo.Boeing.in", result.LogoUrl);
        }
    }
}