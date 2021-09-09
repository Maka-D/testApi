using AutoMapper;
using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Repository;
using CarSales.Repository.RepositoryPattern;
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
        private readonly IRepository<Client> _repository;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public ClientService(IRepository<Client> repository, IMapper mapper, AppDbContext appDbContext)
        {
            _repository = repository;
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<Client> AddClient(ClientInput client)
        {
            if(client == null)
            {
                throw new ArgumentNullException("client");
            }
            else if(await ClientExists(client.IdentityNumber))
            {
                throw new ClientAlreadyExistsException();
            }
            return await _repository.Insert(_mapper.Map<Client>(client));
        }

        public async Task UpdateClient(ClientInput client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }
            else if (! await ClientExists(client.IdentityNumber))
            {
                throw new ClientDoesNotExistsException();
            }
            await _repository.Update(_mapper.Map<Client>(client));
        }

        public async Task<Client> GetClient(string IdentityNum)
        {
            if (String.IsNullOrEmpty(IdentityNum))
            {
                throw new ArgumentException("Identity Number Is Required!");
            }
            var client = await _appDbContext.Clients.Where(x => x.IdentityNumber == IdentityNum && x.DeletedAt == null).FirstOrDefaultAsync();
            if(client == null)
            {
                throw new ClientDoesNotExistsException();
            }
            return client;
        }

        public async Task DeleteClient(string IdenNum)
        {
            var client = await GetClient(IdenNum);
             _repository.Delete(client);
        }


        #region private methods
        private async Task<bool> ClientExists(string IdentityNumber)
        {
            return await  _appDbContext.Clients.Where(x => x.IdentityNumber == IdentityNumber && x.DeletedAt == null).AnyAsync();
        }
        #endregion
    }
}
