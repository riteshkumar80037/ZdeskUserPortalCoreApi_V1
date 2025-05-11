using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdeskUserPortal.Business.Interface;
using ZdeskUserPortal.Domain.Model.Master;
using ZdeskUserPortal.Domain.RepositoryInterfaces;
using ZdeskUserPortal.Utility;

namespace ZdeskUserPortal.Business.Services
{
    public class UserServices : IUser
    {
        IBaseRepository<UsersEntity> _usersRepository;

        public UserServices(IBaseRepository<UsersEntity> usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public async Task<UsersEntity> GetUsers(int id)
        {
            try
            {
                return await _usersRepository.GetById(x=>x.Id==id && x.Active==true);
                
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error in business logic while getting user details.{GetType().FullName}.", ex);
            }
        }

        public async Task<UsersEntity> GetUsers(string email)
        {
            try
            {
                return await _usersRepository.GetById(x => x.Email == email && x.Active == true);

            }
            catch (DataAccessException ex)
            {
                throw new BusinessException($"Error in business logic while getting user details.{GetType().FullName}.", ex);
            }
        }
    }
}
