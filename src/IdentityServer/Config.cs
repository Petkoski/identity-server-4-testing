// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId()
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
            };
        
    }
}