using FluentValidation;
using JewelleryStore.Application.Price.Validator;
using JewelleryStore.Application.User;
using JewelleryStore.Application.User.Validator;
using JewelleryStore.DataAccess;
using JewelleryStore.Model.Configuration;
using JewelleryStore.Model.Jewellery;
using JewelleryStore.Model.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JewelleryStore.Api
{
    internal static class DependencyConfiguration
    {
        internal static void AddDependency(IServiceCollection services, IConfiguration configuration)
        {
            InfrastructureDependecy(services, configuration);
            UserDepedencies(services);
            ValidatorDependencies(services);
        }

        private static void UserDepedencies(IServiceCollection services)
        {
            services.AddScoped<IUserDataAccess, UserDataAccess>();
            services.AddScoped<UserDetailsProvider>();
        }

        private static void ValidatorDependencies(IServiceCollection services)
        {
            services.AddScoped<AbstractValidator<UserAuthenticationMessage>, UserAuthenticationMessageValidator>();
            services.AddScoped<AbstractValidator<JewelleryMessage>, JewelleryMessageValidator>();
        }

        private static void InfrastructureDependecy(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DbSetting>(configuration.GetSection("DbConnection"));
        }
    }
}
