using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authorization
{
    public class RoleRequirementAttribute : TypeFilterAttribute
    {

        public RoleRequirementAttribute(ERole roleCode) : base(typeof(RoleRequirementFilter))
        {
            Arguments = new object[] { roleCode };
        }
    }
}
