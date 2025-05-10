using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.Domain.Model.Login;

namespace ZdeskUserPortal.Domain.RepositoryInterfaces.Login
{
    public interface IRefereshToken
    {
        Task<RefereshTokenEntity> save(RefereshTokenEntity refereshTokenEntity);
        Task<RefereshTokenEntity> getByToken(string token);
        Task<RefereshTokenEntity> update(string token, RefereshTokenEntity refereshTokenEntity);
    }
}
