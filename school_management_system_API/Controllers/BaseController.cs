using Microsoft.AspNetCore.OData.Routing.Controllers;
using System;
using System.Linq;
using System.Security.Claims;

namespace school_management_system_API.Controllers;

public abstract class BaseController : ODataController
{
    
    protected int SchoolId
    {
        get
        {
            String value = User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name, StringComparison.OrdinalIgnoreCase))?.Value;

            if (String.IsNullOrWhiteSpace(value) || !int.TryParse(value, out int schoolId))
                return 0x0;
            
            return schoolId;
        }
    }
}