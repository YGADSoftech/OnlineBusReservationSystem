using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml.Linq;
using VehicleReservationSystem.Domain.UserManagement;
using VehicleReservationSystem.WebUI.Models;
using VehicleReservationSystem.WebUI.Models.Users;

namespace VehicleReservationSystem.WebUI.Controllers
{
    public class UsersController : Controller
    {

        //public ActionResult Pdf()
        //{
        //    List<User> user = new UserHandler().GetUsers();

        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachement;filename=print.pdf");
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);
        //    StringReader sr = new StringReader(sw.ToString());
        //    XDocument doc=new XDocument(PageSize.A4)

        //}

        [HttpGet]
        public ActionResult Login()
        {
            User user = Session[WebUtil.CURRENT_USER] as User;
            if (user != null)
            {
                if (WebUtil.ADMIN_ROLE == 1)
                {
                    return RedirectToAction("Admin", "Home");
                }

                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            string ReturnUrl = Request.QueryString["returnUrl"];
            ViewBag.ReturnUrl = ReturnUrl;
            HttpCookie myCookie = Request.Cookies[WebUtil.MY_COOKIE];
            if (myCookie != null)
            {
                string[] data = myCookie.Value.Split(',');
                User u = new UserHandler().GetUserforLoginId(data[0], data[1]);
                if(u!=null)
                {
                    myCookie.Expires = DateTime.Today.AddDays(7);
                    Response.SetCookie(myCookie);
                    Session.Add(WebUtil.CURRENT_USER, u);
                    string[] parts = null;
                    if (!string.IsNullOrWhiteSpace(ReturnUrl))
                    {
                        parts = ReturnUrl.Split('/');
                        if (parts.Length == 2) return RedirectToAction(parts[0], parts[1]);
                    }
                    if (WebUtil.ADMIN_ROLE==1) 
                    {
                        return RedirectToAction("Admin", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

            }

            return View();

        }
        

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                
                return View ("Login", model);
            }

            User us = Session[WebUtil.CURRENT_USER] as User;
            if (us != null)
            {
                if (WebUtil.ADMIN_ROLE==1)
                {
                    return RedirectToAction("Admin", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            User user = new UserHandler().GetUserforLoginId(model.LoginId, model.Password);
            if (user != null)
            {
                Session.Add(WebUtil.CURRENT_USER, user);
                if (model.RememberMe)
                {
                    HttpCookie myCookie = new HttpCookie(WebUtil.MY_COOKIE);
                    myCookie.Expires = DateTime.Today.AddDays(7);
                    myCookie.Value = $"{model.LoginId},{model.Password}";
                    Response.SetCookie(myCookie);
                }
                string returnURL = Request.QueryString["rurl"];
                string[] parts = null;
                if (!string.IsNullOrWhiteSpace(returnURL))
                {
                    parts = returnURL.Split('/');
                    if (parts.Length == 2) return RedirectToAction(parts[1], parts[0]);
                }

                if (user.Email==model.LoginId & user.Password==model.Password)

                {
                    return RedirectToAction("Admin", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            else
            {
                TempData.Add("AlertMessage", new AlertModel("Incorrect LoginId or Password... Please try again...",AlertModel.AlertType.Error));
                return RedirectToAction("Login");
            }

           
        }

        public ActionResult Logout()
        {
            User user = Session[WebUtil.CURRENT_USER] as User;
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            HttpCookie cookie = Request.Cookies[WebUtil.MY_COOKIE];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now;
                Response.SetCookie(cookie);
            }
            Session.Remove(WebUtil.CURRENT_USER);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
      
        public ActionResult SignUp(FormCollection data)
        {
            User us = Session[WebUtil.CURRENT_USER] as User;
            if (us != null)
            {
                if (WebUtil.ADMIN_ROLE==1) 
                {
                    return RedirectToAction("Admin","Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            User u = new UserHandler().GetUserforEmail(data["email"]);
            if (u == null) //for checking whether this email is already registered or not
            {
               try
               {
                    User user = new User();
                    user.FistName = data["FirstName"];
                    user.LastName = data["LastName"];
                    user.LoginId = data["username"];
                    user.Email = data["email"];
                    user.Password = data["Password"];
                    user.Password = data["ReTypePass"];
                    user.PhoneNumber = data["PhoneNumber"];
                    user.Is_Active = Convert.ToBoolean(data["isActive"]);
                    new UserHandler().AddUser(user);
                    SmtpClient smtpclt = new SmtpClient("smtp.gmail.com", 587);
                    smtpclt.EnableSsl = true;
                    smtpclt.Credentials = new NetworkCredential("junaidb966@gmail.com", "235686861212");
                    smtpclt.Send("junaidb966@gmail.com", user.Email, "Welcome Message", $"Dear {user.FistName} {user.LastName} WElcome to the Online bus reservation system");

                    TempData.Add("AlertMessage", new AlertModel("You are a  Register User!", AlertModel.AlertType.Success));

               }

               catch 
               {
                    TempData.Add("AlertMessage", new AlertModel("Unable to register you. Please try again", AlertModel.AlertType.Error));

               }
            }

            else
            {
                TempData.Add("ALertMessage", new AlertModel("You already have an account!", AlertModel.AlertType.Information));
            }

            return RedirectToAction("SignUp");

        }


        [HttpGet]

        public ActionResult AccountSetting()
        {
            User user = Session[WebUtil.CURRENT_USER] as User;
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            return View();

        }

        [HttpPost]

        public ActionResult AccountSetting(FormCollection data)
        {

            User currentUser = Session[WebUtil.CURRENT_USER] as User;
            if (currentUser == null)
            {
                return RedirectToAction("Login");
            }
            try
            {
                User u = new User();
                u.Id = Convert.ToInt32(data["id"]);
                u.FistName = data["FirstName"];
                u.LastName = data["LastName"];
                u.PhoneNumber = data["phoneno"];
                new UserHandler().UpdateAccount(u);
                TempData.Add("AlertMessage", new AlertModel("The User is updated successfully!", AlertModel.AlertType.Success));
            }
            catch
            {
                TempData.Add("AlertMessage", new AlertModel("Faild to update user! Please try again", AlertModel.AlertType.Error));
            }

            return RedirectToAction("AccountSetting");
        }
        public ActionResult RecoverPassword()
        {
            return View();
        }


        [HttpGet]

        public ActionResult Manage()
        {
            List<User> user = new UserHandler().GetUsers();
            return View(user);
        }

        [HttpGet]

        public ActionResult Create()
        {
            return PartialView("~/Views/Users/_Create.cshtml");
        }

        [HttpPost]

        public ActionResult Create(FormCollection data)
        {
            try
            {
                User user = new User();
                user.FistName = data["FirstName"];
                user.LastName = data["LastName"];
                user.Email = data["Email"];
                user.LoginId = data["UserName"];
                user.Password = data["Password"];
                user.PhoneNumber = data["phoneno"];
                user.Is_Active = Convert.ToBoolean(data["isActive"]);
                new UserHandler().AddUser(user);
                TempData.Add("AlertMessage", new AlertModel("The User is added Successfully!", AlertModel.AlertType.Success));

            }

            catch (Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to add User!", AlertModel.AlertType.Success));

            }
            return RedirectToAction("Manage");
        }


        [HttpPost]

        public ActionResult Delete(int id)
        {
            try
            {
                User user = new UserHandler().GetUser(id);
                new UserHandler().DeleteUser(user.Id);
                TempData.Add("AlertMessage", new AlertModel("The user is deleted successfully!", AlertModel.AlertType.Success));
            }

            catch (Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to delete user!", AlertModel.AlertType.Error));
            }

            return Json(Url.Action("Manage"));
        }


        [HttpGet]

        public ActionResult Edit(int id)
        {
            User user = new UserHandler().GetUser(id);
            return PartialView("~/Views/Users/_Edit.cshtml", user);
        }



        [HttpPost]

        public ActionResult Edit(FormCollection data)
        {
            try
            {
                User user = new User();
                user.Id = Convert.ToInt32(data["id"]);
                user.FistName = data["FirstName"];
                user.LastName = data["LastName"];
                user.Email = data["Email"];
                user.LoginId = data["UserName"];
                user.Password = data["Password"];
                user.PhoneNumber = data["phoneno"];
                user.Is_Active = Convert.ToBoolean(data["isActive"]);
                new UserHandler().UpdateUser(user);
                TempData.Add("AlertMessage", new AlertModel("The User is Updated Successfully!", AlertModel.AlertType.Success));

            }

            catch (Exception ex)
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to Update User!", AlertModel.AlertType.Success));

            }
            return RedirectToAction("Manage");
        }


        [HttpGet]

        public ActionResult ChangePassword()
        {
            User currentUser = Session[WebUtil.CURRENT_USER] as User;
            if (currentUser == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection data)
        {

            User currentUser = Session[WebUtil.CURRENT_USER] as User;

            if (currentUser == null)
            {
                return RedirectToAction("Login");
            }
            try
            {


                if (data["CurrentPassword"] != currentUser.Password)
                {
                    TempData.Add("AlertMessage", new AlertModel("You entered wrong password..! Please try again...",AlertModel.AlertType.Error));
                    return RedirectToAction("ChangePassword");
                }
                if (data["NewPassword"] != data["Confirm"])
                {
                    TempData.Add("alert", new AlertModel("Some error occurred..! Please try again...", AlertModel.AlertType.Error));
                    return RedirectToAction("ChangePassword");
                }

                new UserHandler().ChangePassword(data["NewPassword"], currentUser.Id);
                TempData.Add("AlertMessage", new AlertModel("The password is updated successfully!", AlertModel.AlertType.Success));
        }

            catch
            {
                TempData.Add("AlertMessage", new AlertModel("Failed to change your password..! Please try again...", AlertModel.AlertType.Error));
                
            }
            return RedirectToAction("ChangePassword");
        }


        [HttpGet]

        public ActionResult ForgotPassword(LoginModel model)
        {
            User currentuser = Session[WebUtil.CURRENT_USER] as User;
            if (currentuser!=null)
            {
                if(WebUtil.ADMIN_ROLE==1)
                {
                    return RedirectToAction("Admin", "Home");
                }

                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            try
            {
                User user = new UserHandler().ForgotPassword(model.LoginId);
                SmtpClient smtpclt = new SmtpClient("smtp.gmail.com", 587);
                smtpclt.EnableSsl = true;
                smtpclt.Credentials = new NetworkCredential("junaidb966@gmail.com", "235686861212");
                smtpclt.Send("junaidb966@gmail.com", user.Email ,"Password", $"Dear {user.FistName} {user.LastName}.Your Password is,{user.Password}");
        }

            catch
            {
                return RedirectToAction("Login");
    }

            return RedirectToAction("Login");
        }
    }
}