using System.Threading.Tasks;
using System.Web.Mvc;
using Resources;
using AMPSystem.Classes;
using AMPSystem.Classes.Filters;
using AMPSystem.Interfaces;
using Microsoft.Graph;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class FilterController: TemplateController
    {
        public override ActionResult Hook(TimeTableManager manager)
        {
            var filters = new AndCompositeFilter(manager);
            foreach (var filter in Request.QueryString)
            {
                if (Request.QueryString[(string)filter] == "ClassName")
                {
                    IFilter nameFilter = new Name((string)filter, manager);
                    filters.Add(nameFilter);
                }
                else if (Request.QueryString[(string)filter] == "Type")
                {
                    IFilter typeFilter = new TypeF((string)filter, manager);
                    filters.Add(typeFilter);
                }
            }
            filters.ApplyFilter();
            return base.Hook(manager);
        }

        //Handles every request that was made by a user to filter activities.
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
                return RedirectToAction($"Index", $"Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }
    }
}