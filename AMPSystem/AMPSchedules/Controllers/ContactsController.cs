using System.Web.Mvc;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class ContactsController : Controller
    {
        // GET: ContactsController
        public ActionResult Index()
        {
            return View($"Contacts");
        }
    }
}