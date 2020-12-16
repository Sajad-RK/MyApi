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
            return Table.Where(a => a.Username == username && a.PasswordHash == hashed).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
