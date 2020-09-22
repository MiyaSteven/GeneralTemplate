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


        [HttpGet("dashboard")]
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
                    .Where(w => w.Date > DateTime.Today)
                    .ToList(),
                LoggedUser = DbContext.DbUsers
                    .FirstOrDefault(u => u.UserId == (int)LoggedId)
            };
            return View("Dashboard", WMod);
        }

        [HttpGet("groups/new")]
        public IActionResult NewGroup()
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if (LoggedId == null)
            {
                return RedirectToAction("LogReg");
            }
            return View("NewGroup");
        }

        [HttpPost("groups/create")]
        public IActionResult CreateGroup(Group FromForm)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if (LoggedId == null)
            {
                return RedirectToAction("LogReg");
            }
            // Attach the user who planned the group to the object.
            FromForm.UserId = (int)LoggedId;

            if (ModelState.IsValid)
            {
                if (FromForm.Date < DateTime.Today)
                {
                    ModelState.AddModelError("Date", "No time travel!");
                    return NewGroup();
                }

                DbContext.Add(FromForm);
                DbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return NewGroup();
            }
        }

        [HttpGet("groups/{GroupId}")]
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
                .Include(c => c.Comments)
                // .Include(s => s.SignUpId)
                // .Include(t => t.Date)
                // .Include(t => t.Time)
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

        [HttpPost("groups/{GroupId}/comment")]
        public IActionResult PostComment(int GroupId)
        {
            int? LoggedUser = HttpContext.Session.GetInt32("UserId");
            if (LoggedUser == null)
            {
                return RedirectToAction("LogReg");
            }
            if (ModelState.IsValid)
            {
                string response = Request.Form["CommentText"];
                Comment comment = new Comment();
                comment.GroupId = GroupId;
                comment.UserId = (int)LoggedUser;
                comment.CommentText = response;
                DbContext.Add(comment);
                DbContext.SaveChanges();
                return RedirectToAction("GroupDetail");
            }
            return Dashboard();
        }

        // [HttpPost("/groups/{GroupId}/signup")]
        // public IActionResult SignUp(int GroupId)
        // {
        //     int? LoggedId = HttpContext.Session.GetInt32("UserId");
        //     if (LoggedId == null)
        //     {
        //         return RedirectToAction("LogReg");
        //     }

        //     if (!DbContext.DbGroups.Any(s => s.GroupId == GroupId))
        //     {
        //         return RedirectToAction("Dashboard");
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         string response = Request.Form["SignUpTimeLength"];
        //         SignUp user = new SignUp();
        //         user.GroupId = GroupId;
        //         user.UserId = (int)LoggedId;
        //         user.SignUpTimeLength = response;
        //         DbContext.Add(user);
        //         DbContext.SaveChanges();

        //         return RedirectToAction("GroupDetail");
        //     }
        //     return Dashboard();
        // }

        [HttpGet("groups/{GroupId}/edit")]
        public IActionResult EditGroup(int GroupId)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if (LoggedId == null)
            {
                return RedirectToAction("LogReg");
            }

            Group ToEdit = DbContext.DbGroups.FirstOrDefault(w => w.GroupId == GroupId);

            if (ToEdit == null || ToEdit.UserId != (int)LoggedId)
            {
                return RedirectToAction("Dashboard");
            }

            return View("EditGroup", ToEdit);
        }

        [HttpPost("groups/{GroupId}/update")]
        public IActionResult UpdateGroup(int GroupId, Group FromForm)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if (LoggedId == null)
            {
                return RedirectToAction("LogReg");
            }

            if (!DbContext.DbGroups.Any(w => w.GroupId == GroupId && w.UserId == (int)LoggedId))
            {
                return RedirectToAction("Dashboard");
            }
            FromForm.UserId = (int)LoggedId;
            if (ModelState.IsValid)
            {
                FromForm.GroupId = GroupId;
                DbContext.Update(FromForm);
                DbContext.Entry(FromForm).Property("CreatedAt").IsModified = false;
                DbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return EditGroup(GroupId);
            }
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