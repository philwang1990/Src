using System;
using System.Collections.Generic;
using System.Data;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;


namespace IS4.API.DEMO.Configuration {
    public class InMemoryConfiguration {


        public static IEnumerable<IdentityResource> GetIdentityResources() {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }

        // 1. 哪些API可以使用这个authorization server
        public static IEnumerable<ApiResource> ApiResources() {
            return new[]
            {
                new ApiResource("KKDAY_B2D", "分銷商平台"){
                 
                     UserClaims = new [] { "email" }
                     
                     
                },

                new ApiResource("api", "Demo API") {
                ApiSecrets = { new Secret("secret".Sha256()) }
                }
            };
        }

        //2. 那些客户端Client(应用)可以使用这个authorization server.
        public static IEnumerable<Client> Clients() {
            return new[]
            {
                new Client
                {
                    ClientId = "KKDAY_B2D",

                    AccessTokenLifetime = 2592000,  //AccessToken过期时间：一個月(秒為單位)
                   
                    //Client用来获取token
                    ClientSecrets = new [] { new Secret("secret".Sha256()) },

                    //通过用户名密码和ClientCredentials来换取token
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,

                    //这里只用socialnetwork
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "KKDAY_B2D"
                    }

                   // RefreshTokenUsage = TokenUsage.ReUse,

                   // RefreshTokenExpiration = TokenExpiration.Sliding, 
                    //when refreshing the token, the lifetime of the refresh token will be renewed 
                    //(by the amount specified in SlidingRefreshTokenLifetime)

                 //   AllowOfflineAccess = true,

                   // AccessTokenType = AccessTokenType.Reference //unsure if this is needed
                }


            };
        }


     
        //3. 指定可以使用authorization server授权的用户

        //public static IEnumerable<TestUser> Users() {

        //    List<TestUser> ulist = new List<TestUser>();
        //    ulist = UserRepository.LoadUserList();

        //    return ulist.ToArray();                               
        //}


    }
}
