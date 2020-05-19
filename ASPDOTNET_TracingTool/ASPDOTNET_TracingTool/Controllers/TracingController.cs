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
        const string SLRES_CODE = "responseCode"; 
        public bool isHTML = false;
        public bool isDebug = false;

        public string checkRequestParams(HttpRequestBase functionHttpRequest_Input, string functionString_SearchItem)
        {
            string returnObject = "Empty";
            try
            {
                if (functionHttpRequest_Input.Params.AllKeys.Contains(functionString_SearchItem))
                {
                    returnObject = functionHttpRequest_Input.QueryString[functionString_SearchItem];
                }
                else
                {
                    returnObject = "Empty";
                }
            }catch(Exception exc)
            {
                returnObject = "Empty";
            }
            return returnObject; 
        }
        public string checkRequestHeader(HttpRequestBase functionHttpRequest_Input, string functionString_SearchItem)
        {
            string returnObject = "Empty";
            try
            {
                returnObject = functionHttpRequest_Input.QueryString[functionString_SearchItem];
            }
            catch (Exception exc)
            {
                returnObject = "Empty";
            }
            return returnObject; 
        }
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
        public Dictionary<string, string> generateRequest(string string_function_url)
        {
            Dictionary<string, string> returnObject = new Dictionary<string, string>();
            HttpWebRequest requestInstance = (HttpWebRequest)WebRequest.Create(string_function_url);
            HttpWebResponse responseInstance = null; 
            responseInstance = (HttpWebResponse)requestInstance.GetResponse();
            returnObject["content"] = responseInstance.ToString(); 
            return returnObject; 
        }
        public ContentResult requestFactory()
        {
            ContentResult returnObject = new ContentResult();
            string string_PassThroughURL_Param = checkRequestParams(Request, SLPASS);
            string string_responseCode_Param = checkRequestParams(Request, SLRES_CODE);
            string string_SLREQ_Header = checkRequestHeader(Request, SLREQ);
            string string_SLRES_Header = checkRequestHeader(Request, SLRES);
            string string_SLPATH_Header = checkRequestHeader(Request, SLPATH);
            string string_SLPOS_Header = checkRequestHeader(Request, SLPOS); 
                // check for the debug parameter is equal to true/1/True
                try
                {
                    if (checkRequestParams(Request, "debug").ToLower() == "true" | checkRequestParams(Request, "debug") == "1")
                    {
                        isDebug = true;
                    }
                    if (checkRequestParams(Request, "Debug").ToLower() == "true" | checkRequestParams(Request, "Debug") == "1")
                    {
                        isDebug = true;
                    }
                    if (isDebug)
                    {
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
                    }
                    else
                    {

                    }
                    if(string_SLREQ_Header != "Empty")
                    {
                        if (isDebug)
                        {
                            // dynatrace support lab request fake header found
                            addBody(line(SLNAME + " DynaSupLabReq header found:")); 
                        }
                    }
                    if(string_SLRES_Header != "Empty")
                    {
                        if (isDebug)
                        {
                            // dynatrace support lab response fake header found
                            addBody(line(SLNAME + " DynaSupLabRes header found:")); 
                                
                        }
                    }
                    if (string_SLPATH_Header != "Empty")
                    {
                        if (isDebug)
                        {
                            // dynatrace support path header found
                            addBody(line(SLNAME + " path header found:"));
                        }
                    }
                    if(string_SLPOS_Header != "Empty")
                    {
                        if (isDebug)
                        {
                            // dynatrace support position header found
                            addBody(line(SLNAME + " position header found:"));
                        }
                    }
                    if (string_PassThroughURL_Param != "Empty")
                    {
                    //create url request
                        if (isDebug)
                        {
                            // dynatrace url pass through parameter found 
                            addBody(line(SLNAME + " url pass through parameter found"));
                            addBody(line(generateRequest(string_PassThroughURL_Param)["content"]));
                        }
                        
                    }
                if (!isDebug)
                {
                    addBody(line("Debug : False"));
                }
                }
                catch(Exception e)
                {
                    // print exception 
                    addBody(line(SLNAME + " Exception Found: " + line(e.ToString()))); 
                }
            returnObject.Content = body;
            return returnObject; 
        }

        [HttpGet]
        [Route("apiTest_GET")]
        public ContentResult testApi_GET()
        {

            return requestFactory();
        }
        [HttpGet]
        [Route("")]
        public ContentResult index()
        {

            return requestFactory();
        }
    }
}
