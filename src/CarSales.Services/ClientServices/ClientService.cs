using AutoMapper;
using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Repository;
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

        public ClientService(IMapper mapper, IRepository<Client> clientRepo)
        {
            _clientRepo = clientRepo;
            _mapper = mapper;
        }
        public async Task<Client> AddClient(ClientInput client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }
            return await _clientRepo.Insert(_mapper.Map<Client>(client));
        }

        public async Task UpdateClient(ClientInput client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }
            
            await _clientRepo.Update(_mapper.Map<Client>(client));
        }

        public async Task<Client> FindClient(string IdentityNum)
        {
            if (!InputValidator.IsValidIdentityNumber(IdentityNum))
            {
                throw new InvalidInputException();
            }
            return await _clientRepo.Get(x => x.IdentityNumber == IdentityNum && x.DeletedAt == null);
            
        }

        public async Task DeleteClient(string IdenNum)
        {
            if (!InputValidator.IsValidIdentityNumber(IdenNum))
            {
                throw new InvalidInputException();
            }
            await _clientRepo.Delete(await _clientRepo.Get(x => x.IdentityNumber == IdenNum && x.DeletedAt == null));
        }

    }
}
