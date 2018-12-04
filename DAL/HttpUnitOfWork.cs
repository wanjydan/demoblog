using DAL.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DAL
{
    public class HttpUnitOfWork : UnitOfWork
    {
        public HttpUnitOfWork(ApplicationDbContext context, IAccountManager accountManager,
            IHttpContextAccessor httpAccessor) : base(context)
        {
            context.CurrentUser = httpAccessor.HttpContext == null ? null : accountManager.GetCurrentUserAsync().Result;
        }
    }
}