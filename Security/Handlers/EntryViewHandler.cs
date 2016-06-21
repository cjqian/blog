using Blog;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog
{
    public class EntryViewHandler : AuthorizationHandler<ViewRequirement, Entry>
    {
        protected override void Handle(AuthorizationContext context, ViewRequirement requirement, Entry resource)
        {
            if (!resource.IsPublic && context.User.Identity.Name != resource.Author)
            {
                context.Fail();
            } else
            {
                context.Succeed(requirement);
            }
        }
    }
}
