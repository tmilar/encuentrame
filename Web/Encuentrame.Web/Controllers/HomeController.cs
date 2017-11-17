using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Dashboards;
using Encuentrame.Security.Authorizations;
using Encuentrame.Support;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.Models.Homes;
using NailsFramework.IoC;
using NailsFramework.UnitOfWork;

namespace Encuentrame.Web.Controllers
{
    [AuthorizationPass(new[] { RoleEnum.Administrator, RoleEnum.EventAdministrator })]
    public class HomeController : BaseController
    {
        [Inject]
        public IDashboardCommand DashboardCommand { get; set; }

        [UnitOfWork(TransactionMode = TransactionMode.NoTransaction)]
        public ActionResult Index()
        {
            var business = GetLoggedUser().Business;
            var model = new HomeModel()
            {
                PeopleThatIAmTracking = DashboardCommand.PeopleThatIAmTracking(business?.Id),
                PeopleInEvents = DashboardCommand.PeopleInEvents(business?.Id),
                EventsInEmergency = DashboardCommand.EventsInEmergency(business?.Id),
                PeopleWithoutAnswer = DashboardCommand.PeopleWithoutAnswer(business?.Id),
            };
            return View(model);

        }

        [HttpPost]
        public JsonResult EventsByStatus()
        {
            var business = GetLoggedUser().Business;
            var eventsByStatus = DashboardCommand.EventsByStatus(business?.Id);

            var pieModels = new List<PieModel>();

            pieModels.Add(new PieModel()
            {
                Value = eventsByStatus.InEmergency,
                Label = Translations.InEmergency,
                Color = "#F7464A",
                Highlight = "#FF5A5E",
            });

            pieModels.Add(new PieModel()
            {
                Value = eventsByStatus.Pending,
                Label = Translations.Pending,
                Color = "#1989c0",
                Highlight = "#007cba",
            });
            pieModels.Add(new PieModel()
            {
                Value = eventsByStatus.InProgress,
                Label = Translations.InProgress,
                Color = "#FDB45C",
                Highlight = "#FFC870",
            });
            return Json(JsReturnHelper.Return(pieModels));
        }

        [HttpPost]
        public JsonResult EventsAlongTheTime()
        {
            var business = GetLoggedUser().Business;
            var today = SystemDateTime.Now.SetAtBeginDay();
            var begin = today.AddMonths(-11).SetAtBeginDay();
            var eventsAlongTheTime = DashboardCommand.EventsAlongTheTime(business?.Id);

            var lineModel = new LineModel();
            var labels=new List<DateTime>(); 
            for (int i = 0; i < 12; i++)
            {
                labels.Add(begin);
                begin=begin.AddMonths(1);
            }

            var temp = new List<EventsAlongTheTimeQuantityInfo>();
            foreach (var label in labels)
            {
                var item=eventsAlongTheTime.FirstOrDefault(x => x.Year==label.Year && x.Month==label.Month);

                if (item!=null)
                {
                    temp.Add(item);
                }
                else
                {
                    temp.Add(new EventsAlongTheTimeQuantityInfo()
                    {
                        All = 0,
                        InEmergency = 0,
                        Month = label.Month,
                        Year = label.Year,
                    });
                }
            }
            eventsAlongTheTime = temp;

         

            lineModel.Labels = eventsAlongTheTime.Select(x => $"{x.Year}-{x.Month} ").ToArray();
            lineModel.Data1 = eventsAlongTheTime.Select(x => x.All).ToArray();
            lineModel.Data2 = eventsAlongTheTime.Select(x => x.InEmergency).ToArray();

            return Json(JsReturnHelper.Return(lineModel));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}