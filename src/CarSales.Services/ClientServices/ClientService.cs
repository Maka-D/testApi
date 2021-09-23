using AutoMapper;
using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Repository;
using CarSales.Repository.CustomRepositories;
using CarSales.Repository.CacheService;
using CarSales.Repository.RepositoryPattern;
using CarSales.Services.DTOs;
using CarSales.Services.ValidateInput;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CarSales.Services.ClientServices
{
    public class ClientService : IClientService
    {
        private readonly ClientRepository _clientRepo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public ClientService(IMapper mapper, ClientRepository clientRepo, ICacheService cacheService)
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

            var cachedClient = JsonConvert.DeserializeObject<Client>(await _cacheService.Get(client.IdentityNumber));
            if (cachedClient != null)
                await _cacheService.Remove(clientToUpdate.IdentityNumber);

            clientToUpdate.FirstName = client.FirstName;
            clientToUpdate.SecondName = client.SecondName;
            clientToUpdate.PhoneNumber = client.PhoneNumber;
            clientToUpdate.Address = client.Address;
            clientToUpdate.BirthDate = client.BirthDate;
            clientToUpdate.Email = client.Email;

            await _cacheService.Set(clientToUpdate.IdentityNumber, clientToUpdate);

            return await _clientRepo.Update(clientToUpdate);

        }

        public async Task<Client> FindClient(string IdentityNum)
        {
            if (!InputValidator.IsValidIdentityNumber(IdentityNum))
            {
                throw new InvalidInputException();
            }

            var cachedClient = JsonConvert.DeserializeObject(await _cacheService.Get(IdentityNum));

            if(cachedClient == null)
            {
                cachedClient = await _clientRepo.Get(x => x.IdentityNumber == IdentityNum && x.DeletedAt == null);

                await _cacheService.Set(IdentityNum, cachedClient);
            }
            return (Client)cachedClient;
            
        }

        public async Task DeleteClient(string IdenNum)
        {
            if (!InputValidator.IsValidIdentityNumber(IdenNum))
            {
                throw new InvalidInputException();
            }
            await _clientRepo.Delete(await _clientRepo.Get(x => x.IdentityNumber == IdenNum && x.DeletedAt == null));

            var cachedClient = JsonConvert.DeserializeObject<Client>(await _cacheService.Get(IdenNum));

            if (cachedClient != null)
                await _cacheService.Remove(IdenNum);
        }

    }
}
