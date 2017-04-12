using Higi.Middleware.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Higi.UI.Controllers
{
    public class UserController : Controller
    {
        private List<UserHealthData> userHealthDataList;

        public UserController()
        {
            this.userHealthDataList = new List<UserHealthData>
            {
                new UserHealthData
                {
                    UserId = 1,
                    HealthData = new Dictionary<string, string>
                    {
                        { "weight", "64kg"},
                        { "height", "5.8'" },
                        { "bmi", "2.4" }
                    }
                },
                new UserHealthData
                {
                    UserId = 2,
                    HealthData = new Dictionary<string, string>
                    {
                        { "weight", "96kg"},
                        { "height", "4.2'" },
                        { "bmi", "7.8" }
                    }
                },
                new UserHealthData
                {
                    UserId = 3,
                    HealthData = new Dictionary<string, string>
                    {
                        { "weight", "76kg"},
                        { "height", "6.1'" },
                        { "bmi", "3.6" }
                    }
                },
                new UserHealthData
                {
                    UserId = 4,
                    HealthData = new Dictionary<string, string>
                    {
                        { "weight", "82kg"},
                        { "height", "5.6'" },
                        { "bmi", "2.4" }
                    }
                }
            };
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUserHealthData(int userId)
        {
            var userHealthData = this.userHealthDataList.FirstOrDefault(u => u.UserId == userId);

            return Json(userHealthData, JsonRequestBehavior.AllowGet);
        }
    }
}