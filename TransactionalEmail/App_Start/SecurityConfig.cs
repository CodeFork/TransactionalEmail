using System.Web.Http;
using ApiSecurity.Filters;
using TransactionalEmail;


[assembly: WebActivatorEx.PreApplicationStartMethod(
    typeof(SecurityConfig), "Register")]

namespace TransactionalEmail
{
    public class SecurityConfig
    {
        public static void Register()
        {
            GlobalConfiguration.Configuration.Filters.Add(new RequireHttpsAttribute());
            GlobalConfiguration.Configuration.Filters.Add(new IdentityBasicAuthenticationAttribute());
            GlobalConfiguration.Configuration.Filters.Add(new AuthorizeAttribute());
        }
    }
}
