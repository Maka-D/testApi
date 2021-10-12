using AutoMapper;
using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Repository;
using CarSales.Repository.CustomRepositories;
using CarSales.Services.CacheService;
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
using Microsoft.Extensions.Caching.Distributed;

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
        public async Task<Client> Register(RegistrationInput client, string userId)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (await _clientRepo.Get(x => x.IdentityNumber == client.IdentityNumber && x.DeletedAt == null) != null)
                throw new ClientAlreadyExistsException();

            var insertedClient = _mapper.Map<Client>(client);
            insertedClient.UserId = userId;

            if (await _clientRepo.Get(x => x.IdentityNumber == client.IdentityNumber && x.DeletedAt != null) != null)
                return await _clientRepo.UpdateDeletedClient(insertedClient);

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

            //checks if client exists in cache and removes
            var cachedClient = await _cacheService.Get<Client>(client.IdentityNumber);
            if (cachedClient != null)
                await _cacheService.Remove(clientToUpdate.IdentityNumber);

            clientToUpdate.FirstName = client.FirstName;
            clientToUpdate.SecondName = client.SecondName;
            clientToUpdate.PhoneNumber = client.PhoneNumber;
            clientToUpdate.Address = client.Address;
            clientToUpdate.BirthDate = client.BirthDate;
            clientToUpdate.Email = client.Email;

            await _cacheService.Set(clientToUpdate.IdentityNumber, clientToUpdate, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(60),
                SlidingExpiration = TimeSpan.FromMinutes(30)
            });

            return await _clientRepo.Update(clientToUpdate);

        }

        public async Task<Client> FindClient(string IdentityNum)
        {
            if (!InputValidator.IsValidIdentityNumber(IdentityNum))
            {
                throw new InvalidInputException();
            }

            var cachedClient = await _cacheService.Get<Client>(IdentityNum);

            if (cachedClient == null)
            {
                cachedClient = await _clientRepo.Get(x => x.IdentityNumber == IdentityNum && x.DeletedAt == null);

                if (cachedClient == null)
                    throw new ClientDoesNotExistsException();

                await _cacheService.Set(IdentityNum, cachedClient, new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(60),
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }
            return cachedClient;

        }

        public async Task DeleteClient(string IdenNum)
        {
            if (!InputValidator.IsValidIdentityNumber(IdenNum))
            {
                throw new InvalidInputException();
            }

            //checks if client exist in cache and removes
            if (await _cacheService.Get<Client>(IdenNum) != null)
                await _cacheService.Remove(IdenNum);

            await _clientRepo.Delete(await _clientRepo.Get(x => x.IdentityNumber == IdenNum && x.DeletedAt == null));
        }


    }
}
