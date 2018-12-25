using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace VehicleReservationSystem.Domain.VehicleManagement
{
    public class VehiclesHandlers
    {


        public class BookingDetails
        {
            public DateTime BookingDate { get; set; }
            public string CustomerName { get; set; }
            public TimeSpan DepartureTime { get; set; }
            public DateTime DepartureDate { get; set; }
            public string RouteName { get; set; }
            public string RegistrationNumber { get; set; }
            public string VehicleName { get; set; }
            public string VehicleTypeName{ get; set; }
            public int SeatNumber{ get; set; }
            public float TotalPrice{ get; set; }
        }
        public class DummyBookingDetails
        {
            public string BookingDate { get; set; }
            public string CustomerName { get; set; }
            public TimeSpan DepartureTime { get; set; }
            public string DepartureDate { get; set; }
            public string RouteName { get; set; }
            public string RegistrationNumber { get; set; }
            public string VehicleName { get; set; }
            public string VehicleTypeName { get; set; }
            public int SeatNumber { get; set; }
            public float TotalPrice { get; set; }
        }

        public List<BookingDetails> GetAll(){
            using (var context = new VehiclesContext()) {

                return context.Database.SqlQuery<BookingDetails>("sp_AllRecords").ToList();
            }
        }

        public List<BookingDetails> GetPrevious()
        {
            using (var context = new VehiclesContext())
            {

                return context.Database.SqlQuery<BookingDetails>("sp_PreviousRecords").ToList();
            }
        }

        public List<BookingDetails> GetUpcoming()
        {
            using (var context = new VehiclesContext())
            {

                return context.Database.SqlQuery<BookingDetails>("sp_UpcomingRecords").ToList();
            }
        }

        public List<BookingDetails> GetCurrent()
        {
            using (var context = new VehiclesContext())
            {

                return context.Database.SqlQuery<BookingDetails>("sp_CurrentRecords").ToList();
            }
        }


        public List<Booking> GetAllNotifications()
        {
            using (var context = new VehiclesContext())
            {
                var notifications = context.BookingDetails
                    .Include(a => a.Seats)
                    .Include(a => a.TimeTables.Route.Origin)
                    .Include(a => a.TimeTables.Route.Destination)
                    .Where(a => a.Is_Admin_Seen == false)
                    .ToList();

                return notifications;


                //var notificationIds = notifications.Select(a => a.Id).ToList();

                //var seatAgainstBooking = context.Seats.Where(a =>notificationIds.Contains(a.Id)).GroupBy(a=>a.Booking).ToList();
            }
        }

        public void AddBookingToSeat(int seatId, int bookingId)
        {
            using (var context = new VehiclesContext())
            {
                var booking = context.Seats.Include(a => a.Vehicle).FirstOrDefault(a => a.Id == seatId);
                booking.Vehicle = new Vehicle { Id = bookingId };
                context.Entry(booking.Vehicle).State = EntityState.Unchanged;
                context.SaveChanges();

            }

        }


        public void SetSeenByAdmin(int id)
        {
            using (var context = new VehiclesContext())
            {
                var booking = context.BookingDetails.FirstOrDefault(a => a.Id == id);
                booking.Is_Admin_Seen = true;
                context.SaveChanges();

            }


        }

        public List<Route> GetRoutes()
        {
            using (var context = new VehiclesContext())
            {

                return (from r in context.Routes
                        .Include(r => r.Origin)
                        .Include(r => r.Destination)

                        select r).ToList();

            }
        }

        public Route GetRoute(int id)
        {
            using (var context = new VehiclesContext())
            {
                return (from r in context.Routes

                         .Include(r => r.Origin)
                         .Include(r => r.Destination)


                        where r.Id == id
                        select r).FirstOrDefault();


            }
        }
        public void AddRoute(Route route)
        {
            using (var context = new VehiclesContext())
            {
                context.Entry(route.Origin).State = System.Data.Entity.EntityState.Unchanged;
                context.Entry(route.Destination).State = System.Data.Entity.EntityState.Unchanged;


                context.Routes.Add(route);
                context.SaveChanges();
            }
        }

        public void UpdateRoute(Route route)
        {
            using (var context = new VehiclesContext())
            {
                Route found = (from r in context.Routes

                          .Include(r => r.Origin)


                               where r.Id == route.Id
                               select r).FirstOrDefault();

                if (found.Origin.Id != route.Origin.Id)
                {
                    found.Origin = route.Origin;
                    context.Entry(found.Origin).State = EntityState.Unchanged;
                }





                found.Price = route.Price;


                found.TravelTime = route.TravelTime;


                context.SaveChanges();
            }

            using (var context1 = new VehiclesContext())
            {
                Route d = (from r in context1.Routes
                          .Include(r => r.Destination)
                           where r.Id == route.Id


                           select r).FirstOrDefault();

                if (d.Destination.Id != route.Destination.Id)
                {
                    d.Destination = route.Destination;
                    context1.Entry(d.Destination).State = EntityState.Unchanged;
                }

                context1.SaveChanges();
            }

        }


        public void DeleteRoute(int id)
        {
            using (var context = new VehiclesContext())
            {

                Route found = context.Routes.Find(id);
                context.Routes.Remove(found);
                context.SaveChanges();

            }
        }

        public List<City> GetCities()
        {
            using (var context = new VehiclesContext())
            {
                return context.Cities.ToList();
            }
        }

        public City GetCity(int id)
        {
            using (var context = new VehiclesContext())
            {
                return context.Cities
                        .Where(c => c.Id == id).FirstOrDefault();
            }
        }

        public void AddCity(City city)
        {
            using (var context = new VehiclesContext())
            {
                context.Cities.Add(city);
                context.SaveChanges();
            }
        }

        public void DeleteCity(City city)
        {
            using (var context = new VehiclesContext())
            {
                City found = context.Cities.Find(city.Id);
                context.Cities.Remove(found);
                context.SaveChanges();
            }
        }

        public void UpdateCity(City city)
        {
            using (var context = new VehiclesContext())
            {
                City temp = context.Cities.Find(city.Id);
                if (!string.IsNullOrWhiteSpace(city.Name))
                {
                    temp.Name = city.Name;
                }
                context.SaveChanges();

            }
        }

        public List<VehicleType> GetVehicleTypes()
        {
            using (var context = new VehiclesContext())
            {
                return context.VehicleTypes.
                    ToList();
            }
        }

        public VehicleType GetVehicleType(int id)
        {
            using (var context = new VehiclesContext())
            {
                return context.VehicleTypes
                    .Where(a => a.Id == id).FirstOrDefault();
            }
        }

        public void AddVehicleType(VehicleType type)
        {
            using (var context = new VehiclesContext())
            {
                context.VehicleTypes.Add(type);
                context.SaveChanges();
            }
        }

        public void DeleteVehicleType(VehicleType type)
        {
            using (var context = new VehiclesContext())
            {
                VehicleType found = context.VehicleTypes.Find(type.Id);
                if (found != null)
                {
                    context.VehicleTypes.Remove(found);
                    context.SaveChanges();
                }
            }
        }

        public void UpdateVehicleType(VehicleType type)
        {
            using (var context = new VehiclesContext())
            {
                VehicleType found = context.VehicleTypes.Find(type.Id);
                if (!string.IsNullOrWhiteSpace(type.Name))
                {
                    found.Name = type.Name;
                }

                context.SaveChanges();
            }
        }

        public List<Vehicle> GetVehicles()
        {
            using (var context = new VehiclesContext())
            {
                return (from v in context.Vehicles
                        .Include(a => a.VehicleType)

                        select v).ToList();


            }
        }

        public Vehicle GetVehicle(int id)
        {
            using (var context = new VehiclesContext())
            {
                return (from c in context.Vehicles.Include(p => p.VehicleType) where c.Id == id select c).FirstOrDefault();
            }
        }
        public void AddVehicle(Vehicle vehicle)
        {
            using (var context = new VehiclesContext())
            {
                context.Entry(vehicle.VehicleType).State = EntityState.Unchanged;
                context.Vehicles.Add(vehicle);
                context.SaveChanges();

            }
        }

        public void DeleteVehicle(int id)
        {
            using (var context = new VehiclesContext())
            {
                Vehicle found = context.Vehicles.Find(id);

                context.Vehicles.Remove(found);
                context.SaveChanges();
            }
        }


        public void UpdateVehicle(Vehicle v)
        {
            using (VehiclesContext context = new VehiclesContext())
            {
                Vehicle find = context.Vehicles.Include(a => a.VehicleType).Where(a => a.Id == v.Id).FirstOrDefault();
                if (find.VehicleType.Id != v.VehicleType.Id)
                {
                    find.VehicleType = v.VehicleType;
                    context.Entry(find.VehicleType).State = EntityState.Unchanged;
                }

                find.Name = v.Name;
                find.RegistrationNumber = v.RegistrationNumber;
                find.ImageUrl = v.ImageUrl;
                find.TotalSeats = v.TotalSeats;



                context.SaveChanges();
            }



        }


        public List<TimeTable> GetSchedules()
        {
            using (var context = new VehiclesContext())
            {
                return context.TimeTables.Include(a => a.Vehicle)
                    .Include(a => a.Route.Origin)
                    .Include(a => a.Route.Destination)
                    .ToList();
            }
        }

        public TimeTable GetSchedule(int id)
        {
            using (var context = new VehiclesContext())
            {
                return context.TimeTables.Include(a => a.Vehicle).Include(a => a.Route).Where(a => a.Id == id).FirstOrDefault();
            }
        }

        public void AddSchedule(TimeTable schedule)
        {
            using (var context = new VehiclesContext())
            {
                context.Entry(schedule.Vehicle).State = EntityState.Unchanged;
                context.Entry(schedule.Route).State = EntityState.Unchanged;

                context.TimeTables.Add(schedule);

                context.SaveChanges();
            }
        }

        public void DeleteSchedule(int id)
        {
            using (var context = new VehiclesContext())
            {
                var find = context.TimeTables.Find(id);
                context.TimeTables.Remove(find);
                context.SaveChanges();
            }
        }

        public void UpdateSchedule(TimeTable tt)
        {
            using (var context = new VehiclesContext())
            {
                var find = context.TimeTables
                     .Include(a => a.Vehicle)
                     .Include(a => a.Route)

                     .Where(a => a.Id == tt.Id).FirstOrDefault();

                if (find.Vehicle.Id != tt.Vehicle.Id)
                {
                    find.Vehicle = tt.Vehicle;
                    context.Entry(find.Vehicle).State = EntityState.Unchanged;
                }
                if (find.Route.Id != tt.Route.Id)
                {
                    find.Route = tt.Route;
                    context.Entry(find.Route).State = EntityState.Unchanged;
                }

                find.DepartureTime = tt.DepartureTime;

                context.SaveChanges();
            }



        }

        public List<Seat> GetSeats()
        {
            using (var context = new VehiclesContext())
            {
                return context.Seats
                    .Include(a => a.Vehicle)
                    .ToList();
            }
        }

        public Seat GetSeat(int id)
        {
            using (var context = new VehiclesContext())
            {
                return context.Seats.Include(a => a.Vehicle).Where(a => a.Id == id).FirstOrDefault();
            }
        }

        public void AddSeat(Seat seat)
        {
            using (var context = new VehiclesContext())
            {
                context.Entry(seat.Vehicle).State = EntityState.Unchanged;

                context.Seats.Add(seat);
                context.SaveChanges();
            }
        }

        public void DeleteSeat(int id)
        {
            using (var context = new VehiclesContext())
            {
                var found = context.Seats
                    .Include(a => a.Vehicle)
                    .Where(a => a.Id == id).FirstOrDefault();
                context.Seats.Remove(found);
                context.SaveChanges();
            }
        }

        public void UpdateSeat(Seat seat)
        {
            using (var context = new VehiclesContext())
            {
                var found = context.Seats.Include(a => a.Vehicle)
                    .Where(a => a.Id == seat.Id).FirstOrDefault();

                if (found.Vehicle.Id != seat.Vehicle.Id)
                {
                    found.Vehicle = seat.Vehicle;
                    context.Entry(found.Vehicle).State = EntityState.Unchanged;
                }

                found.SeatNumber = seat.SeatNumber;
                found.Is_Available = seat.Is_Available;
                context.SaveChanges();
            }
        }

        public List<TimeTable> GetSch()
        {
            using (var context = new VehiclesContext())
            {
                return context.TimeTables

                    .Include(a => a.Route.Origin)
                    .Include(a => a.Route.Destination)
                    .Include(a => a.Route.Price)
                    .Include(a => a.Vehicle.RegistrationNumber)
                    .Include(a => a.Vehicle.VehicleType)
                    .ToList();

            }
        }
        public List<TimeTable> GetScheduleByOrginNdestination(int OriginId, int DestId, DateTime date)
        {
            List<TimeTable> Timetable = new List<TimeTable>();
            using (var context = new VehiclesContext())
            {
                Route R = (from r in context.Routes
                          .Include(r => r.Destination)
                          .Include(r => r.Origin)
                           where r.Origin.Id == OriginId && r.Destination.Id == DestId
                           select r).FirstOrDefault();

                List<TimeTable> t1 = (from t in context.TimeTables
                                    .Include(t => t.Route.Destination)
                                    .Include(t => t.Route.Origin)
                                    .Include(t => t.Vehicle.VehicleType)
                                      where t.Route.Id == R.Id && t.Date == date
                                      select t).ToList();
                if (t1.Count == 0)
                {
                    List<TimeTable> t2 = (from t in context.TimeTables
                                        .Include(t => t.Route.Destination)
                                        .Include(t => t.Route.Origin)
                                        .Include(t => t.Vehicle.VehicleType)
                                          where t.Route.Id == R.Id && t.IsFirst
                                          select t).ToList();
                    List<Vehicle> vehicle = new List<Vehicle>();

                    //List<SeatModel> seatModel = new List<SeatModel>();
                    foreach (var t in t2)
                    {
                        Vehicle temp = (from v in context.Vehicles.Include(v => v.VehicleType) where v.Id == t.Vehicle.Id select v).FirstOrDefault();
                        if (temp != null)
                        {
                            context.Database.ExecuteSqlCommand("insert into Vehicles (RegistrationNumber,Name,ImageUrl, TotalSeats, VehicleType_Id) values ({0},{1},{2},{3},{4})", temp.RegistrationNumber, temp.Name, temp.ImageUrl, temp.TotalSeats, temp.VehicleType.Id);
                            //this.AddVehicle(new Vehicle { RegistrationNumber = temp.RegistrationNumber, Name = temp.Name, ImageUrl = temp.ImageUrl, TotalSeats = temp.TotalSeats, VehicleType = new VehicleType { Id = temp.VehicleType.Id } });
                            //context.Vehicles.Add();
                            Vehicle temp1 = context.Vehicles.OrderByDescending(p => p.Id).FirstOrDefault();
                            List<Seat> seatsTemp = (from s in context.Seats.Include(s => s.Vehicle) where s.Vehicle.Id == temp.Id select s).ToList();
                            if (seatsTemp.Count > 0)
                            {
                                foreach (var s in seatsTemp)
                                {
                                    context.Database.ExecuteSqlCommand("insert into Seats (SeatNumber, Is_Available, Vehicle_Id) values ({0},{1},{2})", s.SeatNumber, 1, temp1.Id);
                                    //this.AddSeat(new Seat { SeatNumber = s.SeatNumber, Is_Available = true, Vehicle = new Vehicle { Id = temp1.Id } });
                                    //context.Entry(s.Vehicle).State = EntityState.Unchanged;
                                    //context.Seats.Add(new Seat { SeatNumber = s.SeatNumber, Is_Available = true, Vehicle = new Vehicle { Id = temp1.Id } });
                                }
                            }
                            context.Database.ExecuteSqlCommand("insert into TimeTables (DepartureTime, Route_Id,Vehicle_Id, Date) values ({0},{1},{2},{3})", t.DepartureTime, t.Route.Id, temp1.Id, date);
                            //context.Entry(t.Route).State = EntityState.Unchanged;
                            //context.Entry(t.Vehicle).State = EntityState.Unchanged;
                            //context.TimeTables.Add();
                        }
                    }
                    Timetable = (from t in context.TimeTables
                                .Include(t => t.Route.Destination)
                                .Include(t => t.Route.Origin)
                                .Include(t => t.Vehicle.VehicleType)
                                 where t.Route.Id == R.Id && t.Date == date
                                 select t).ToList();
                }
                else
                {
                    Timetable = t1;
                }
            }
            return Timetable;
        }

        public List<Seat> GetSeatonVehicleId(int vehicleId)
        {
            using (var context = new VehiclesContext())
            {
                List<Seat> s = context.Seats.Include(a => a.Vehicle)
                  .Where(a => a.Vehicle.Id == vehicleId).ToList();
                return s;


            }
        }


        public int AddBooking(Booking booking)
        {
            try
            {
                using (var context = new VehiclesContext())
                {
                    foreach (var s in booking.Seats)
                    {
                        context.Entry(s).State = EntityState.Unchanged;
                    }
                    context.Entry(booking.TimeTables).State = EntityState.Unchanged;
                    context.Entry(booking).State = EntityState.Added;
                    context.SaveChanges();
                    return booking.Id;
                }
            }
            catch { }
            return 0;
        }

        public int ReserveSeat(int SeatNum, int VehicleId)
        {
            using (var context = new VehiclesContext())
            {
                var found = (from s in context.Seats.Include(s => s.Vehicle) where s.SeatNumber == SeatNum && s.Vehicle.Id == VehicleId select s).FirstOrDefault();
                context.Entry(found.Vehicle).State = EntityState.Unchanged;
                if (found.Is_Available)
                {
                    found.Is_Available = false;
                }

                context.SaveChanges();
                return found.Id;
            }
        }

        public void ResetSeats(int id)
        {
            using (var context = new VehiclesContext())
            {
                var found = (from v in context.Vehicles.Include(V => V.VehicleType) where v.Id == id select v).FirstOrDefault();
                context.Entry(found.VehicleType).State = EntityState.Unchanged;
                for (;;)
                {
                    found.DepartureDateAndTime = found.DepartureDateAndTime.AddDays(1);
                    if (found.DepartureDateAndTime > DateTime.Now)
                        break;
                }
                context.SaveChanges();
            }
            using (var context = new VehiclesContext())
            {
                var found = (from s in context.Seats.Include(s => s.Vehicle) where s.Vehicle.Id == id select s).ToList();
                foreach (var f in found)
                {
                    context.Entry(f.Vehicle).State = EntityState.Unchanged;
                    if (!f.Is_Available)
                    {
                        f.Is_Available = true;
                    }
                }
                context.SaveChanges();
            }
        }

    }
}
