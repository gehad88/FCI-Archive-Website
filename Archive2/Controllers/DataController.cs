using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using modelfor_archive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Archive2.Controllers
{
    public class DataController : Controller
    {
        private readonly Archive2Context _db;

        public DataController(Archive2Context db)
        {
            _db = db;
        }

        /// <summary>
        /// Imports messages sent by the user.
        /// </summary>
        /// <param name="query">Query string for filtering messages.</param>

        /// <returns>View with imported messages.</returns>
        public async Task<IActionResult> Import(string query, string sort)
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (_db.UserAccs.Where(s => s.UserAccID == userId).Any())
            {
                UserAcc Account = _db.UserAccs.Where(s => s.UserAccID == userId && !s.IsAdmin).FirstOrDefault();
                var sentMessages = await _db.ListUserMes
                .Where(l => l.UserId == userId)
                .Include(l => l.Message)
                .ToListAsync();

                List<Messages> messages = new List<Messages>();

                foreach (var item in sentMessages)
                {
                    var message = _db.Message.Where(s => s.MessagesId == item.MessageId).FirstOrDefault();

                    Messages messageSend = new Messages()
                    {
                        MessagesId = item.MessageId,
                        Title = message.Title,
                        UserId = item.UserId,
                        SendDate = item.Message.SendDate,
                        MessageText = message.MessageText,
                        MessageAddress = message.MessageAddress,
                    };
                    messages.Add(messageSend);
                }

                if (!string.IsNullOrEmpty(query))
                {
                    // Check if the query is a valid date
                    if (DateTime.TryParse(query, out DateTime date))
                    {
                        messages = messages.Where(m => m.UserId == userId && m.SendDate.HasValue && m.SendDate.Value.Date == date.Date).ToList();
                    }
                    else
                    {
                        // Treat the query as Title/email
                        messages = messages.Where(m => m.UserId == userId && (m.Title.Contains(query) || m.MessageAddress.Contains(query))).ToList();
                    }
                }
                else
                {
                    messages = messages.Where(m => m.UserId == userId).ToList();
                }
                switch (sort)
                {
                    case "title_asc":
                        messages = messages.OrderBy(m => m.Title).ToList();
                        break;
                    case "title_desc":
                        messages = messages.OrderByDescending(m => m.Title).ToList();
                        break;
                    case "send_date_asc":
                        messages = messages.OrderBy(m => m.SendDate).ToList();
                        break;
                    case "send_date_desc":
                        messages = messages.OrderByDescending(m => m.SendDate).ToList();
                        break;
                    case "email_asc":
                        messages = messages.OrderBy(m => m.MessageAddress).ToList();
                        break;
                    case "email_desc":
                        messages = messages.OrderByDescending(m => m.MessageAddress).ToList();
                        break;
                    default:
                        messages = messages.OrderByDescending(m => m.SendDate).ToList();
                        break;
                }
                ViewBag.messages = messages;
                // Set Cache-Control header to prevent caching
                Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";
                return View(Account);
            }
            else
            {
                return RedirectToAction("Login", "Home");
                //var userId = HttpContext.Session.GetInt32("ID");
            }

        }


        /// <summary>
        /// Exports messages sent by the user.
        /// </summary>
        /// <param name="query">Query string for filtering messages.</param>
        /// <returns>View with exported messages.</returns>
        public IActionResult Export(string query, string sortOption)
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (_db.UserAccs.Where(s => s.UserAccID == userId).Any())
            {
                UserAcc Account = _db.UserAccs.Where(s => s.UserAccID == userId).FirstOrDefault();
                List<Messages> messages = _db.Message.AsNoTracking().ToList();
                if (!string.IsNullOrEmpty(query))
                {
                    // Apply search query if provided
                    if (DateTime.TryParse(query, out DateTime date))
                    {
                        messages = messages.Where(m => m.UserId == userId && m.SendDate.HasValue && m.SendDate.Value.Date == date.Date).ToList();
                    }
                    else
                    {
                        // Treat the query as Title/email
                        messages = messages.Where(m => m.UserId == userId && (m.Title.Contains(query) || m.MessageDestination.Contains(query))).ToList();
                    }
                }
                else
                {
                    messages = messages.Where(m => m.UserId == userId).ToList();
                }
                // Sort messages based on the selected option
                switch (sortOption)
                {
                    case "TitleAsc":
                        messages = messages.OrderBy(m => m.Title).ToList();
                        break;
                    case "TitleDesc":
                        messages = messages.OrderByDescending(m => m.Title).ToList();
                        break;
                    case "date_asc":
                        messages = messages.OrderBy(m => m.SendDate).ToList();
                        break;
                    case "date_desc":
                        messages = messages.OrderByDescending(m => m.SendDate).ToList();
                        break;
                    case "email_asc":
                        messages = messages.OrderBy(m => m.MessageDestination).ToList();
                        break;
                    case "email_desc":
                        messages = messages.OrderByDescending(m => m.MessageDestination).ToList();
                        break;
                    default:
                        // Default sorting by Send Date ascending
                        messages = messages.OrderBy(m => m.SendDate).ToList();
                        break;
                }
                // Set Cache-Control header to prevent caching
                Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";
                ViewBag.messages = messages;
                return View(Account);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }


    }
}
