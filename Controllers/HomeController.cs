using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using GeneralTemplate.Models;

namespace GeneralTemplate.Controllers
{
    public class HomeController : Controller
    {
        private MyContext DbContext;

        public HomeController(MyContext context)
        {
            DbContext = context;
        }

        [HttpGet("")]
        public ViewResult LogReg()
        {
            return View("LogReg");
        }

        [HttpPost("users/register")]
        public IActionResult Register(LogRegWrapper FromForm)
        {
            if (ModelState.IsValid)
            {
                // Unique validation
                if (DbContext.DbUsers.Any(u => u.Email == FromForm.Register.Email))
                {
                    ModelState.AddModelError("Register.Email", "Already registered? Please Log In.");
                    return LogReg();
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                FromForm.Register.Password = Hasher.HashPassword(FromForm.Register, FromForm.Register.Password);

                DbContext.Add(FromForm.Register);
                DbContext.SaveChanges();


                HttpContext.Session.SetInt32("UserId", FromForm.Register.UserId);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return LogReg();
            }
        }

        [HttpPost("users/login")]
        public IActionResult Login(LogRegWrapper FromForm)
        {
            if (ModelState.IsValid)
            {
                User InDb = DbContext.DbUsers.FirstOrDefault(u => u.Email == FromForm.Login.Email);

                if (InDb == null)
                {
                    ModelState.AddModelError("Login.Email", "Invalid email/password");
                    return LogReg();
                }

                PasswordHasher<LogUser> Hasher = new PasswordHasher<LogUser>();
                PasswordVerificationResult Result = Hasher.VerifyHashedPassword(FromForm.Login, InDb.Password, FromForm.Login.Password);

                if (Result == 0)
                {
                    ModelState.AddModelError("Login.Email", "Invalid email/password");
                    return LogReg();
                }
                HttpContext.Session.SetInt32("UserId", InDb.UserId);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return LogReg();
            }
        }

        [HttpGet("logout")]
        public RedirectToActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LogReg");
        }


        [HttpGet("bright_ideas")]
        public IActionResult Dashboard()
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if (LoggedId == null)
            {
                return RedirectToAction("LogReg");
            }

            GroupWrapper WMod = new GroupWrapper()
            {
                AllGroups = DbContext.DbGroups
                    .Include(w => w.Planner)
                    .Include(w => w.GuestsAttending)
                    .ThenInclude(r => r.Guest)
                    .ToList(),
                LoggedUser = DbContext.DbUsers
                    .FirstOrDefault(u => u.UserId == (int)LoggedId)
            };
            return View("Dashboard", WMod);
        }

        [HttpPost("create")]
        public IActionResult CreateGroup(int id)
        {
            int? LoggedUser = HttpContext.Session.GetInt32("UserId");
            if (LoggedUser == null)
            {
                return RedirectToAction("LogReg");
            }
            if (ModelState.IsValid)
            {
                string response = Request.Form["GroupName"];
                Group message = new Group();
                message.UserId = (int)LoggedUser;
                message.GroupId = id;
                message.GroupName = response;
                DbContext.Add(message);
                DbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return Dashboard();
        }

        [HttpGet("users/{id}")]
        public IActionResult UserDetail(int? id)
        {
            GroupWrapper WMod = new GroupWrapper();
            {
                WMod.AllGroups = DbContext.DbGroups
                    .Include(w => w.Planner)
                    .ThenInclude(u => u.ExistingGroups)
                    .Include(w => w.GuestsAttending)
                    .ThenInclude(r => r.Guest)
                    .ToList();
                WMod.LoggedUser = DbContext.DbUsers
                    .FirstOrDefault(w => w.UserId == id);
            }

            return View("UserDetail", WMod);
        }

        [HttpGet("bright_ideas/{GroupId}")]
        public IActionResult GroupDetail(int GroupId)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if (LoggedId == null)
            {
                return RedirectToAction("LogReg");
            }

            GroupWrapper GMod = new GroupWrapper();

            GMod.AllGroups = DbContext.DbGroups
                .Include(w => w.Planner)
                .Include(w => w.GuestsAttending)
                .ThenInclude(r => r.Guest)
                .Where(w => w.GroupId == GroupId)
                .ToList();

            GMod.AllUsers = DbContext.DbUsers
                .ToList();

            if (GMod == null)
            {
                return RedirectToAction("Dashboard");
            }
            return View("GroupDetail", GMod);
        }

        [HttpGet("groups/{GroupId}/rsvp")]
        public RedirectToActionResult RSVP(int GroupId)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if (LoggedId == null)
            {
                return RedirectToAction("LogReg");
            }
            Group ToJoin = DbContext.DbGroups
                .Include(w => w.GuestsAttending)
                .FirstOrDefault(w => w.GroupId == GroupId);

            if (ToJoin == null || ToJoin.UserId == (int)LoggedId || ToJoin.GuestsAttending.Any(r => r.UserId == (int)LoggedId))
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                RSVP NewRsvp = new RSVP()
                {
                    UserId = (int)LoggedId,
                    GroupId = GroupId
                };
                DbContext.Add(NewRsvp);
                DbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
        }

        [HttpGet("groups/{GroupId}/unrsvp")]
        public RedirectToActionResult UnRSVP(int GroupId)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if (LoggedId == null)
            {
                return RedirectToAction("LogReg");
            }
            Group ToLeave = DbContext.DbGroups
                .Include(w => w.GuestsAttending)
                .FirstOrDefault(w => w.GroupId == GroupId);

            if (ToLeave == null || !ToLeave.GuestsAttending.Any(r => r.UserId == (int)LoggedId))
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                RSVP ToRemove = DbContext.DbRSVPs.FirstOrDefault(r => r.UserId == (int)LoggedId && r.GroupId == GroupId);
                DbContext.Remove(ToRemove);
                DbContext.SaveChanges();

                return RedirectToAction("Dashboard");
            }
        }

        [HttpGet("groups/{GroupId}/delete")]
        public RedirectToActionResult DeleteGroup(int GroupId)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if (LoggedId == null)
            {
                return RedirectToAction("LogReg");
            }

            Group ToDelete = DbContext.DbGroups
                .FirstOrDefault(w => w.GroupId == GroupId);

            if (ToDelete == null || ToDelete.UserId != (int)LoggedId)
            {
                return RedirectToAction("Dashboard");
            }

            DbContext.Remove(ToDelete);
            DbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}
