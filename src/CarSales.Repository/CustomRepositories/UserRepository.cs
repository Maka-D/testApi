using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Repository.CustomRepositories
{
    public class UserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<IdentityUser> GetUserByToken(string refreshToken)
        {
            
           var entity = await  _appDbContext.UserTokens.Where(x => x.Value == refreshToken).FirstOrDefaultAsync();

           var user = await _appDbContext.Users.FindAsync(entity.UserId);

            return user;
        }
    }
}
