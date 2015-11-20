using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Demo.Data.Models;
using Demo.Data.Repositories;
using Demo.DTO;
using Demo.Service;
using Demo.ServiceInterface;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
            Demo.Service.MapCreator.CreateMap();

            using (DemoContext context = new DemoContext())
            {
                IUserInfoService userinfoService = new UserInfoService(context, new UserInfoRepository(context));
                UserInfoDTO dto = new UserInfoDTO();
                dto.Name = "test8";
                dto.Remark = "测试用户";
                dto.Title = "非系统用户";
                userinfoService.Add(dto);
            }

            Console.WriteLine("over.");
            Console.ReadKey();
        }
    }
}
