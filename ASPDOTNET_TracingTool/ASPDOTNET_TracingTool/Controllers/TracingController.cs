using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Text;

namespace ASPDOTNET_TracingTool.Controllers
{
    [Route("")]
    public class TracingController : Controller
    {
        [HttpGet]
        public string Get()
        {

            return "sure";
        }


    }
}
