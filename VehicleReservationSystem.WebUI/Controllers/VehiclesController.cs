using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleReservationSystem.Domain.VehicleManagement;
using VehicleReservationSystem.WebUI.Models;

namespace VehicleReservationSystem.WebUI.Controllers
{
    public class VehiclesController : Controller
    {
        // GET: Vehicles
        [HttpGet]
        public ActionResult Manage()
        {
            List<VehiclesModel> model = new VehiclesHandlers().GetVehicles().ToModelList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Types = new VehiclesHandlers().GetVehicleTypes().ToSelectItemList();
            return PartialView("~/Views/Vehicles/_Create.cshtml");
        }

        [HttpPost]
        public ActionResult Create(VehiclesModel model, FormCollection data)
        {
            try
            {

                Vehicle v = new Vehicle();
                v.Name = model.Name;
                v.RegistrationNumber = model.RegistrationNumber;
                v.TotalSeats = model.Total_Seats;
                HttpPostedFileBase pic = Request.Files["pic"];
                if (pic != null && pic.ContentLength > 0)
                {
                    long uno = DateTime.Now.Ticks;
                    string url = $"~/Images/Vehicles/{uno}{pic.FileName.Substring(pic.FileName.LastIndexOf('.'))}";
                    string path = Server.MapPath(url);
                    pic.SaveAs(path);
                    v.ImageUrl = url;
                }
                v.VehicleType = new VehicleType { Id = Convert.ToInt32(data["VehicleType"]) };
                new VehiclesHandlers().AddVehicle(v);
                TempData.Add("AlertMessage", new AlertModel("The Vehicle is added Successfully!", AlertModel.AlertType.Success));

            }
            catch (Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to add vehicle", AlertModel.AlertType.Error));
            }
            return RedirectToAction("Manage");
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Vehicle v = new VehiclesHandlers().GetVehicle(id);
                TempData["v"] = v.VehicleType.Id;
                new VehiclesHandlers().DeleteVehicle(id);
                TempData.Add("AlertMessage", new AlertModel("The Vehicle is deleted successfully!", AlertModel.AlertType.Success));
            }
            catch (Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to delete vehicle!", AlertModel.AlertType.Error));

            }


            return Json(Url.Action($"Manage/{TempData["v"]}"));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.Types = new VehiclesHandlers().GetVehicleTypes();
            Vehicle v = new VehiclesHandlers().GetVehicle(id);
            return PartialView("~/Views/Vehicles/_Edit.cshtml", v);
        }

        [HttpPost]

        public ActionResult Edit(FormCollection data)
        {
            try
            {
                Vehicle v = new Vehicle();
                v.Id = Convert.ToInt32(data["id"]);
                v.Name = data["Name"];
                v.RegistrationNumber = data["Regno"];

                HttpPostedFileBase pic = Request.Files["pic"];
                if (pic != null && pic.ContentLength > 0)
                {
                    long uno = DateTime.Now.Ticks;
                    string url = $"~/Images/Vehicles/{uno}{pic.FileName.Substring(pic.FileName.LastIndexOf('.'))}";
                    string path = Server.MapPath(url);
                    pic.SaveAs(path);
                    v.ImageUrl = url;
                }
                v.VehicleType = new VehicleType { Id = Convert.ToInt32(data["type"]) };
                new VehiclesHandlers().UpdateVehicle(v);

                TempData.Add("AlertMessage", new AlertModel("Vehicle is  updated successfully", AlertModel.AlertType.Success));
            }
            catch (Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("failed to update vehicle", AlertModel.AlertType.Error));
            }


            return RedirectToAction("Manage");
        }
    }
}