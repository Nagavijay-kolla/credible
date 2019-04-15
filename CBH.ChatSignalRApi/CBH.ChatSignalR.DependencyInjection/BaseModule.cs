using System;
using System.Collections.Generic;
using System.Linq;
using CBH.ChatSignalR.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CBH.ChatSignalR.DependencyInjection
{
    public abstract class BaseModule
    {
        public virtual void Add(IServiceCollection services)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class BaseModule<T> where T : Configuration
    {
        protected List<T> ConfigurationList { get; set; }
        protected T Configuration => ConfigurationList.Single();
        protected BaseModule(T configuration)
        {
            ConfigurationList = new List<T> { configuration };
        }
        protected BaseModule(List<T> configurations)
        {
            ConfigurationList = configurations;
        }
        protected T2 GetConfiguration<T2>() where T2 : Configuration
        {
            return ConfigurationList.OfType<T2>().Single();
        }
        public virtual void Add(IServiceCollection services)
        {
            throw new NotImplementedException();
        }
    }
}
