using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IOT.Models;
using IOT.BL;
using IOT.DB;

namespace IOT.Controllers
{
    public class ActionController : Controller
    {
        public readonly AppDB appDB;
        public ActionController(AppDB appDB){
            this.appDB = appDB;
        }

        [Route("Action")]
        public async Task<IActionResult> Index()
        {
            var actions = new List<Models.Action>();
            using (appDB){
                ActionRepository repo = new ActionRepository(appDB);
                actions = await repo.ReadActionsAsync();
            }
            return View(actions);
        }

        [Route("api/Action")]
        [HttpGet]
        public int getNextAction()
        {
            return ActionQueue.NextAction();
        }

        [HttpPost("api/Action/{id}")]
        public String GetProduct(int id) { 
            ActionQueue.AddAction(id);
            return "ok";
        }

    }
}
