using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO;
using IT.Netic.DispatchIt.Web.Backend.Services.Interfaces;
using System;

namespace IT.Netic.DispatchIt.Web.Backend.Services
{
    public class ValidationService : IValidationService
    {
        private ConfigurationManager<OpenIdConnectConfiguration> _configurationManager;
        private ClaimsPrincipal _claimsPrincipal;

        private string _wellKnownEndpoint = string.Empty;
        private string _tenantId = string.Empty;
        private string _audience = string.Empty;
        private string _instance = string.Empty;

        public ValidationService()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = configBuilder.Build();

            _tenantId = configuration.GetConnectionString("TenantId");
            _audience = configuration.GetConnectionString("ClientId");
            _instance = configuration.GetConnectionString("Instance");
            _wellKnownEndpoint = $"{_instance}{_tenantId}/v2.0/.well-known/openid-configuration";
        }
        public async Task<ClaimsPrincipal> ValidateTokenAsync(string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return null;
            }

            if (!authorizationHeader.Contains("Bearer"))
            {
                return null;
            }
            var accessToken = authorizationHeader.Substring("Bearer ".Length);
            var oidcWellknownEndpoints = await GetOIDCWellknownConfiguration();

            var tokenValidator = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                RequireSignedTokens = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                IssuerSigningKeys = oidcWellknownEndpoints.SigningKeys,
                ValidIssuer = oidcWellknownEndpoints.Issuer
            };

            try
            {
                SecurityToken securityToken;
                _claimsPrincipal = tokenValidator.ValidateToken(accessToken, validationParameters, out securityToken);

                return _claimsPrincipal;
            }
            catch(Exception ex)
            {
                var eMessage = ex.Message;
            }
            return null;
        }
        private async Task<OpenIdConnectConfiguration> GetOIDCWellknownConfiguration()
        {
            _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                 _wellKnownEndpoint, new OpenIdConnectConfigurationRetriever());

            return await _configurationManager.GetConfigurationAsync();
        }

    }
}
