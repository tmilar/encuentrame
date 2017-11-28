using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Positions;
using Encuentrame.Security.Authorizations;
using Encuentrame.Web.Models.Positions;
using NailsFramework.IoC;

namespace Encuentrame.Web.Controllers
{
    [AuthorizationPass(new[] { RoleEnum.Administrator })]
    public class PositionController : ListBaseController<Position,PositionListModel>
    {
        [Inject]
        public IPositionCommand PositionCommand { get; set; }
        // GET: Position
        public ActionResult Index()
        {
            return View();
        }

        protected override PositionListModel GetViewModelFrom(Position item)
        {
            return new PositionListModel()
            {
                Id = item.Id,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                UserId = item.UserId,
                Creation = item.Creation,
            };
        }
      

    }
}