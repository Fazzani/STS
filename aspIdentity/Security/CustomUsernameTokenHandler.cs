using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Collections.ObjectModel;
using System.IdentityModel.Configuration;
using System.IdentityModel.Selectors;

namespace DarksideCookie.AspNet.FedAuth.CustomSTS.Security
{
  public class CustomUsernameTokenHandler : SamlSecurityTokenHandler
  {
    public override bool CanValidateToken { get { return true; } }
    public override ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
    {
      UserNameSecurityToken userNameToken = token as UserNameSecurityToken;
      if (userNameToken.UserName.Equals(userNameToken.Password))
        throw new SecurityTokenValidationException("Invalid credentials");
      var claim = new Claim(System.IdentityModel.Claims.ClaimTypes.Name, userNameToken.UserName);
      var claimId = new Claim(System.IdentityModel.Claims.ClaimTypes.PPID, "CustomUsernameTokenHandler");
      var identity = new ClaimsIdentity(new Claim[] { claim }, "NameToken");
      return new ReadOnlyCollection<ClaimsIdentity>(new ClaimsIdentity[] { });
    }
  }
}