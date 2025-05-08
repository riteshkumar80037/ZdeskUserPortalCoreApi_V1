using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ZdeskUserPortal.Business.Interface;
using ZdeskUserPortal.Domain.Model;
using ZdeskUserPortal.Domain.RepositoryInterfaces;
using ZdeskUserPortal.DTOModel;

namespace ZdeskUserPortal.Business.Services
{
    public class LoginServices : ILogin
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IMapper _mapper;
        public LoginServices(ILoginRepository loginRepository, IMapper mapper)
        {
            _loginRepository = loginRepository;
            _mapper = mapper;
        }
        public async Task<LoginDTO> UserLogin(string username, string password)
        {
            var user= await _loginRepository.Login(username, password);
            LoginDTO loginDTO = _mapper.Map<LoginDTO>(user);
            return loginDTO;
        }
    }
}
