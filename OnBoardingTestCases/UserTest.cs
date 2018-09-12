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
    public class UserTest
    {
        public static AgentsController GetController()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ResoluteContext>();
            optionsBuilder.UseInMemoryDatabase<ResoluteContext>(Guid.NewGuid().ToString());
            ResoluteContext UserContext = new ResoluteContext(optionsBuilder.Options);
            UserService service = new UserService(UserContext);
            GetUser(optionsBuilder.Options);
            return new AgentsController(service);
        }

        public static void GetUser(DbContextOptions<ResoluteContext> options)
        {
            using (var UserContext = new ResoluteContext(options))
            {
                var agents = new List<Agent>()
                {
                      new Agent(){
                   Role= null,
                   Name= "Nishant",
                   Email= "nishant@stackroute.in",
                   Phonenumber= "1234567890",
                   ProfileImgUrl= "www.profileimgnishant.in",
                   Organization= null
                },
                     new Agent(){
                   Role= null,
                   Name= "Nishant",
                   Email= "nishant@stackroute.in",
                   Phonenumber= "1234567890",
                   ProfileImgUrl= "www.profileimgnishant.in",
                   Organization= null
                },
                    new Agent(){
                   Role= null,
                   Name= "Nishant",
                   Email= "nishant@stackroute.in",
                   Phonenumber= "1234567890",
                   ProfileImgUrl= "www.profileimgnishant.in",
                   Organization= null
                }
                };
                UserContext.Agent.AddRange(agents);
                var CountOfEntitiesBeingTracked = UserContext.ChangeTracker.Entries().Count();
                UserContext.SaveChanges();
            }
        }
        [Fact]
        public void TestGet()
        {
            var _controller = GetController();
            var res = _controller.GetAgent();
            Console.WriteLine(res.ToList().Count);
            Assert.Equal(3, res.ToList().Count);
        }

        [Fact]
        public async Task TestGetByID()
        {
            var _controller = GetController();
            var res = await _controller.GetAgent(1) as OkObjectResult;
            Organisation result = res.Value as Organisation;
            Assert.Equal("Boeing", result.OrganisationName);
        }
        [Fact]
        public async Task TestGetByQuery()
        {
            var _controller = GetController();
            var res = await _controller.GetAgentByQuery("Boeing", "organisation@Boeing.in", "6234578732") as OkObjectResult;
            Organisation result = res.Value as Organisation;
            Assert.Equal("www.logo.Boeing.in", result.LogoUrl);
        }
    }
}
