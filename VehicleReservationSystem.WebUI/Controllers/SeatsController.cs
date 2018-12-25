using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleReservationSystem.Domain.VehicleManagement;
using VehicleReservationSystem.WebUI.Models;

namespace VehicleReservationSystem.WebUI.Controllers
{
    public class SeatsController : Controller
    {
        // GET: Seats
        public ActionResult Manage()
        {
            List<SeatsModel> model = new VehiclesHandlers().GetSeats().ToModelList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Vehicles = new VehiclesHandlers().GetVehicles().ToSelectItemList();
            return PartialView("~/Views/Seats/_Create.cshtml");
        }

        [HttpPost]
        public ActionResult Create(FormCollection data)
        {
            try
            {
                Seat seat = new Seat();
                seat.SeatNumber = Convert.ToInt32(data["Seatno"]);
                seat.Is_Available = Convert.ToBoolean(data["isAvail"]);
                seat.Vehicle = new Vehicle { Id = Convert.ToInt32(data["VehicleName"]) };
                new VehiclesHandlers().AddSeat(seat);

                TempData.Add("AlertMessage", new AlertModel("The Seat is added successfully", AlertModel.AlertType.Success));
            }

            catch (Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to add seat", AlertModel.AlertType.Error));
            }
            return RedirectToAction("Manage");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Seat seat = new VehiclesHandlers().GetSeat(id);
                TempData["seat"] = seat.Vehicle.Id;
                new VehiclesHandlers().DeleteSeat(seat.Id);
                TempData.Add("AlertMessage", new AlertModel("The seat is deleted successfully!", AlertModel.AlertType.Success));

            }

            catch (Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Faild to delete seat!", AlertModel.AlertType.Error));
            }

            return Json(Url.Action($"Manage/{TempData["seat"]}"));
        }

        [HttpGet]

        public ActionResult Edit(int id)
        {
            ViewBag.Vehicles = new VehiclesHandlers().GetVehicles();
            Seat seat = new VehiclesHandlers().GetSeat(id);
            return PartialView("~/Views/Seats/_Edit.cshtml", seat);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection data)
        {
            //try
            //{
               Seat seat = new Seat();
                seat.Id = Convert.ToInt32( data["id"]);
                seat.SeatNumber = Convert.ToInt32( data["Seatno"]);
                seat.Is_Available =Convert.ToBoolean(data["isAvail"]);
                seat.Vehicle = new Vehicle { Id = Convert.ToInt32(data["VehicleName"]) };
                new VehiclesHandlers().UpdateSeat(seat);
                TempData.Add("AlertMessage", new AlertModel("The Seat is updated successfully!", AlertModel.AlertType.Success));
            //}

            //catch (Exception ex)
            //{
            //    TempData.Add("AlertMessage", new AlertModel("Failed to update seat!", AlertModel.AlertType.Error));
            //}
            return RedirectToAction("Manage");
        }
    }

 
}