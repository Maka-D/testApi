using AutoMapper;
using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Repository;
using CarSales.Repository.MemoryCacheService;
using CarSales.Repository.RepositoryPattern;
using CarSales.Services.DTOs;
using CarSales.Services.ValidateInput;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.ClientServices
{
    public class ClientService : IClientService
    {
        private readonly IRepository<Client> _clientRepo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public ClientService(IMapper mapper, IRepository<Client> clientRepo, ICacheService cacheService)
        {
            _clientRepo = clientRepo;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task<Client> AddClient(ClientInput client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (await _clientRepo.Get(x => x.IdentityNumber == client.IdentityNumber && x.DeletedAt == null) != null)
                throw new ClientAlreadyExistsException();           

            if (await _clientRepo.Get(x => x.IdentityNumber == client.IdentityNumber && x.DeletedAt != null) != null)
                 return await UpdateClient(client);

            var insertedClient = _mapper.Map<Client>(client);
            return await _clientRepo.Insert(insertedClient);
        }

        public async Task<Client> UpdateClient(ClientInput client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var clientToUpdate = await _clientRepo.Get(x => x.IdentityNumber == client.IdentityNumber && x.DeletedAt == null);

            if (clientToUpdate == null)
                throw new ClientDoesNotExistsException();

            if (_cacheService.TryGet(clientToUpdate.IdentityNumber, out Client cachedClient))
                _cacheService.Remove(clientToUpdate.IdentityNumber);

            clientToUpdate.FirstName = client.FirstName;
            clientToUpdate.SecondName = client.SecondName;
            clientToUpdate.PhoneNumber = client.PhoneNumber;
            clientToUpdate.Address = client.Address;
            clientToUpdate.BirthDate = client.BirthDate;
            clientToUpdate.Email = client.Email;

            _cacheService.Set(clientToUpdate.IdentityNumber, clientToUpdate);

            return await _clientRepo.Update(clientToUpdate);

        }

        public async Task<Client> FindClient(string IdentityNum)
        {
            if(!_cacheService.TryGet(IdentityNum, out Client client))
            {
                if (!InputValidator.IsValidIdentityNumber(IdentityNum))
                {
                    throw new InvalidInputException();
                }
                client = await _clientRepo.Get(x => x.IdentityNumber == IdentityNum && x.DeletedAt == null);

                _cacheService.Set(IdentityNum, client);
            }
            return client;
            
        }

        public async Task DeleteClient(string IdenNum)
        {
            if (!InputValidator.IsValidIdentityNumber(IdenNum))
            {
                throw new InvalidInputException();
            }
            await _clientRepo.Delete(await _clientRepo.Get(x => x.IdentityNumber == IdenNum && x.DeletedAt == null));
            if (_cacheService.TryGet(IdenNum, out Client client))
                _cacheService.Remove(IdenNum);
        }

    }
}
