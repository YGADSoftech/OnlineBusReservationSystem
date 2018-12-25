using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleReservationSystem.Domain.VehicleManagement;
using VehicleReservationSystem.WebUI.Models;

namespace VehicleReservationSystem.WebUI.Controllers
{
    public class TypesController : Controller
    {
        // GET: Types
        [HttpGet]

        public ActionResult Manage()
        {
            return View(new VehiclesHandlers().GetVehicleTypes().ToModelList());
        }
       
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("~/Views/Types/_Create.cshtml");
        }

        [HttpPost]

        public ActionResult Create(VehicleTypeModel model)
        {
            try
            {
                new VehiclesHandlers().AddVehicleType(model.ToEntity());
                TempData.Add("AlertMessage", new AlertModel("The Vehicle Type is add Successfully", AlertModel.AlertType.Success));

            }

            catch(Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to add Vehicle Type!", AlertModel.AlertType.Error));
            }
            return RedirectToAction("Manage");
        }

        [HttpPost]

        public ActionResult Delete(VehicleTypeModel model,int id)
        {
            try
            {
                VehicleTypeModel model1 = new VehiclesHandlers().GetVehicleType(id).ToModel();
                TempData["model1"] = model1.Id;
                new VehiclesHandlers().DeleteVehicleType(model.ToEntity());
                TempData.Add("AlertMessage", new AlertModel("The Vehicle type is deleted successfully!", AlertModel.AlertType.Success));

            }
            catch(Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to delete vehicle tyoe", AlertModel.AlertType.Error));
            }

            return Json(Url.Action($"Manage/{TempData["model1"]}"));
        }



        [HttpGet]

        public ActionResult Edit(int id)
        {
            VehicleTypeModel model = new VehiclesHandlers().GetVehicleType(id).ToModel();
            return PartialView("~/Views/Types/_Edit.cshtml", model);
        }

        [HttpPost]

        public ActionResult Edit(VehicleTypeModel model)
        {
            try
            {
                new VehiclesHandlers().UpdateVehicleType(model.ToEntity());
                TempData.Add("AlertMessage", new AlertModel("The Vehicle type is Updated successfully!", AlertModel.AlertType.Success));

            }
            catch (Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to Update vehicle tyoe!", AlertModel.AlertType.Error));
            }

            return RedirectToAction("Manage");
        }
    }
}