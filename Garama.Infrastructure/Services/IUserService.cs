using Garama.Domain.Common.Auth;
using Garama.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Garama.Infrastructure.Services
{
    public interface IUserService
    {
        User GetUserDetails(LoginModel loginModel);
    }
}
