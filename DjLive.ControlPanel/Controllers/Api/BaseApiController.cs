using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;

namespace DjLive.ControlPanel.Controllers.Api
{
    public class BaseApiController : ApiController
    {
        protected override JsonResult<T> Json<T>(T content, JsonSerializerSettings serializerSettings, Encoding encoding)
        {
            return base.Json(content, serializerSettings, encoding);
        }
    }
}
