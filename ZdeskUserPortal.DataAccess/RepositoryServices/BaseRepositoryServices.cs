using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZdeskUserPortal.DataAccess.EntityFramwork.Context;
using ZdeskUserPortal.Domain.RepositoryInterfaces;

namespace ZdeskUserPortal.DataAccess.RepositoryServices
{
    public class BaseRepositoryServices<T> : IBaseRepository<T> where T : class
    {
        private readonly ZdeskDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepositoryServices(ZdeskDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? predicate = null)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<T> GetById(BigInteger Id)
        {
           return await _dbSet.Where(x => x.Equals(Id)).FirstOrDefaultAsync();
        }
    }
}
