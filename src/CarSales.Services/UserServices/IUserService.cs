using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.UserServices
{
    public interface IUserService
    {
        Task<IdentityUser> GetUserByRefreshToken(string refreshToken);
    }
}
