namespace DSR.CrudCats.Auth
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Security.Principal;
    using System.Threading.Tasks;

    public interface IIdentityHelper
    {
        int CurrentUserId { get; }
    }

    class IdentityHelper : IIdentityHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        public IdentityHelper(IHttpContextAccessor httpContextAccessor, ILogger<IdentityHelper> logger)
        {
            logger.LogTrace("{method} > ...", nameof(IdentityHelper));
            (_httpContextAccessor, _logger) = (httpContextAccessor, logger);
            logger.LogTrace("{method} <", nameof(IdentityHelper));
        }

        public int CurrentUserId
        {
            get
            {
                 _logger.LogTrace("{method} >", nameof(CurrentUserId));
                 
                 var currentUserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.Identity.Name);
                 
                 _logger.LogTrace("{method} < {currentUserId}", nameof(CurrentUserId), currentUserId);
                 return currentUserId;
            }
        }
    }
}
