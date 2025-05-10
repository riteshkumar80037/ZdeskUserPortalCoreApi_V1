using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ZdeskUserPortal.Domain.Model;
using ZdeskUserPortal.Domain.Model.Login;
using ZdeskUserPortal.DTOModel;

namespace ZdeskUserPortal.Business
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UsersEntity, TokenDTO>();
            CreateMap<LoginDTO, UsersEntity>();
            CreateMap<LogoDTO, OrganizationEntity>();
            CreateMap<OrganizationEntity, LogoDTO>();
        }
    }
}
