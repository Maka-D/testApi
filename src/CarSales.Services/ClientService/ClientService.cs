using AutoMapper;
using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Repository;
using CarSales.Repository.RepositoryPattern;
using CarSales.Repository.RepositoryPattern.ClientRepository;
using CarSales.Services.DTOs;
using CarSales.Services.ValidateInput;
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
        private readonly IInputValidator _validate;

        public ClientService(IMapper mapper, IClientRepository clientRepo, IInputValidator validate)
        {
            _clientRepo = clientRepo;
            _mapper = mapper;
            _validate = validate;
        }
        public async Task<Client> AddClient(ClientInput client)
        {
            if (client == null)
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
                throw new ClientDoesNotExistsException();
            }
            await _clientRepo.UpdateClient(_mapper.Map<Client>(client));
        }

        public async Task<Client> FindClient(string IdentityNum)
        {
            if (!_validate.IsValidIdentityNumber(IdentityNum))
            {
                throw new InvalidInputException();
            }
            return await _clientRepo.GetClient(IdentityNum);
        }

        public async Task DeleteClient(string IdenNum)
        {
            if (!_validate.IsValidIdentityNumber(IdenNum))
            {
                throw new InvalidInputException();
            }
            await _clientRepo.DeleteClient(await _clientRepo.GetClient(IdenNum));
        }

    }
}
