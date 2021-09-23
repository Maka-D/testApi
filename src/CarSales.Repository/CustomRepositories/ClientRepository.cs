using CarSales.Domain.Models;
using CarSales.Repository.RepositoryPattern;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Repository.CustomRepositories
{
    public class ClientRepository :Repository<Client>
    {
        private readonly AppDbContext _appDbContext;

        public ClientRepository(AppDbContext appDbContext) :base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public new async Task Delete(Client entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
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

        public async Task<Client> UpdateDeletedClient(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var deletedClient = await _appDbContext.Clients.Where(x => x.IdentityNumber == client.IdentityNumber).FirstOrDefaultAsync();

            deletedClient.DeletedAt = null;
            deletedClient.FirstName = client.FirstName;
            deletedClient.SecondName = client.SecondName;
            deletedClient.PhoneNumber = client.PhoneNumber;
            deletedClient.Address = client.Address;
            deletedClient.BirthDate = client.BirthDate;
            deletedClient.Email = client.Email;

            await _appDbContext.SaveChangesAsync();
            return deletedClient;
        }

    }
}
