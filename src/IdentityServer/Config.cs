// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        //Add support for the standard 'openid' (subject id) and 'profile' (first name, last name etc..) scopes.
        //All standard scopes and their corresponding claims can be found in the OpenID Connect specification:
        //https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims

        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[] 
            { 
                new ApiResource("api1", "Api we are trying to access"),
            };
        
        public static IEnumerable<Client> Clients =>
            new Client[] 
            { 
                new Client { 
                    ClientId = "jovanconsoleapp",
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //Oauth 2.0 ClientCredentials grant type
                    ClientSecrets =
                    {
                        new Secret("mypass".Sha256()),
                    },
                    AllowedScopes =
                    {
                        "api1",
                    },
                },
                new Client
                {
                    ClientId = "jovanmvc",
                    ClientSecrets = { new Secret("mypass2".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = true,

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1", //Add "api1" resource to the allowed scopes list
                    },

                    AllowOfflineAccess = true, //Enable support for refresh tokens
                },
                new Client
                {
                    ClientId = "jovanjs",
                    ClientName = "Js Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =           { "http://localhost:5003/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
                    AllowedCorsOrigins =     { "http://localhost:5003" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1",
                    }
                },
            };
        
    }
}