using CarSales.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Repository.RepositoryPattern.ClientRepository
{
    public interface IClientRepository
    {
        Task<Client> GetClient(string IdentityNumber);
        Task<Client> InsertClient(Client client);
        Task<bool> UpdateClient(Client entity);
        Task DeleteClient(Client entity);
    }
}
