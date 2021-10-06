using CarSales.Domain.Models;
using CarSales.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.ClientServices
{
    public interface IUserService
    {
        Task<Client> AddClient(UserInput client);
        Task<Client> UpdateClient(UserInput client);
        Task<Client> FindClient(string IdentityNum);
        Task DeleteClient(string IdenNum);

    }
}
