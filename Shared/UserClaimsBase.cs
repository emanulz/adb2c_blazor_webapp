using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace blazorADB2CWebApp.Pages
{
    /// <summary>
    /// Base class for UserClaims component.
    /// Retrieves claims present in the ID Token issued by Azure AD.
    /// </summary>
    public class UserClaimsBase : ComponentBase
    {
        [Inject]
        private AuthenticationStateProvider? LocalAuthenticationStateProvider { get; set; }

        protected string _authMessage = "";
        protected IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();

        // Defines list of claim types that will be displayed af'ter successfull sign-in.
        private string[] returnClaims =
        {
            "idp",
            "given_name",
            "family_name",
            "oid",
            "auth_time",
            "exp"
        };

        protected override async Task OnInitializedAsync()
        {
            await GetClaimsPrincipalData();
        }

        /// <summary>
        /// Retrieves user claims for the signed-in user.
        /// </summary>
        /// <returns></returns>
        private async Task GetClaimsPrincipalData()
        {
            // Gets an AuthenticationState that describes the current user.
            var authState = await LocalAuthenticationStateProvider!.GetAuthenticationStateAsync();

            var user = authState.User;

            // Checks if the user has been authenticated.
            if (user.Identity!.IsAuthenticated)
            {
                _authMessage = $"{user.Identity.Name} is authenticated.";

                // Sets the claims value in _claims variable.
                // The claims mentioned in returnClaims variable are selected only.
                _claims = user.Claims.Where(x => returnClaims.Contains(x.Type));
            }
            else
            {
                _authMessage = "The user is NOT authenticated.";
            }
        }
    }
}
