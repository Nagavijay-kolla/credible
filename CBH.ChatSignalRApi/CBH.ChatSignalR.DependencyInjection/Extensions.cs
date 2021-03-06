﻿using CBH.ChatSignalR.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CBH.ChatSignalR.DependencyInjection
{
    public static class Extensions
    {
        public static void AddModule(this IServiceCollection services, BaseModule module)
        {
            module.Add(services);
        }

        public static void AddModule<T>(this IServiceCollection services, BaseModule<T> module) where T : Configuration
        {
            module.Add(services);
        }
    }
}
