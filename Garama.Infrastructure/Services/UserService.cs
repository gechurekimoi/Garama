using Garama.Domain.Common.Auth;
using Garama.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garama.Infrastructure.Services
{
    public class UserService : IUserService
    {
        ILogger logger;
        GaramaDbContext dbContext;

        public UserService(ILogger logger, GaramaDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public User GetUserDetails(LoginModel loginModel)
        {
            try
            {
                string Password = loginModel.Password.Trim().ToLower();
                string Username = loginModel.UserName.Trim().ToLower();

                var user = dbContext.Users.Where(p=>p.UserName == Username && p.PasswordHash == Password).FirstOrDefault();

                return user;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting user details");
                return null;
            }
        }

        public bool UpdateRefreshToken(string RefreshToken, string UserId)
        {
            try
            {

                User user = dbContext.Users.Where(p => p.Id == UserId).FirstOrDefault();
                user.RefreshToken = RefreshToken;

                dbContext.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error Updating Token");
                return false;
            }
        }

        public User GetUserById(string Id)
        {
            try
            {
                var user = dbContext.Users.Where(p => p.Id == Id).FirstOrDefault();

                return user;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting user by Id");
                return null;
            }
        }
    }
}
