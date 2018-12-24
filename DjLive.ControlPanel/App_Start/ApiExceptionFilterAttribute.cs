using System.CodeDom;
using System.Net.Http;
using System.Web.Http.Filters;

namespace DjLive.ControlPanel
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            //业务异常
            var exception = context.Exception;
            if (exception is CustomException)
            {
                
            }
        }
    }
}