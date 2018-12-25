using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleReservationSystem.Domain.VehicleManagement;
using VehicleReservationSystem.WebUI.Models;

namespace VehicleReservationSystem.WebUI.Controllers
{
    public class SchedulesController : Controller
    {
        // GET: Schedules
        public ActionResult Manage()
        {
            List<TimeTable> find = new VehiclesHandlers().GetSchedules();
            return View(find);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Vehicles = new VehiclesHandlers().GetVehicles().ToSelectItemList();
            ViewBag.Routes = new VehiclesHandlers().GetRoutes().ToSelectItemList();
            
            return PartialView("~/Views/Schedules/_Create.cshtml");

        }

        [HttpPost]
        public ActionResult Create(FormCollection data)
        {
            try
            {
                TimeTable tt = new TimeTable();
                
                tt.DepartureTime = Convert.ToDateTime( data["DepartureTime"]).TimeOfDay;
                tt.Vehicle = new Vehicle { Id = Convert.ToInt32(data["VehicleName"]) };
                tt.Route = new Route { Id = Convert.ToInt32(data["RouteName"]) };
                new VehiclesHandlers().AddSchedule(tt);
                TempData.Add("AlertMessage", new AlertModel("Schedule is added successfully", AlertModel.AlertType.Success));


            }
            catch (Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to add schedule", AlertModel.AlertType.Error));

            }
            return RedirectToAction("Manage");
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                TimeTable tt = new VehiclesHandlers().GetSchedule(id);
                TempData["tt"] = tt.Vehicle.Id;
                TempData["tt"] = tt.Route.Id;
                new VehiclesHandlers().DeleteSchedule(id);
                TempData.Add("ALertMessage", new AlertModel("The Schedule is deleted succussfully", AlertModel.AlertType.Success));
            }
            catch(Exception)
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to delete schedule!", AlertModel.AlertType.Error));
            }

            return Json(Url.Action($"Manage/{TempData["tt"]}"));
        }



        [HttpGet]

        public ActionResult Edit(int id)
        {
            ViewBag.Vehicles = new VehiclesHandlers().GetVehicles();
            ViewBag.Routes = new VehiclesHandlers().GetSchedules();
            TimeTable tt = new VehiclesHandlers().GetSchedule(id);
            return PartialView("~/Views/Schedules/_Edit.cshtml", tt);

        }

        [HttpPost]
        public ActionResult Edit(FormCollection data)
        {
            try
            {
                TimeTable tt = new TimeTable();
                tt.Id = Convert.ToInt32( data["id"]);
                tt.DepartureTime = Convert.ToDateTime( data["DepartureTime"]).TimeOfDay;
                tt.Vehicle = new Vehicle { Id = Convert.ToInt32(data["vehicle"]) };
                tt.Route = new Route { Id = Convert.ToInt32(data["routename"]) };
                new VehiclesHandlers().UpdateSchedule(tt);
                TempData.Add("AlertMessage", new AlertModel("The Schedule is updated successfully!", AlertModel.AlertType.Success));



            }

            catch (Exception ex)
            {
                TempData.Add("ALertMessage", new AlertModel("Failed to update Schedule", AlertModel.AlertType.Error));
            }

            return RedirectToAction("Manage");
        }
    }
}