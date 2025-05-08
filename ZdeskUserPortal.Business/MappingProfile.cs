using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ZdeskUserPortal.Domain.Model;
using ZdeskUserPortal.DTOModel;

namespace ZdeskUserPortal.Business
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UsersEntity, LoginDTO>();
            CreateMap<LoginDTO, UsersEntity>();
        }
    }
}
