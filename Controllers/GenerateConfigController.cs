using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Services;
using Ticket.Filters;
using Ticket.Models;

namespace Ticket.Controllers
{
    [HMACAuthentication]
    [RoutePrefix("api/Configuration")]
    public class GenerateConfigController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("")]
        public IHttpActionResult Get()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            var Name = ClaimsPrincipal.Current.Identity.Name;

            return Ok(db.EmailConfigurations);
        }
    }
}
