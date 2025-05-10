using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.DataAccess.EntityFramwork.Context;
using ZdeskUserPortal.DataAccess.Home;
using ZdeskUserPortal.Domain.Model.Login;
using ZdeskUserPortal.Domain.RepositoryInterfaces.Login;
using ZdeskUserPortal.Utility;

namespace ZdeskUserPortal.DataAccess.RepositoryServices.Home
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
            try
            {
                return await _dbSet.Where(x => x.Token == token).FirstAsync();
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Error in Data Access while getting {GetType().FullName}.", ex);
            }
        }

        public async Task<RefereshTokenEntity> save(RefereshTokenEntity refereshTokenEntity)
        {
            try
            {
                await _dbSet.AddAsync(refereshTokenEntity);
                _context.SaveChanges();
                return refereshTokenEntity;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Error in Data Access while getting {GetType().FullName}.", ex);
            }

        }

        public async Task<RefereshTokenEntity> update(string token, RefereshTokenEntity refereshTokenEntity)
        {
            try
            {
                var existingEntity = await _dbSet.FirstOrDefaultAsync(r => r.Token == token);

                if (existingEntity == null)
                {
                    return null;
                }

                existingEntity.ExpiryDate = DateTime.Now.AddDays(2);
                existingEntity.Token = refereshTokenEntity.Token;
                existingEntity.UserId = refereshTokenEntity.UserId;
                await _context.SaveChangesAsync();

                return existingEntity;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Error in Data Access while getting {GetType().FullName}.", ex);
            }


        }
    }
}
