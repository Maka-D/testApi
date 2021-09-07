using DomainLayer.Models;
using RepositoryLayer.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.ClientService
{
    class ClientService : IClientService
    {
        private IRepository<Client> _repository;

        public ClientService(IRepository<Client> repository)
        {
            _repository = repository;
        }
        public async void AddClient(Client client)
        {
            if (await _repository.Exists(client.Id))
            {
               
            }
            _repository.Insert(client);
        }

       
    }
}
