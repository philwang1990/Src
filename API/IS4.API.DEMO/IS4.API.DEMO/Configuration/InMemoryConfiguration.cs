using System;
using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IS4.API.DEMO.Configuration {
    public class InMemoryConfiguration {

        // 1. 哪些API可以使用这个authorization server
        public static IEnumerable<ApiResource> ApiResources() {
            return new[]
            {
                new ApiResource("socialnetwork", "社交网络")
            };
        }

        //2. 那些客户端Client(应用)可以使用这个authorization server.
        public static IEnumerable<Client> Clients() {
            return new[]
            {
                new Client
                {
                    ClientId = "socialnetwork",


                    //AccessTokenLifetime = 5,  //AccessToken过期时间
                   
                    //Client用来获取token
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },

                    //通过用户名密码和ClientCredentials来换取token
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,

                    //这里只用socialnetwork
                    AllowedScopes = new [] { "socialnetwork" }
                }
            };
        }

        //3. 指定可以使用authorization server授权的用户
        public static IEnumerable<TestUser> Users() {
            return new[]
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "mail@qq.com",
                    Password = "password"
                }
            };
        }
    }
}
