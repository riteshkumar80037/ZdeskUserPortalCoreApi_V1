using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ZdeskUserPortal.DataAccess.EntityFramwork.Context;
using ZdeskUserPortal.Domain.Model;
using ZdeskUserPortal.Domain.RepositoryInterfaces;
using ZdeskUserPortal.Utility;

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
            catch (SqlException ex)
            {
                throw new DataAccessException($"Error in Data Access while getting All Record{GetType().FullName}..", ex);
            }

        }

        public async Task<T> GetById(Expression<Func<T, bool>>? predicate = null)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                return await query.FirstOrDefaultAsync();
            }
            catch (SqlException ex)
            {
                throw new DataAccessException("Error in Data Access while getting ById Record.", ex);
            }
        }
    }
}
