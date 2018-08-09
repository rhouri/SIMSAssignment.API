using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SIMSAssignment.API.Domain;
using SIMSAssignment.API.Models;

namespace SIMSAssignment.API.Services
    {
    public class UsersService
        {
        private readonly IConfiguration configuration;
        private readonly SIMSDbContext dbContext;

        public UsersService ( IConfiguration configuration, SIMSDbContext db)
            {
            this.configuration = configuration;
            this.dbContext = db;
            }
        
        public async Task<ServiceResult<AuthenticatedModel>> Login ( LoginModel loginModel )
            {
            try
                {
                return await HandleTokenLogin(loginModel);
                }
            catch
                {
                }

            return new ServiceResult<AuthenticatedModel>("Failed to generate token.");
            }

        private async Task<ServiceResult<AuthenticatedModel>> HandleTokenLogin ( LoginModel loginModel )
            {
            var user = await dbContext.Users.FindAsync(loginModel.UserID);
            if (user == null || user.Password.Trim()!=loginModel.Password.Trim())
                return new ServiceResult<AuthenticatedModel>("Username or password invalid.");
            return GenerateAuthenticationResultForUser(user);
            }

        private ServiceResult<AuthenticatedModel> GenerateAuthenticationResultForUser ( SIMSUser user )
            {
            var claims = new List<Claim>();
            if (user.Claims != null)
                {
                string[] userClaims = user.Claims.Split(new char[] { ',' });
                foreach (string c in userClaims)
                    claims.Add(new Claim(c, "true")); // convert to Identity Claim
                }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: configuration["Jwt:Issuer"],
                                             audience: configuration["Jwt:Issuer"],
                                             claims: claims,
                                             expires: DateTime.UtcNow.AddHours(4),
                                             signingCredentials: credentials);
            
            var result = new ServiceResult<AuthenticatedModel>(new AuthenticatedModel
                {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
                });
            return result;
            }

        }

    public class LoginModel
        {
        public string UserID
            {
            get; set;
            }
        public string Password
            {
            get; set;
            }
        }

    }
