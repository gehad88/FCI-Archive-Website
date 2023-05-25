using Archive2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using modelfor_archive.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Archive2.Controllers
{
    public class HomeController : Controller
    {
        private readonly Archive2Context _db;

        public HomeController(Archive2Context db)
        {
            _db = db;
        }

        /// <summary>
        /// Displays the home page for a regular user.
        /// </summary>
        /// <returns>View with user account information.</returns>
        public IActionResult UserHome()
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (_db.UserAccs.Where(s => s.UserAccID == userId && !s.IsAdmin).Any())
            {
                //UserAcc Account = _db.UserAccs.Where(s => s.UserAccID == userId).FirstOrDefault();

                UserAcc account = _db.UserAccs.Where(s => s.UserAccID == userId).FirstOrDefault();

                if (account == null)
                {
                    return new NotFoundResult();
                }
                // Set Cache-Control header to prevent caching
                Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";
                return View(account);

            }
            else
                return RedirectToAction("Login", "Home");

        }

        /// <summary>
        /// Displays the home page for an admin user.
        /// </summary>
        /// <returns>View with admin account information.</returns>
        public IActionResult AdminHome()
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (_db.UserAccs.Where(s => s.UserAccID == userId && s.IsAdmin).Any())
            {
                UserAcc Account = _db.UserAccs.Where(s => s.UserAccID == userId).FirstOrDefault();

                if (Account == null)
                {
                    return new NotFoundResult();
                }
                Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";

                return View(Account);
            }
            else
                return RedirectToAction("Login", "Home");


        }

        [HttpGet]
        public IActionResult Login()
        {
            HttpContext.Session.Clear(); // Clear the session data
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login login, bool loginAsAdmin)
        {
            string email = login.Email;
            string password = login.Password;

            UserAcc user = _db.UserAccs.Where(s => s.AcadMail == email && s.Pass == password && s.Active).FirstOrDefault();

            if (user == null || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Please enter a valid email and password.";
                return View();
            }

            HttpContext.Session.SetInt32("ID", user.UserAccID);

            if (loginAsAdmin && !user.IsAdmin)
            {
                ViewBag.ErrorMessage = "You are not authorized to login as an admin.";
                return View();
            }
            else if (loginAsAdmin && user.IsAdmin)
            {
                return RedirectToAction("AdminHome", "Home");
            }
            else if (!loginAsAdmin && user.IsAdmin)
            {
                return RedirectToAction("AdminHome", "Home");
            }
            else
            {
                return RedirectToAction("UserHome", "Home");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Displays the help page for the current user.
        /// </summary>
        /// <returns>View with user account information.</returns>
        public IActionResult Help()
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (_db.UserAccs.Where(s => s.UserAccID == userId).Any())
            {
                //UserAcc Account = _db.UserAccs.Where(s => s.UserAccID == userId).FirstOrDefault();

                UserAcc account = _db.UserAccs.Where(s => s.UserAccID == userId).FirstOrDefault();

                if (account == null)
                {
                    return new NotFoundResult();
                }
                // Set Cache-Control header to prevent caching
                Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";
                return View(account);

            }
            else
                return RedirectToAction("Login", "Home");

        }

        /// <summary>
        /// Displays a page with all non-admin active users.
        /// </summary>
        /// <param name="query">Search query for filtering users by name or email.</param>
        /// <returns>View with a list of users.</returns>
        public IActionResult ShowAll(string query)
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (_db.UserAccs.Where(s => s.UserAccID == userId && s.IsAdmin).Any())
            {
                UserAcc Account = _db.UserAccs.Where(s => s.UserAccID == userId).FirstOrDefault();

                IQueryable<UserAcc> allUsersQuery = _db.UserAccs.AsNoTracking().Where(s => s.IsAdmin == false && s.Active);

                if (!string.IsNullOrEmpty(query))
                {
                    allUsersQuery = allUsersQuery.Where(s => s.UserName.Contains(query) || s.AcadMail.Contains(query));
                }

                List<UserAcc> allUsers = allUsersQuery.OrderBy(m => m.UserName)
                                                       .ThenBy(m => m.AcadMail)
                                                       .ToList();

                ViewBag.AllUsers = allUsers;
                // Set Cache-Control header to prevent caching
                Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";
                return View();
                //return View(Account);
            }
            else
                return RedirectToAction("Login", "Home");

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Renders the admin layout view.
        /// </summary>
        /// <returns>View with admin layout.</returns>
        public IActionResult _AdminLayout()
        {
            var userId = HttpContext.Session.GetInt32("ID");
            UserAcc account = _db.UserAccs.Where(s => s.UserAccID == userId && s.IsAdmin).FirstOrDefault();

            if (account == null)
            {
                return new NotFoundResult();
            }

            Gender userGender = account.userGender;

            ViewBag.UserGender = userGender.ToString();

            return View("_AdminLayout");
        }

        /// <summary>
        /// Renders the user layout view.
        /// </summary>
        /// <returns>View with user layout.</returns>
        public IActionResult _UserLayout()
        {
            var userId = HttpContext.Session.GetInt32("ID");
            UserAcc account = _db.UserAccs.Where(s => s.UserAccID == userId && !s.IsAdmin).FirstOrDefault();

            if (account == null)
            {
                return new NotFoundResult();
            }

            Gender userGender = account.userGender;

            ViewBag.UserGender = userGender.ToString();

            return View();
        }
    }
}
