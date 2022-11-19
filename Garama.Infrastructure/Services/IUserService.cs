using Garama.Domain.Common.Auth;
using Garama.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Garama.Infrastructure.Services
{
    public interface IUserService
    {
        User GetUserDetails(LoginModel loginModel);
        public bool UpdateRefreshToken(string RefreshToken, string UserId);
        public User GetUserById(string Id);
        Task<string> AddThirdPartyUserToDbAndGetUserId(ThirdPartyAuthUserDetails details);
    }
}
