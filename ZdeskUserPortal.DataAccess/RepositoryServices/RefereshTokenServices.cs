using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.DataAccess.EntityFramwork.Context;
using ZdeskUserPortal.Domain.Model;
using ZdeskUserPortal.Domain.RepositoryInterfaces;

namespace ZdeskUserPortal.DataAccess.RepositoryServices
{
    public class RefereshTokenServices : IRefereshToken
    {
        private readonly ZdeskDbContext _context;
        private readonly DbSet<RefereshTokenEntity> _dbSet;

        public RefereshTokenServices(ZdeskDbContext context)
        {
            _context = context;
            _dbSet = context.Set<RefereshTokenEntity>();
        }

        public async Task<RefereshTokenEntity> getByToken(string token)
        {
            return await _dbSet.Where(x => x.Token == token).FirstAsync();
        }

        public async Task<RefereshTokenEntity> save(RefereshTokenEntity refereshTokenEntity)
        {
            await _dbSet.AddAsync(refereshTokenEntity);
             _context.SaveChanges();
            return refereshTokenEntity;
        }

        public async Task<RefereshTokenEntity> update(string token, RefereshTokenEntity refereshTokenEntity)
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(r => r.Token == token);

            if (existingEntity == null)
            {
                return null;
            }

            // Update the properties you want
            existingEntity.ExpiryDate = DateTime.Now.AddDays(2);
            existingEntity.Token = refereshTokenEntity.Token;
            existingEntity.UserId = refereshTokenEntity.UserId;
            // Add other properties as needed

            // Save changes
            await _context.SaveChangesAsync();

            return existingEntity;

        }
    }
}
