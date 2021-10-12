using CarSales.Domain.CustomExceptions;
using CarSales.Repository.CustomRepositories;
using CarSales.Services.TokenService;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly ITokenService _token;
        private readonly UserRepository _userRepository;

        public UserService(ITokenService token, UserRepository userRepository)
        {
            _token = token;
            _userRepository = userRepository;
        }
        public async Task<IdentityUser> GetUserByRefreshToken(string refreshToken)
        {
            if (!await _token.ValidateRefreshToken(refreshToken))
                throw new InvalidInputException();

            var user = await _userRepository.GetUserByToken(refreshToken);

            if (user == null)
                throw new ArgumentNullException();

            return user;
        }
    }
}
