using CommanBusinessLogic;
using GailconnectLiveEvents.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JWT_TOKEN_Application.Controllers
{
    [RoutePrefix("api/LiveEvents")]
    public class LiveEventsController : ApiController
    {

        [HttpGet]
        [Route("one")]
        [Obsolete]
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage one()
        {
            HttpResponseMessage MyHttpResponseMessage = new HttpResponseMessage();
            MyHttpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, new { data = "Admin access granted" });
            return MyHttpResponseMessage;
        }
        [HttpGet]
        [Obsolete]
        [Route("two")]
        [Authorize(Roles = "User")]
        public HttpResponseMessage two()
        {
            HttpResponseMessage MyHttpResponseMessage = new HttpResponseMessage();
            MyHttpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, new { data = "User access granted" });
            return MyHttpResponseMessage;
        }
        
        [HttpGet]
        [Route("three")]
        [Obsolete]
        [Authorize]
        public HttpResponseMessage three()
        {
            HttpResponseMessage MyHttpResponseMessage = new HttpResponseMessage();
            MyHttpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, new { data = "only access granted" });
            return MyHttpResponseMessage;
        }
        [HttpGet]
        [Route("four")]
        [Obsolete]
        [Authorize(Roles = "Admin")]
        public HttpResponseMessage four()
        {
            HttpResponseMessage MyHttpResponseMessage = new HttpResponseMessage();
            MyHttpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, new { data = "Admin access granted" });
            return MyHttpResponseMessage;
        }
        [HttpGet]
        [Route("five")]
        [Obsolete]
        [AllowAnonymous]
        public HttpResponseMessage five()
        {
            HttpResponseMessage MyHttpResponseMessage = new HttpResponseMessage();
            MyHttpResponseMessage = Request.CreateResponse(HttpStatusCode.OK, new { data = "Allow Anonymous " });
            return MyHttpResponseMessage;
        }

    }
}
