using System;
using Microsoft.Extensions.DependencyInjection;
using KKday.API.IS4.Server.Models.Repository;
using IS4.API.DEMO.Models.Repository;

namespace KKday.API.IS4.Server {
    public static class CustomIdentityServerBuilderExtensions {
        public static IIdentityServerBuilder AddCustomUserStore
        (this IIdentityServerBuilder builder) {
            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.AddProfileService<CustomProfileService>();
            builder.AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>();

            return builder;
        }
    }
}
