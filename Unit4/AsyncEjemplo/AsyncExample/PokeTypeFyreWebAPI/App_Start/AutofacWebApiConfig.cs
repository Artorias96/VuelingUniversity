﻿using Autofac.Integration.WebApi;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Business.ServiceImplementations;
using Infrastructure.RepositoryImplementations;
using Business.ServiceContracts;
using Domain.RepositoryContracts;
using System.IO;
using Serilog;

namespace PokeTypeFyreWebAPI.App_Start
{
    public class AutofacWebApiConfig
    {
        public static IContainer Container;
        private static string _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AplicationLog", "Log.txt");

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }


        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            //Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            Log.Logger = new LoggerConfiguration().WriteTo.File(_logFilePath).CreateLogger();
            builder.RegisterInstance(Log.Logger).As<ILogger>().SingleInstance();

            builder.RegisterType<PokeService>().As<IPokeService>();
            builder.RegisterType<PokeTypeFyreRepository>().As<IPokeTypeFyreRepository>();
            builder.RegisterType<PokeMoveRepository>().As<IPokeMoveRepository>();
            builder.RegisterType<PokeTypesRepository>().As<IPokeTypesRepository>();



            //Set the dependency resolver to be Autofac.
            Container = builder.Build();

            return Container;
        }
    }
}