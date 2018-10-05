using System;
using System.Collections.Generic;
using System.Text;
using AspNet.Security.OpenIdConnect.Primitives;
using DAL.Core;
using DAL.Core.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Http;

namespace DAL
{
    public class HttpUnitOfWork: UnitOfWork
    {
        public HttpUnitOfWork(ApplicationDbContext context, IAccountManager _accountManager, IHttpContextAccessor httpAccessor) : base(context)
        {
            context.CurrentUser = httpAccessor.HttpContext == null ? null : _accountManager.GetCurrentUser().Result;
        }
    }
}
