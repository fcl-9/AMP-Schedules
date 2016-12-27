using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Resources;
using AMPSchedules.Helpers;
using AMPSchedules.Models;
using AMPSystem.Classes;

using AMPSystem.Interfaces;
using Microsoft.Graph;
using Newtonsoft.Json;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class FilterController:TemplateController
    {


        public override ActionResult hook(TimeTableManager manager)
        {
            AndCompositeFilter Filters = new AndCompositeFilter(manager);
            foreach (var filter in Request.QueryString)
            {
                if (Request.QueryString[(string)filter] == "ClassName")
                {
                    IFilter nameFilter = new Name((string)filter, manager);
                    Filters.Add(nameFilter);
                }
                else if (Request.QueryString[(string)filter] == "Type")
                {
                    IFilter typeFilter = new TypeF((string)filter, manager);
                    Filters.Add(typeFilter);
                }
            }
            Filters.ApplyFilter();
            return base.hook(manager);
        }

        //Handles every request that was made by a user to filter it's activities
        [HttpGet]
        public async Task<ActionResult> AddFilter()
        {
            try
            {
                return await TemplateMethod();
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
           
        }
    }
}