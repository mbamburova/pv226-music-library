using System;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using BL.Services.UserAccounts;
using BrockAllen.MembershipReboot;

namespace PL.Helpers.Auth
{
    public class SignInManager
    {
        private readonly AuthenticationWrapper authenticationWrapper;

        public SignInManager(AuthenticationWrapper authWrapper)
        {
            authenticationWrapper = authWrapper;

            authenticationWrapper.InitializeIssueTokenAction(issueTokenAction);
            authenticationWrapper.InitializeRevokeTokenAction(revokeTokenAction);
        }

        public void SignIn(Guid userId, bool rememberMe)
        {
            authenticationWrapper.PerformSignIn(userId, rememberMe);
        }

        public void SignOut()
        {
            authenticationWrapper.PerformSignOut();
        }

        #region Actions

        private readonly Action<ClaimsPrincipal, TimeSpan?, bool?> issueTokenAction =
            (principal, tokenLifetime, persistentCookie) =>
            {
                if (principal == null)
                {
                    throw new ArgumentNullException("principal");
                }

                if (tokenLifetime == null)
                {
                    var handler =
                        FederatedAuthentication.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers[
                            typeof (SessionSecurityToken)] as SessionSecurityTokenHandler;
                    if (handler == null)
                    {
                        Tracing.Verbose(
                            "[SamAuthenticationService.IssueToken] SessionSecurityTokenHandler is not configured");
                        throw new Exception("SessionSecurityTokenHandler is not configured and it needs to be.");
                    }

                    tokenLifetime = handler.TokenLifetime;
                }

                if (persistentCookie == null)
                {
                    persistentCookie =
                        FederatedAuthentication.FederationConfiguration.WsFederationConfiguration
                            .PersistentCookiesOnPassiveRedirects;
                }

                var sam = FederatedAuthentication.SessionAuthenticationModule;
                if (sam == null)
                {
                    Tracing.Verbose(
                        "[SamAuthenticationService.IssueToken] SessionAuthenticationModule is not configured");
                    throw new Exception("SessionAuthenticationModule is not configured and it needs to be.");
                }

                var token = new SessionSecurityToken(principal, tokenLifetime.Value);
                token.IsPersistent = persistentCookie.Value;
                token.IsReferenceMode = sam.IsReferenceMode;

                sam.WriteSessionTokenToCookie(token);

                Tracing.Verbose("[SamAuthenticationService.IssueToken] cookie issued: {0}",
                    principal.Claims.GetValue(ClaimTypes.NameIdentifier));
            };

        private readonly Action revokeTokenAction =
            () =>
            {
                var sam = FederatedAuthentication.SessionAuthenticationModule;
                if (sam == null)
                {
                    Tracing.Verbose(
                        "[SamAuthenticationService.RevokeToken] SessionAuthenticationModule is not configured");
                    throw new Exception("SessionAuthenticationModule is not configured and it needs to be.");
                }

                sam.SignOut();
            };

        #endregion
    }
}