using Blog;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog
{
    public class EntryModifyHandler : AuthorizationHandler<ModifyRequirement, Entry>
    {
        protected override void Handle(AuthorizationContext context, ModifyRequirement requirement, Entry resource)
        {
            if (resource.Author == context.User.Identity.Name)
            {
                context.Succeed(requirement);
            }
        }
    }
}
