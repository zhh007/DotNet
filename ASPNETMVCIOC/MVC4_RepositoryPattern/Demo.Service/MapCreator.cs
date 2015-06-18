using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Demo.Data.Models;
using Demo.DTO;

namespace Demo.Service
{
    public class MapCreator
    {
        public static void CreateMap()
        {
            Mapper.CreateMap<UserInfoDTO, UserInfo>();
            Mapper.CreateMap<UserInfo, UserInfoDTO>();
        }
    }
}
