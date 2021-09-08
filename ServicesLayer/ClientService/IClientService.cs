using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.ClientService
{
    public interface IClientService
    {
        void AddClient(Client client);
        //Task<Client> FindClient(string IdenNum);
    }
}
