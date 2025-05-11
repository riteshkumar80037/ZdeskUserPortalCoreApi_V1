using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.Domain.Model.Master;

namespace ZdeskUserPortal.Business.Interface
{
    public interface IUser
    {
        Task<UsersEntity> GetUsers(int id);
        Task<UsersEntity> GetUsers(string email);
    }
}
