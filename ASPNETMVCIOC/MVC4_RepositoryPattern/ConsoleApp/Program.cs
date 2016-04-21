using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Demo.Data.Models;
using Demo.Data.Repositories;
using Demo.DTO;
using Demo.Infrastructure;
using Demo.Service;
using Demo.ServiceInterface;
using Microsoft.Practices.Unity;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
            Demo.Service.MapCreator.CreateMap();

            var container = ServiceLocator.Instance.GetConfiguredContainer();
            container.RegisterTypes(AllClasses.FromLoadedAssemblies(), WithMappings.FromMatchingInterface, WithName.Default, overwriteExistingMappings: true);

            //sample1
            //using (DemoContext context = new DemoContext())
            //{
            //    IUserInfoService userinfoService = new UserInfoService(context);
            //    UserInfoDTO dto = new UserInfoDTO();
            //    dto.Name = "test8";
            //    dto.Remark = "测试用户";
            //    dto.Title = "非系统用户";
            //    userinfoService.Add(dto);
            //}

            //sample2
            //DemoContext context2 = new DemoContext();
            //IUserInfoService userinfoService2 = ServiceLocator.Instance.GetService<IUserInfoService>(new { context = context2 });
            //UserInfoDTO dto2 = new UserInfoDTO();
            //dto2.Name = "test9";
            //dto2.Remark = "测试用户";
            //dto2.Title = "非系统用户";
            //userinfoService2.Add(dto2);
            //context2.Dispose();

            //sample1
            using (IUserInfoService us3 = ServiceLocator.Instance.GetService<IUserInfoService>())
            {
                UserInfoDTO dto = new UserInfoDTO();
                dto.Name = "test10";
                dto.Remark = "测试用户";
                dto.Title = "非系统用户";
                us3.Add(dto);
            }

            Console.WriteLine("over.");
            Console.ReadKey();

            //var container = ServiceLocator.Instance.GetConfiguredContainer();
            container.Dispose();
        }
    }
}
