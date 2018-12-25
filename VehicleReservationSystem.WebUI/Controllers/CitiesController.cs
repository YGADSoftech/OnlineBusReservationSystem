using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleReservationSystem.Domain.VehicleManagement;
using VehicleReservationSystem.WebUI.Models;

namespace VehicleReservationSystem.WebUI.Controllers
{
    public class CitiesController : Controller
    {
        // GET: Cities

            [HttpGet]
        public ActionResult Manage()
        {
            return View(new VehiclesHandlers().GetCities().ToModelList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("~/Views/Cities/_Create.cshtml");

        }

        [HttpPost]
        public ActionResult Create(CityModel model)
        {
            try
            {
                new VehiclesHandlers().AddCity(model.ToEntity());
                TempData.Add("AlertMessage", new AlertModel("The City is added successfully", AlertModel.AlertType.Success));
            }
            catch (Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to add the City", AlertModel.AlertType.Error));
            }
            return RedirectToAction("Manage");
        }


     

        [HttpPost]
        public ActionResult Delete(CityModel model,int id)
        {

            try
            {
                CityModel model1 = new VehiclesHandlers().GetCity(id).ToModel();
                TempData["model1"] = model1.Id;
                new VehiclesHandlers().DeleteCity(model.ToEntity());
                TempData.Add("AlertMessage", new AlertModel("The City is deleted successfully", AlertModel.AlertType.Success));
            }
            catch (Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to delete the City", AlertModel.AlertType.Error));
            }
            return Json(Url.Action($"Manage/{TempData["model1"]}"));
           
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            CityModel dept = new VehiclesHandlers().GetCity(id).ToModel();
            return PartialView("~/Views/Cities/_Edit.cshtml", dept);
        }

        [HttpPost]
        public ActionResult Edit(CityModel model)
        {
            try
            {
                new VehiclesHandlers().UpdateCity(model.ToEntity());
                TempData.Add("AlertMessage", new AlertModel("The City is updated successfully", AlertModel.AlertType.Success));
            }
            catch (Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to update the City", AlertModel.AlertType.Error));
            }
            return RedirectToAction("Manage");
        }



    }
}