using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace uBeac.Web.Api.Controllers
{
    public class BodyAttribute : FromBodyAttribute
    {
    }

    public class QueryAttribute : FromQueryAttribute
    {
    }

    public class DeleteAttribute : HttpDeleteAttribute
    {
        public DeleteAttribute()
        {
        }
        public DeleteAttribute(string template) : base(template)
        {
        }
    }

    public class DeleteByIdAttribute : DeleteAttribute
    {
        public DeleteByIdAttribute() : base("{id}")
        {
        }
    }


    public class GetAttribute : HttpGetAttribute
    {
        public GetAttribute()
        {
        }
        public GetAttribute(string template) : base(template)
        {
        }
    }
    public class GetByIdAttribute : GetAttribute
    {
        public GetByIdAttribute() : base("{id}")
        {
        }
    }

    public class PostAttribute : HttpPostAttribute
    {
        public PostAttribute()
        {
        }
        public PostAttribute(string template) : base(template)
        {
        }
    }
    public class PostByIdAttribute : PostAttribute
    {
        public PostByIdAttribute() : base("{id}")
        {
        }
    }

    public class RolesAttribute : AuthorizeAttribute
    {
        public RolesAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }

    public class AnonymousAttribute : AllowAnonymousAttribute
    {
    }
}
