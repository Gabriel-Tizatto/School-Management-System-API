using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace school_management_system_API.Models.Authentication;

public class TokenConfigurationsModel
{
    public String? Audience { get; set; }

    public String? Issuer { get; set; }

    public Int32 TokenSeconds { get; set; }

    public String Key { get; set; }

    public SymmetricSecurityKey SecurityKey { get { return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key)); } }
}
