﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMPSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AMPSchedules.Controllers
{
    public class ColorController : Controller
    {
        public AMPSystemFacade Facade { get; set; }

        // GET: Color/Add
        public void Add()
        {
        }
    }
}