using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnBoarding.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnBoarding.Services
{
    public class EndUserService : IEndUserService
    {
        private readonly OnBoardingContext _context;
        public EndUserService(OnBoardingContext context)
        {
            _context = context;
        }
        public IEnumerable<EndUser> RetrieveUser()
        {
            return _context.EndUser.Include(x => x.SocialId).Include(x => x.Organization);
        }

        public async Task<EndUser> RetrieveUserById(long id)
        {
            return await _context.EndUser.Include(x => x.SocialId).Include(x => x.Organization).FirstOrDefaultAsync(x => x.Id == id);
        }

        public string GetUserName(long id)
        {
            try
            {
                return _context.EndUser.FirstOrDefault(x => x.Id == id).Name;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<EndUserDto> RetrieveUserDto(string email, string name, string phoneNumber)
        {
            EndUser endUser = await _context.EndUser.Include(x => x.SocialId).Include(x => x.Organization).FirstOrDefaultAsync(EndUserMatches(email, name, phoneNumber));
            EndUserDto endUserDto = new EndUserDto
            {
                EndUserId = endUser.Id,
                Name = endUser.Name,
                ProfileImageUrl = endUser.ProfileImgUrl,
                OrganisationId = endUser.Organization.Id,
                OrganisationEmail = endUser.Organization.Email
            };

            return endUserDto;
        }


        public string TrimInput(string Input)
        {
            return (Input == null) ? string.Empty : Input.Trim('\"').Trim('\\');
        }

        private System.Linq.Expressions.Expression<Func<EndUser, bool>> EndUserMatches(string email, string name, string phoneNumber)
        {
            return element => element.Name == name
                                     || element.Email == email
                                     || element.PhoneNumber == phoneNumber;
        }

        public async Task ExtractData(Organisation Organisation)
        {
            string filePathCSV = @"./wwwroot/Upload/EndUser.csv";
            Task<string> fileData = ReadFileAsync(filePathCSV);
            await fileData;
            string[] contents = fileData.Result.Split('\n');
            long countOfSocialIds = System.Text.RegularExpressions.Regex.Matches(contents[0], "/Source").Count;
            string[] header = contents[0].Split(',');
            for (long i = 0; i < header.Length; i++)
            {
                header[i] = header[i].Replace("\r", string.Empty).Trim('\"');
            }
            long indexOfName = Array.IndexOf(header, "Name");
            long indexOfEmail = Array.IndexOf(header, "Email");
            long indexOfPhoneNumber = Array.IndexOf(header, "PhoneNumber");
            long indexOfProfileImage = Array.IndexOf(header, "ProfileImgUrl");
            long[] indexOfSocialAccountSource = new long[countOfSocialIds];
            long[] indexOfSocialAccountIdentifier = new long[countOfSocialIds];
            for (long i = 0; i < countOfSocialIds; i++)
            {
                indexOfSocialAccountSource[i] = Array.IndexOf(header, $"SocialId/{i}/Source");
                indexOfSocialAccountIdentifier[i] = Array.IndexOf(header, $"SocialId/{i}/Identifier");
            }

            for (long i = 1; i <= contents.Count() - 1; i++)
            {
                string[] info = contents[i].Split(',');

                EndUser endUser = new EndUser
                {
                    Name = info[indexOfName].Trim('\"'),
                    Email = info[indexOfEmail].Trim('\"'),
                    PhoneNumber = info[indexOfPhoneNumber].Trim('\"'),
                    ProfileImgUrl = info[indexOfProfileImage].Trim('\"'),
                    SocialId = new List<UserSocialId>(),
                    Organization = _context.Organisation.FirstOrDefault(x => x.OrganisationName == Organisation.OrganisationName) ?? Organisation,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now

                };

                for (long j = 0; j < countOfSocialIds; j++)
                {
                    if (info[indexOfSocialAccountSource[j]].Trim('\"') != string.Empty && info[indexOfSocialAccountIdentifier[j]].Trim('\"') != string.Empty)
                    {
                        endUser.SocialId.Add(new UserSocialId
                        {
                            Source = info[indexOfSocialAccountSource[j]].Trim('\"'),
                            Identifier = info[indexOfSocialAccountIdentifier[j]].Trim('\"'),
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now
                        });
                    }
                }

                _context.EndUser.Add(endUser);
                await _context.SaveChangesAsync();

            }
        }

        private static async Task<string> ReadFileAsync(string filepath)
        {
            string fileData = "";
            using (StreamReader streamReader = new StreamReader(filepath))
            {
                fileData = await streamReader.ReadToEndAsync();
            }
            return fileData;
        }
    }
}
