using Common.Exceptions;
using Common.Utilities;
using Data.Cotracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken)
        {
            var hashed = Common.Utilities.SecurityHelper.GetSha256Hash(password);
            return Table.Where(a => a.UserName == username && a.PasswordHash == hashed).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddAsync(User user, string password, CancellationToken cancellationToken)
        {
            var exist = await TableNoTracking.AnyAsync(a => a.UserName == user.UserName);
            if (exist)
                throw new BadRequestException("نام کاربری تکراری است");
            var passwordHash = SecurityHelper.GetSha256Hash(password);
            user.PasswordHash = passwordHash;
            await base.AddAsync(user, cancellationToken); 
        }
    }
}
