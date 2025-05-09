
using System;
using Microsoft.EntityFrameworkCore;
using ZdeskUserPortal.Domain.Model;

namespace ZdeskUserPortal.DataAccess.EntityFramwork.Context
{
    public class ZdeskDbContext: DbContext
    {
        public ZdeskDbContext(DbContextOptions<ZdeskDbContext> options)
        : base(options) { }

        public DbSet<BusinessUnitEntity> BusinessUnit { get; set; }
    }
}
