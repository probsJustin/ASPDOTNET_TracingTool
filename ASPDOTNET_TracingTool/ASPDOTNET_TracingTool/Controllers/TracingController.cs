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
using System.Web.UI.WebControls;

namespace ASPDOTNET_TracingTool.Controllers
{
    [Route("")]
    public class TracingController : Controller
    {
        string body = "";
        const string SLREQ = "X-dynaSupLabReq-info";
        const string SLRES = "X-dynaSupLabRes-info";
        const string SLPATH = "X-dynaSupLabPath-info";
        const string SLPOS = "X-dynaSupLabPosition-info";
        const string SLPASS = "url_passthrough";
        const string SLHTML = "html";
        const string SLNAME = "Dynatrace Support Lab Debug ";
        public bool isHTML = false; 
        public string line(string functionString_input = "")
        {
            if (isHTML)
            {
                return functionString_input + "<BR>";
            }
            else
            {
                return functionString_input + '\n';
            }
        }

        public string addBody(string functionString_input)
        {
            try
            {
                body += functionString_input;
                return "True";
            }catch (Exception e)
            {
                return e.ToString(); 
            }
        }
        public ContentResult requestFactory()
        {
            ContentResult returnObject = new ContentResult();
            if (Request.Params.AllKeys.Contains(SLHTML))
            {
                if (Request.Params[SLHTML] == "true")
                {
                    isHTML = true;
                }
                else
                {
                    isHTML = false;
                }
                addBody(line(SLNAME + "HTML Debug Flag Found"));
                addBody(line("HTML Parameter is set to: " + Request.Params[SLHTML] + line()));
            }
            addBody(line(SLNAME + "Headers:"));
            foreach (string instanceHeader in Request.Headers)
            {
                addBody(line(instanceHeader + " : " + Request.Headers[instanceHeader]));
            }
            returnObject.Content = body;
            return returnObject; 
        }
        [HttpGet]
        [Route("")]
        public ContentResult Get()
        {
            return requestFactory(); 
        }
        [HttpGet]
        [Route("testApi_GET")]
        public ContentResult testApi_GET()
        {

            return requestFactory(); 
        }


    }
}
