using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ZdeskUserPortal.Business.Interface;
using ZdeskUserPortal.Domain.Model.Login;
using ZdeskUserPortal.Domain.RepositoryInterfaces.Login;
using ZdeskUserPortal.DTOModel;
using ZdeskUserPortal.Utility;

namespace ZdeskUserPortal.Business.Services
{
    public class LoginServices : ILogin
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IMapper _mapper;
        private readonly IRefereshToken _refereshToken;
        public LoginServices(ILoginRepository loginRepository, IMapper mapper, IRefereshToken refereshToken)
        {
            _loginRepository = loginRepository;
            _mapper = mapper;
            _refereshToken = refereshToken;
        }
        public async Task<Tuple<int,string>> UserLogin(string username, string password)
        {
            
            try
            {
                var user = await _loginRepository.Login(username, password);
                if (user == null) return Tuple.Create(0, "");
                RefereshTokenEntity refereshTokenEntity = new RefereshTokenEntity
                {
                    Active = true,
                    ExpiryDate = DateTime.Now.AddDays(2),
                    Token = await GenerateRefreshToken(),
                    UserId = user.Id,
                    EmailId = user.Email,

                };
                _refereshToken.save(refereshTokenEntity);
                return Tuple.Create(user.Id, refereshTokenEntity.Token);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error in business logic while getting user details.{GetType().FullName}.", ex);
            }
        }
        
       

        public async Task<RefereshTokenEntity> getByToken(string token)
        {
            try
            {
                return await _refereshToken.getByToken(token);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error in business logic while getting user details.{GetType().FullName}.", ex);
            }
            
        }

        public async Task<RefereshTokenEntity> update(string token, RefereshTokenEntity refereshTokenEntity)
        {
            try
            {
                return await _refereshToken.update(token, refereshTokenEntity);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error in business logic while getting user details.{GetType().FullName}.", ex);
            }
          

        }

        public async Task<string> GenerateRefreshToken()
        {
            try
            {
                var randomNumber = new byte[32];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error in business logic while getting user details.{GetType().FullName}.", ex);
            }
           
        }
    }
}
