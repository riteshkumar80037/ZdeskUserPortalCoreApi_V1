
using System;
using Microsoft.EntityFrameworkCore;
using ZdeskUserPortal.Domain.Model.Login;
using ZdeskUserPortal.Domain.Model.Master;
using ZdeskUserPortal.Domain.RepositoryInterfaces;

namespace ZdeskUserPortal.DataAccess.EntityFramwork.Context
{
    public class ZdeskDbContext: DbContext
    {
        public ZdeskDbContext(DbContextOptions<ZdeskDbContext> options)
        : base(options) { }

        public DbSet<BusinessUnitEntity> BusinessUnit { get; set; }
        public DbSet<RefereshTokenEntity> RefreshTokens { get; set; }
    }
}
