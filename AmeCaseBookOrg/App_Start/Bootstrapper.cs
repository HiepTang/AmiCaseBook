﻿using AmeCaseBookOrg.DAL.Infrastructure;
using AmeCaseBookOrg.Service;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using System.Web.Mvc;

namespace AmeCaseBookOrg.App_Start
{
    public class Bootstrapper
    {
        public static void run()
        {
            SetAutofacContainer();
            AutoMapperConfiguration.Configure();
        }
        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            builder.RegisterType<FileService>().As<IFileService>().InstancePerRequest();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerRequest();
            builder.RegisterType<AnnouncementService>().As<IAnnouncementService>().InstancePerRequest();
            builder.RegisterType<MemberService>().As<IMemberService>().InstancePerRequest();
            builder.RegisterType<DataItemService>().As<IDataItemService>().InstancePerRequest();
            builder.RegisterType<CommunityService>().As<ICommunityService>().InstancePerRequest();

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}