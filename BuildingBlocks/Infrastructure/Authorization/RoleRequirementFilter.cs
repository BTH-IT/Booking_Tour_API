using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authorization
{
    public class RoleRequirementFilter : IAuthorizationFilter
    {
        private readonly ERole RoleCode;
        public RoleRequirementFilter(ERole RoleCode)
        {
            this.RoleCode = RoleCode;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var result  = int.TryParse(context.HttpContext.User.FindFirstValue(ClaimTypes.Role), out int role);
            if(!result) context.Result = new ForbidResult();
            if (role != (int)RoleCode) context.Result = new ForbidResult();
        }
    }
}
