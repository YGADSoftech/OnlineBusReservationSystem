using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using VehicleReservationSystem.Domain.UserManagement;
using VehicleReservationSystem.Domain.VehicleManagement;
using VehicleReservationSystem.WebUI.Models;

namespace VehicleReservationSystem.WebUI.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Cities = new VehiclesHandlers().GetCities().ToSelectItemList();
            return View();
        }

        public ActionResult SeenByAdmin(int id)
        {
            new VehiclesHandlers().SetSeenByAdmin(id);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Services()
        {
            return View();
        }
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(FormCollection data)
        {
            try
            {
                Contact c = new Contact();
                c.Name = data["name"];
                c.Email = data["email"];
                c.Subject = data["sub"];
                c.Message = data["msg"];
                new UserHandler().AddContactUser(c);
                TempData.Add("AlertMessage", new AlertModel("Your Information has been successfully send!", AlertModel.AlertType.Success));


            }
            catch (Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Your Information is not sending Please try again!", AlertModel.AlertType.Error));
            }
            return RedirectToAction("Contact");


        }

        [HttpGet]
        public ActionResult Admin()
        {
            var admin = Session[WebUtil.CURRENT_USER] as User;

            if (admin == null)
            {
                return RedirectToAction("login", "users", new { returnUrl = "home/admin" });
            }

            ViewBag.Notifications = new VehiclesHandlers().GetAllNotifications();
            return View();
        }

        [HttpPost]

        public ActionResult GetSchedulebyId(Scheduler s)
        {
            Session.Add("date", s.Date);
            Session.Timeout = 2;
            DateTime date = Convert.ToDateTime(s.Date);
            List<TimeTable> t = new VehiclesHandlers().GetScheduleByOrginNdestination(s.OriginID, s.DestID, date);
            TempData["t"] = t;
            return Json(Url.Action("Index"));

        }
        [HttpGet]

        public ActionResult Seats(int id)
        {
            TimeTable t = new VehiclesHandlers().GetSchedule(id);
            Session["timetable"] = t;
            Session.Timeout = 2;
            //if (t.Vehicle.DepartureDateAndTime < DateTime.Now)
            //{
            //    new VehiclesHandlers().ResetSeats(t.Vehicle.Id);
            //}
            List<Seat> seat = new VehiclesHandlers().GetSeatonVehicleId(t.Vehicle.Id);
            Seats seatModel = new Seats();
            foreach (var s in seat)
            {
                seatModel.seats.Add(new SeatsModel { Id = s.Id, IsAvail = s.Is_Available, SeatNumber = s.SeatNumber, VehicleName = s.Vehicle.Name });
            }
            return View(seatModel);
        }
        
        [HttpPost]

        public ActionResult Seats(string[] arr)
        {


            TempData["arr"] = arr;


            return Json(Url.Action("BookingInfo"));
        }

        [HttpGet]

        public ActionResult BookingInfo()
        {
            return View();
        }


        [HttpPost]

        public ActionResult BookingInfo(FormCollection data)
        {
            //try
            //{
            Booking b = new Booking();
            int seatId = 0;
            b.CustomerName = data["CustomerName"];
            string temp = data["seats"];
            string[] seats = temp.Split(',');
            TimeTable t = Session["timetable"] as TimeTable;
            foreach (var s in seats)
            {
                int id = new VehiclesHandlers().ReserveSeat(Convert.ToInt32(s), t.Vehicle.Id);
                seatId = id;
                b.Seats.Add(new Seat { Id = Convert.ToInt32(id) });
            }
            b.TimeTables = new TimeTable { Id = Convert.ToInt32(data["timetableid"]) };
            b.BookingDateTime = Convert.ToDateTime(data["date"]);
            b.TotalPrice = Convert.ToSingle(data["price"]);
            b.Phone = data["Phoneno"];
            b.email = data["email"];
            b.IdCard = data["CNIC"];
            var bookingId = new VehiclesHandlers().AddBooking(b);
       
            //SmtpClient smtpclt = new SmtpClient("smtp.gmail.com", 587);
            //smtpclt.EnableSsl = true;
            //smtpclt.Credentials = new NetworkCredential("junaidb966@gmail.com", "235686861212");
            //smtpclt.Send("junaidb966@gmail.com", b.email, "Welcome Message", $"Dear {b.CustomerName} your seat are booked on Travello Daewoo service.Your seat number are {temp}");

            TempData.Add("AlertMessage", new AlertModel("Booking has been reserved", AlertModel.AlertType.Success));

            //    catch
            //    {
            //        TempData.Add("AlertMessage", new AlertModel("Failed to reserved booking", AlertModel.AlertType.Error));
            //    }
            return RedirectToAction("Index");

        }







        public ActionResult GetReport(string scope)
        {

            Microsoft.Reporting.WebForms.Warning[] warnings;
            string mimeType = "";
            string[] streamids;
            string encoding;
            string filenameExtension;
            byte[] bytes = null;

            var viewer = new ReportViewer();


            // ReportParameter[] parms = new ReportParameter[24];
            //parms[8] = new ReportParameter("cashprize", chash);

            if (scope == "all")
            {
                var path = Path.Combine(Server.MapPath("~/Reports"), "AllReport.rdlc");
                viewer.LocalReport.ReportPath = path;

                var result = new VehiclesHandlers().GetAll();
                List<VehiclesHandlers.DummyBookingDetails> list = new List<VehiclesHandlers.DummyBookingDetails>();
                foreach (var item in result)
                {
                    var i = new VehiclesHandlers.DummyBookingDetails();
                    i.BookingDate = item.BookingDate.ToShortDateString();
                    i.CustomerName = item.CustomerName;
                    i.DepartureDate = item.DepartureDate.ToShortDateString();
                    i.DepartureTime = item.DepartureTime;
                    i.RegistrationNumber = item.RegistrationNumber;
                    i.RouteName = item.RouteName;
                    i.SeatNumber = item.SeatNumber;
                    i.TotalPrice = item.TotalPrice;
                    i.VehicleName = item.VehicleName;
                    i.VehicleTypeName = item.VehicleTypeName;

                    list.Add(i);
                }
                viewer.LocalReport.DataSources.Add(new ReportDataSource("All", list));
            }
            else if (scope == "upcoming")
            {
                var path = Path.Combine(Server.MapPath("~/Reports"), "AllUpcoming.rdlc");
                viewer.LocalReport.ReportPath = path;

                var result = new VehiclesHandlers().GetUpcoming();
                List<VehiclesHandlers.DummyBookingDetails> list = new List<VehiclesHandlers.DummyBookingDetails>();
                foreach (var item in result)
                {
                    var i = new VehiclesHandlers.DummyBookingDetails();
                    i.BookingDate = item.BookingDate.ToShortDateString();
                    i.CustomerName = item.CustomerName;
                    i.DepartureDate = item.DepartureDate.ToShortDateString();
                    i.DepartureTime = item.DepartureTime;
                    i.RegistrationNumber = item.RegistrationNumber;
                    i.RouteName = item.RouteName;
                    i.SeatNumber = item.SeatNumber;
                    i.TotalPrice = item.TotalPrice;
                    i.VehicleName = item.VehicleName;
                    i.VehicleTypeName = item.VehicleTypeName;
                    list.Add(i);
                }
                viewer.LocalReport.DataSources.Add(new ReportDataSource("Upcoming", list));
            }
            else if (scope == "previous")
            {
                var path = Path.Combine(Server.MapPath("~/Reports"), "AllPrevious.rdlc");
                viewer.LocalReport.ReportPath = path;

                var result = new VehiclesHandlers().GetPrevious();
                List<VehiclesHandlers.DummyBookingDetails> list = new List<VehiclesHandlers.DummyBookingDetails>();
                foreach (var item in result)
                {
                    var i = new VehiclesHandlers.DummyBookingDetails();
                    i.BookingDate = item.BookingDate.ToShortDateString();
                    i.CustomerName = item.CustomerName;
                    i.DepartureDate = item.DepartureDate.ToShortDateString();
                    i.DepartureTime = item.DepartureTime;
                    i.RegistrationNumber = item.RegistrationNumber;
                    i.RouteName = item.RouteName;
                    i.SeatNumber = item.SeatNumber;
                    i.TotalPrice = item.TotalPrice;
                    i.VehicleName = item.VehicleName;
                    i.VehicleTypeName = item.VehicleTypeName;
                    list.Add(i);
                }
                viewer.LocalReport.DataSources.Add(new ReportDataSource("Previous", list));
            }
            else if (scope == "current")
            {
                var path = Path.Combine(Server.MapPath("~/Reports"), "Current.rdlc");
                viewer.LocalReport.ReportPath = path;

                var result = new VehiclesHandlers().GetCurrent();
                List<VehiclesHandlers.DummyBookingDetails> list = new List<VehiclesHandlers.DummyBookingDetails>();
                foreach (var item in result)
                {
                    var i = new VehiclesHandlers.DummyBookingDetails();
                    i.BookingDate = item.BookingDate.ToShortDateString();
                    i.CustomerName = item.CustomerName;
                    i.DepartureDate = item.DepartureDate.ToShortDateString();
                    i.DepartureTime = item.DepartureTime;
                    i.RegistrationNumber = item.RegistrationNumber;
                    i.RouteName = item.RouteName;
                    i.SeatNumber = item.SeatNumber;
                    i.TotalPrice = item.TotalPrice;
                    i.VehicleName = item.VehicleName;
                    i.VehicleTypeName = item.VehicleTypeName;
                    list.Add(i);
                }
                viewer.LocalReport.DataSources.Add(new ReportDataSource("PreviousAppointments", list));
            }




            // viewer.LocalReport.SetParameters(parms);
            bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);



            return File(bytes, mimeType);


        }






        //[HttpGet]
        //public ActionResult GetSchedule(Scheduler s)
        //{
        //    List<TimeTable> t = new VehiclesHandlers().GetRouteByOrginNdestination(s.OriginID, s.DestID);
        //    TempData["t"] = t;
        //    return Json(Url.Action("GetSchedulebyId"));
        //}

        //[HttpPost]
        //public ActionResult GetSchedulebyId(FormCollection data)
        //{
        //    int OriginID = Convert.ToInt32(data["o"]);
        //    int DestID = Convert.ToInt32(data["d"]);
        //    List<TimeTable> t = new VehiclesHandlers().GetRouteByOrginNdestination(OriginID, DestID);
        //    return PartialView("~/Views/Home/_GetSchedulebyId.cshtml", t);
        //}
    }
}