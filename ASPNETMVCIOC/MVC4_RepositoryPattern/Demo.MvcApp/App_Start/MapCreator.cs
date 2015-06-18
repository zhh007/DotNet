using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Demo.DTO;
using Demo.MvcApp.Models;

namespace Demo.MvcApp.App_Start
{
    public class MapCreator
    {
        public static void CreateMap()
        {
            Mapper.CreateMap<UserInfoViewModel, UserInfoDTO>();
            Mapper.CreateMap<UserInfoDTO, UserInfoViewModel>();
        }
    }
}