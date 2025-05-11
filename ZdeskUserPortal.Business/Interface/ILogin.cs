using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.Domain.Model.Login;
using ZdeskUserPortal.DTOModel;

namespace ZdeskUserPortal.Business.Interface
{
    public interface ILogin
    {
        Task<Tuple<int,string>> UserLogin(string username, string password);
        Task<string> GenerateRefreshToken();
        Task<RefereshTokenEntity> getByToken(string token);
        Task<RefereshTokenEntity> update(string token, RefereshTokenEntity refereshTokenEntity);
        Task<bool> SendOTP(string email);
    }
}
