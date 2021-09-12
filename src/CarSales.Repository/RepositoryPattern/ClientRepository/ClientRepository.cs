using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Repository.RepositoryPattern.ClientRepository
{
    public class ClientRepository : IClientRepository 
    {
        private readonly AppDbContext _appDbContext;

        public ClientRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task DeleteClient(Client entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            //if user wants to delete client that has cars to sell, first we remove cars and then client
            if (await _appDbContext.Cars.Where(x => x.ClientId == entity.Id && x.DeletedAt == null).AnyAsync())
            {
                var cars = await _appDbContext.Cars.Where(x => x.ClientId == entity.Id && x.DeletedAt == null).ToListAsync();
                foreach (var car in cars)
                {
                    car.DeletedAt = DateTime.Now;
                }              
            }
            _appDbContext.Clients.Remove(entity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Client> GetClient(string IdentityNumber)
        {
            if (String.IsNullOrEmpty(IdentityNumber))
            {
                throw new ArgumentNullException("Identity Number Is Required!");
            }
            var client = await _appDbContext.Clients
                .Where(x => x.IdentityNumber == IdentityNumber && x.DeletedAt == null)
                .FirstOrDefaultAsync();
            if (client == null)
            {
                throw new DoesNotExistsException("Could not find client with this Identity Number!");
            }
            return client;
        }

        public async Task<Client> InsertClient(Client entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            
            var client = await _appDbContext.Clients
                .Where(x => x.IdentityNumber == entity.IdentityNumber).FirstOrDefaultAsync();

            //if such client exists and is active throw exception
            if (client != null && client.DeletedAt == null)
            {
                throw new AlreadyExistsException();

            }
            //if such client exists but isn't active, update it as active
            else if (client != null && client.DeletedAt != null)
            {
                entity.DeletedAt = null;
                _appDbContext.Update(entity);
                await _appDbContext.SaveChangesAsync();
                return entity;
            }
            await _appDbContext.Clients.AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateClient(Client entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            var client = await _appDbContext.Clients
                .Where(x => x.IdentityNumber == entity.IdentityNumber && x.DeletedAt == null).FirstOrDefaultAsync();
            if(client == null)
            {
                throw new DoesNotExistsException();
            }
            _appDbContext.Clients.Update(entity);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
