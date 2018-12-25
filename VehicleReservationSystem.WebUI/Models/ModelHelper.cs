using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleReservationSystem.Domain;
using VehicleReservationSystem.Domain.VehicleManagement;

namespace VehicleReservationSystem.WebUI.Models
{
    public static class ModelHelper
    {

        public static List<SelectListItem> ToSelectItemList(this IEnumerable<IListable> values)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var v in values)
            {
                items.Add(new SelectListItem { Text = v.Name, Value = Convert.ToString(v.Id) });
            }
            return items;
        }


        public static RouteModel ToModel(this Route entity)
        {
            RouteModel model = new RouteModel();
            model.Id = entity.Id;

            model.Price = entity.Price;

            model.OriginName = entity.Origin.Name;
            model.DestName = entity.Destination.Name;
            model.TravelTime = entity.TravelTime;


            return model;
        }

        public static Route ToEntity(this RouteModel model)
        {
            Route entity = new Route();
            entity.Id = model.Id;
            entity.Origin.Name = model.OriginName;
            entity.Destination.Name = model.DestName;
            entity.Price = model.Price;
            entity.TravelTime = model.TravelTime;


            return entity;
        }


        public static List<RouteModel> ToModelList(this List<Route> entities)
        {
            List<RouteModel> models = new List<RouteModel>();
            foreach (var e in entities)
            {
                models.Add(e.ToModel());
            }
            models.TrimExcess();
            return models;
        }



        public static CityModel ToModel(this City entity)
        {
            CityModel model = new CityModel();
            model.Id = entity.Id;
            model.Name = entity.Name;
            return model;
        }

        public static City ToEntity(this CityModel model)
        {
            City entity = new City();
            entity.Id = model.Id;
            entity.Name = model.Name;
            return entity;
        }

        public static List<CityModel> ToModelList(this List<City> entities)
        {
            List<CityModel> models = new List<CityModel>();
            foreach (var e in entities)
            {
                models.Add(e.ToModel());
            }
            models.TrimExcess();
            return models;
        }


        public static VehicleTypeModel ToModel(this VehicleType entity)
        {
            VehicleTypeModel model = new VehicleTypeModel();
            model.Id = entity.Id;
            model.Name = entity.Name;

            return model;
        }

        public static VehicleType ToEntity(this VehicleTypeModel model)
        {
            VehicleType entity = new VehicleType();
            entity.Id = model.Id;
            entity.Name = model.Name;
            return entity;
        }

        public static List<VehicleTypeModel> ToModelList(this List<VehicleType> entities)
        {
            List<VehicleTypeModel> models = new List<VehicleTypeModel>();
            foreach (var e in entities)
            {
                models.Add(e.ToModel());
            }
            models.TrimExcess();
            return models;
        }

        public static VehiclesModel ToModel(this Vehicle entity)
        {
            VehiclesModel model = new VehiclesModel();
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.RegistrationNumber = entity.RegistrationNumber;
            model.ImageUrl = entity.ImageUrl;
            model.VehicleType = entity.VehicleType.Name;
            model.Total_Seats = entity.TotalSeats;

            return model;
        }

        public static Vehicle ToEntity(this VehiclesModel model)
        {
            Vehicle entity = new Vehicle();
            entity.Name = model.Name;
            entity.RegistrationNumber = model.RegistrationNumber;
            entity.ImageUrl = model.ImageUrl;
            entity.TotalSeats = model.Total_Seats;

            return entity;
        }


        public static List<VehiclesModel> ToModelList(this List<Vehicle> entities)
        {
            List<VehiclesModel> models = new List<VehiclesModel>();
            foreach (var e in entities)
            {
                models.Add(e.ToModel());
            }
            models.TrimExcess();
            return models;
        }



        public static SeatsModel ToModel(this Seat entity)
        {
            SeatsModel model = new SeatsModel();
            model.Id = entity.Id;
            model.SeatNumber = entity.SeatNumber;
            model.IsAvail = entity.Is_Available;
            model.VehicleName = entity.Vehicle.Name;


            return model;
        }


        public static List<SeatsModel> ToModelList(this List<Seat> entities)
        {
            List<SeatsModel> models = new List<SeatsModel>();
            foreach (var e in entities)
            {
                models.Add(e.ToModel());
            }
            models.TrimExcess();
            return models;
        }




    }
}