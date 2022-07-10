using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Infrastructure.Contexts;
using WebApplication.Infrastructure.Entities;
using WebApplication.Infrastructure.Interfaces;

namespace WebApplication.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly InMemoryContext _dbContext;

        public UserService(InMemoryContext dbContext)
        {
            _dbContext = dbContext;

            // this is a hack to seed data into the in memory database. Do not use this in production.
            _dbContext.Database.EnsureCreated();
        }

        /// <inheritdoc />
        public async Task<User?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            User? user = await _dbContext.Users.Where(user => user.Id == id)
                                         .Include(x => x.ContactDetail)
                                         .FirstOrDefaultAsync(cancellationToken);

            return user;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> FindAsync(string? givenNames, string? lastName, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.Users.Where(user => user.GivenNames == givenNames || user.LastName == lastName).ToListAsync<User>();

            return result;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> GetPaginatedAsync(int page, int count, CancellationToken cancellationToken = default)
        {
            var total = await _dbContext.Users.CountAsync();
            var items = await _dbContext.Users.Skip((page - 1) * count).Take(count).ToListAsync();
            return items;
        }

        /// <inheritdoc />
        public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            if (_dbContext.Users.Any(u => u.GivenNames == user.GivenNames && u.LastName == user.LastName))
            {
                throw new Exception("User already exists");
            }
            else
            {
                var result = await Task.Run(() =>  _dbContext.Users.Add(user));
                return result.Entity;
            }
        }

        /// <inheritdoc />
        public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            User? userFromDbContext = await _dbContext.Users.Where(user => user.Id == user.Id)
                                                .Include(x => x.ContactDetail)
                                                .FirstOrDefaultAsync(cancellationToken);

            userFromDbContext.GivenNames = user.GivenNames;
            userFromDbContext.LastName = user.LastName;
            userFromDbContext.ContactDetail = user.ContactDetail;
 
            var result = await Task.Run(() => _dbContext.Update(userFromDbContext));
            return result.Entity;
        }

        /// <inheritdoc />
        public async Task<User?> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            User? userFromDbContext = await _dbContext.Users.Where(user => user.Id == user.Id)
                                                   .Include(x => x.ContactDetail)
                                                   .FirstOrDefaultAsync(cancellationToken);

            var result = await Task.Run(() => _dbContext.Users.Remove(userFromDbContext));
            return result.Entity;
        }

        /// <inheritdoc />
        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.Users.CountAsync();
            return result;
        }
    }
}
