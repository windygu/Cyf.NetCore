﻿using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cyf.AuthorizationServer.Models
{
    /// <summary>
    /// 1、Identity测试使用
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 1、微服务API资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
                {
                    new ApiResource("TeamService", "TeamService api需要被保护")
                };
        }
        /// <summary>
        /// 2、客户端
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
    
                    // 没有交互性用户，使用 clientid/secret 实现认证。
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
    
                    // 用于认证的密码
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    // 客户端有权访问的范围（Scopes）
                    AllowedScopes = { "TeamService" }
                },
                new Client
                {
                    ClientId = "client-password",
    	            // 使用用户名密码交互式验证
    	            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
    
    	            // 用于认证的密码
    	            ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
    	            // 客户端有权访问的范围（Scopes）
    	            AllowedScopes = { "TeamService" }
                },
                // openid客户端
                new Client
                {
                    ClientId="client-code",
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.Code,
                    RequireConsent=false,
                    RequirePkce=true,

                    RedirectUris={ "https://localhost:5006/signin-oidc"},//客户端地址

                    PostLogoutRedirectUris={ "https://localhost:5006/signout-callback-oidc"},

                    AllowedScopes=new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "TeamService" // 启用对刷新令牌的支持
                    },

                    // 增加授权访问
                    AllowOfflineAccess=true
                },
            };
        }

        /// <summary>
        /// 客户端下面的用户
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser
                {
                    SubjectId="1",
                    Username="tony",
                    Password="123456"
                },
                // openid 身份验证
                new TestUser{SubjectId = "818727", Username = "张三", Password = "123456",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "张三"),
                        new Claim(JwtClaimTypes.GivenName, "三"),
                        new Claim(JwtClaimTypes.FamilyName, "张"),
                        new Claim(JwtClaimTypes.Email, "zhangsan@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://zhangsan.com"),
                      //  new Claim(JwtClaimTypes.Address, @"{ '城市': '杭州', '邮政编码': '310000' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> Ids => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
    }

    public class InMemoryConfiguration
    {
        public static IConfiguration Configuration { get; set; }
        /// <summary>
        /// Define which APIs will use this IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("clientservice", "CAS Client Service"),
                new ApiResource("productservice", "CAS Product Service"),
                new ApiResource("agentservice", "CAS Agent Service")
            };
        }

        /// <summary>
        /// Define which Apps will use thie IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "client.api.service",
                    ClientSecrets = new [] { new Secret("clientsecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] { "clientservice" }
                },
                new Client
                {
                    ClientId = "product.api.service",
                    ClientSecrets = new [] { new Secret("productsecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] { "clientservice", "productservice" }
                },
                new Client
                {
                    ClientId = "agent.api.service",
                    ClientSecrets = new [] { new Secret("agentsecret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new [] { "agentservice", "clientservice", "productservice" }
                }
            };
        }

        /// <summary>
        /// Define which uses will use this IdentityServer
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestUser> GetUsers()
        {
            return new[]
            {
                new TestUser
                {
                    SubjectId = "10001",
                    Username = "edison@hotmail.com",
                    Password = "edisonpassword"
                },
                new TestUser
                {
                    SubjectId = "10002",
                    Username = "andy@hotmail.com",
                    Password = "andypassword"
                },
                new TestUser
                {
                    SubjectId = "10003",
                    Username = "leo@hotmail.com",
                    Password = "leopassword"
                }
            };
        }
    }

}
