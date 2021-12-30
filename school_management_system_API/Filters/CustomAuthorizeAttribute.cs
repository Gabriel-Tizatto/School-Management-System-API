using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using school_management_system_API.Context;
using System;
using System.Linq;
using System.Security.Claims;

namespace school_management_system_API.Filters;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{

    private readonly DataBaseContext _context;
    public CustomAuthorizeAttribute(DataBaseContext context)
    {
        _context = context;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //if (context.HttpContext.Request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase) || context.HttpContext.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase) || context.HttpContext.Request.Method.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
        //{
        //    String? schoolName = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Actor, StringComparison.OrdinalIgnoreCase))?.Value;

        //    String? schoolId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Sid, StringComparison.OrdinalIgnoreCase))?.Value;

     
        //    if (!PermissionTypeEnum.Admin.GetHashCode().ToString().Equals(value, StringComparison.OrdinalIgnoreCase) && !PermissionTypeEnum.Edit.GetHashCode().ToString().Equals(value, StringComparison.OrdinalIgnoreCase))
        //        context.Result = new JsonResult(new { message = "NotAllowed" }) { StatusCode = StatusCodes.Status405MethodNotAllowed };
        //}
    }
}
