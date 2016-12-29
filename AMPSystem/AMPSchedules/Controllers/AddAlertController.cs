using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Input;
using AMPSystem.Classes;
using AMPSystem.Interfaces;
using Microsoft.Graph;
using Newtonsoft.Json.Linq;
using Resources;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class AddAlertController : TemplateController
    {
        // GET: AddAlert
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                return await TemplateMethod();
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction($"Index", $"Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }

        public override ActionResult Hook(TimeTableManager manager)
        {
            //["Name"].Value<string>()
            //foreach (var key in Request.Form)
            //{
            //    Debug.Write(key);
            //    Debug.Write(Request.Form);
            //}
            string[] keys = Request.QueryString.AllKeys;
            for (var i = 0 ; i < keys.Length; i++)
            {
               // Debug.Write(Request.QueryString[i]);
               var dataParsed = JArray.Parse(Request.QueryString[i]);
                //dataParsed[i].Value<String>()
                foreach (var data in dataParsed)
                {
                    var name = data["name"].Value<string>();
                    var startTime = Convert.ToDateTime(data["startTime"].Value<string>());
                    var endTime = Convert.ToDateTime(data["endTime"].Value<string>());

                    var time = data["time"].Value<int>();
                    var units = data["unit"].Value<string>();

                    //BEGIN - This parametrs should be passed to timetable 
                    var start = Convert.ToDateTime(data["start"].Value<string>());
                    var end = Convert.ToDateTime(data["end"].Value<string>());
                    //END - This parametrs should be passed to timetable 



                    Debug.Write(name +" "+ startTime + " "+ endTime + " " + time +  " " + units + " " + start + " "+ end );
                    //Podes ver no output do debug que esta tudo funcional :D
                }
            }



            return base.Hook(manager);
            
        }
    }
}