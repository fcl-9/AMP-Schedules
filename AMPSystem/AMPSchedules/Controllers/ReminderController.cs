using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AMPSystem.Classes;
using AMPSystem.Classes.LoadData;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.DAL;
using AMPSystem.Interfaces;
using Room = AMPSystem.Models.Room;

namespace AMPSchedules.Controllers
{
    public class ReminderController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public void Add()
        {

        }

        public void Remove()
        {

        }

        public void Update()
        {
            Remove();
            Add();
        }
    }
}