using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleReservationSystem.Domain.VehicleManagement;
using VehicleReservationSystem.WebUI.Models;

namespace VehicleReservationSystem.WebUI.Controllers
{
    public class RoutesController : Controller
    {
        // GET: Routes
        public ActionResult Manage()
        {
            List<RouteModel> model = new VehiclesHandlers().GetRoutes().ToModelList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Cities = new VehiclesHandlers().GetCities().ToSelectItemList();
           
            return PartialView("~/Views/Routes/_Create.cshtml");
        }

        [HttpPost]

        public ActionResult Create(FormCollection data)
        {
            //try
            //{
               Route r = new Route();
                r.Origin = new City { Id = Convert.ToInt32(data["Origin"]) };
                r.Destination = new City { Id = Convert.ToInt32(data["Dest"]) };
                r.Price = Convert.ToSingle(data["Price"]);
                r.TravelTime = Convert.ToDateTime(data["TravelTime"]).TimeOfDay;
                new VehiclesHandlers().AddRoute(r);
                TempData.Add("AlertMessage", new AlertModel("Successfuly added", AlertModel.AlertType.Success));
           
            //}
            //catch 
            //{
            //    TempData.Add("AlertMessage", new AlertModel("Failed to add the Route", AlertModel.AlertType.Error));
            //}
            return RedirectToAction("Manage");
        }


        [HttpPost]

        public ActionResult Delete(int id)
        {

            try
            {
                Route r = new VehiclesHandlers().GetRoute(id);
                new VehiclesHandlers().DeleteRoute(id);
                TempData.Add("AlertMessage", new AlertModel("The Route is deleted successfully", AlertModel.AlertType.Success));
            }
            catch
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to delete the Route", AlertModel.AlertType.Error));
            }
            return Json(Url.Action("Manage"));
        }


        [HttpGet]

        public ActionResult Edit(int id)
        {
            ViewBag.Cities = new VehiclesHandlers().GetCities();
           
            Route model = new VehiclesHandlers().GetRoute(id);
            return PartialView("~/Views/Routes/_Edit.cshtml", model);
        }

        [HttpPost]

        public ActionResult Edit(FormCollection data)
        {
            try
            {
                Route r = new Route();
                r.Id = Convert.ToInt32(data["id"]);
                r.Origin = new City { Id = Convert.ToInt32(data["Origin"]) };
                r.Destination = new City { Id = Convert.ToInt32(data["Destination"]) };
                r.Price = Convert.ToSingle(data["Price"]);
                r.TravelTime = Convert.ToDateTime(data["TravelTime"]).TimeOfDay;


                new VehiclesHandlers().UpdateRoute(r);

                TempData.Add("AlertMessage", new AlertModel("The Route is Updated successfully", AlertModel.AlertType.Success));
            }
            catch
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to Updates the Route", AlertModel.AlertType.Error));
            }
            return RedirectToAction("Manage");

        }




    }
}