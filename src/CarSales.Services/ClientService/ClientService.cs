using AutoMapper;
using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Repository;
using CarSales.Repository.RepositoryPattern;
using CarSales.Repository.RepositoryPattern.ClientRepository;
using CarSales.Services.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.ClientService
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepo;
        private readonly IMapper _mapper;

        public ClientService(IMapper mapper, IClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
            _mapper = mapper;
        }
        public async Task<Client> AddClient(ClientInput client)
        {
            if(client == null)
            {
                throw new ArgumentNullException("client");
            }
            return await _clientRepo.InsertClient(_mapper.Map<Client>(client));
        }

        public async Task UpdateClient(ClientInput client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }
            else if (await _clientRepo.GetClient(client.IdentityNumber) == null)
            {
                throw new DoesNotExistsException();
            }
            await _clientRepo.UpdateClient(_mapper.Map<Client>(client));
        }

        public async Task<Client> FindClient(string IdentityNum)
        { 
            return await _clientRepo.GetClient(IdentityNum);
        }

        public async Task DeleteClient(string IdenNum)
        {
             await _clientRepo.DeleteClient(await _clientRepo.GetClient(IdenNum));
        }

    }
}
