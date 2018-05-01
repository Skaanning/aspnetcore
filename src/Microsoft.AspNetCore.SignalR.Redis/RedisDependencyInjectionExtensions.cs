// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Redis;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for configuring Redis-based scale-out for a SignalR Server in an <see cref="ISignalRServerBuilder" />.
    /// </summary>
    public static class RedisDependencyInjectionExtensions
    {
        /// <summary>
        /// Adds scale-out to a <see cref="ISignalRServerBuilder"/>, using a shared Redis server.
        /// </summary>
        /// <param name="builder">The <see cref="ISignalRServerBuilder"/>.</param>
        /// <returns>The same instance of the <see cref="ISignalRServerBuilder"/> for chaining.</returns>
        public static ISignalRServerBuilder AddRedis(this ISignalRServerBuilder builder)
        {
            return AddRedis(builder, o => { });
        }

        /// <summary>
        /// Adds scale-out to a <see cref="ISignalRServerBuilder"/>, using a shared Redis server.
        /// </summary>
        /// <param name="builder">The <see cref="ISignalRServerBuilder"/>.</param>
        /// <param name="redisConnectionString">The connection string used to connect to the Redis server.</param>
        /// <returns>The same instance of the <see cref="ISignalRServerBuilder"/> for chaining.</returns>
        public static ISignalRServerBuilder AddRedis(this ISignalRServerBuilder builder, string redisConnectionString)
        {
            return AddRedis(builder, o =>
            {
                o.Configuration = ConfigurationOptions.Parse(redisConnectionString);
            });
        }

        /// <summary>
        /// Adds scale-out to a <see cref="ISignalRServerBuilder"/>, using a shared Redis server.
        /// </summary>
        /// <param name="builder">The <see cref="ISignalRServerBuilder"/>.</param>
        /// <param name="configure">A callback to configure the Redis options.</param>
        /// <returns>The same instance of the <see cref="ISignalRServerBuilder"/> for chaining.</returns>
        public static ISignalRServerBuilder AddRedis(this ISignalRServerBuilder builder, Action<RedisOptions> configure)
        {
            builder.Services.Configure(configure);
            builder.Services.AddSingleton(typeof(HubLifetimeManager<>), typeof(RedisHubLifetimeManager<>));
            return builder;
        }

        /// <summary>
        /// Adds scale-out to a <see cref="ISignalRServerBuilder"/>, using a shared Redis server.
        /// </summary>
        /// <param name="builder">The <see cref="ISignalRServerBuilder"/>.</param>
        /// <param name="redisConnectionString">The connection string used to connect to the Redis server.</param>
        /// <param name="configure">A callback to configure the Redis options.</param>
        /// <returns>The same instance of the <see cref="ISignalRServerBuilder"/> for chaining.</returns>
        public static ISignalRServerBuilder AddRedis(this ISignalRServerBuilder builder, string redisConnectionString, Action<RedisOptions> configure)
        {
            return AddRedis(builder, o =>
            {
                o.Configuration = ConfigurationOptions.Parse(redisConnectionString);
                configure(o);
            });
        }
    }
}
