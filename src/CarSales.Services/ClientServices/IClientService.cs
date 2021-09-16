using CarSales.Domain.Models;
using CarSales.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.ClientServices
{
    public interface IClientService
    {
        Task<Client> AddClient(ClientInput client);
        Task UpdateClient(ClientInput client);
        Task<Client> FindClient(string IdentityNum);
        Task DeleteClient(string IdenNum);

    }
}
