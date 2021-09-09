using CarSales.Domain.Models;
using CarSales.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.ClientService
{
    public interface IClientService
    {
        Task<Client> AddClient(ClientInput client);
        Task UpdateClient(Client client);
        Task<Client> GetClient(string IdentityNum);


        //Task<Client> FindClient(string IdenNum);
    }
}
