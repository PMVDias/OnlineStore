﻿using Moq;
using Ninject;
using OnlineStore.Domain.Abstract;
using OnlineStore.Domain.Concrete;
using OnlineStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel; 
        public NinjectDependencyResolver(IKernel kernelParam) 
        { 
            kernel = kernelParam; AddBindings(); 
        }        
        public object GetService(Type serviceType) 
        { 
            return kernel.TryGet(serviceType); 
        }        
        public IEnumerable<object> GetServices(Type serviceType) 
        { 
            return kernel.GetAll(serviceType); 
        }
        private void AddBindings()
        {            
           //put bindings here  
            kernel.Bind<IProductRepository>().To<EFProductRepository>();
            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);
            kernel.Bind<IUserRepository>().To<UserRepository>();
        }

    }
}