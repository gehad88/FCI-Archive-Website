using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using modelfor_archive.Models;
using modeforview2.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Archive2.Models;

namespace Archive2.Controllers
{
    public class UserController : Controller
    {
        Archive2Context Context;
        private IWebHostEnvironment _environment;

        public UserController(Archive2Context db, IWebHostEnvironment environment)
        {
            Context = db;
            _environment = environment;
        }

        // GET: /User/Index
        public IActionResult Index(UserAcc userAcc)
        {
            return View();
        }

        [HttpPost]
        public JsonResult CheckEmail(string email)
        {
            using (var context = new Archive2Context())
            {
                var user = context.UserAccs.FirstOrDefault(u => u.AcadMail == email);
                bool exists = (user != null);
                return Json(new { exists });
            }
        }

        // GET: /User/addUser
        [HttpGet]
        public IActionResult addUser(UserAcc userAcc)
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (Context.UserAccs.Where(s => s.UserAccID == userId && s.IsAdmin).Any())
            {
                UserAcc Account = Context.UserAccs.Where(s => s.UserAccID == userId).FirstOrDefault();
                return View(Account);
            }
            else
                return RedirectToAction("Login", "Home");
            // return View(userAcc);
        }

        // POST: /User/addUser
        [HttpPost]
        public IActionResult addUserPost(UserAcc userAcc)
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (Context.UserAccs.Where(s => s.UserAccID == userId).Any())
            {
                UserAcc Account = Context.UserAccs.Where(s => s.UserAccID == userId).FirstOrDefault();
                using (var context = new Archive2Context())
                {
                    if (ModelState.IsValid)
                    {
                        var emails = context.UserAccs.Where(s => s.AcadMail == userAcc.AcadMail).AsNoTracking().FirstOrDefault();
                        if (emails != null)
                        {
                            TempData["AlertMessage"] = "An account with the specified email already exists.";
                        }
                        else
                        {
                            var newUserAcc = new UserAcc
                            {
                                UserName = userAcc.UserName,
                                Phone = userAcc.Phone,
                                AcadMail = userAcc.AcadMail,
                                Pass = userAcc.Pass,
                                JobTitle = userAcc.JobTitle,
                                Department = userAcc.Department,
                                Active = true,
                                userGender = userAcc.userGender
                            };

                            context.UserAccs.Add(newUserAcc);
                            context.SaveChanges();

                            TempData["SuccessMessage"] = "User added successfully.";
                            return RedirectToAction("ShowAll", "Home");
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid form submission.";
                        return View("/Views/User/addUser.cshtml");
                    }
                }

                // If the execution reaches this point, it means there was an error
                return RedirectToAction("addUser", userAcc);

                // return View(Account);
            }
            else
                return RedirectToAction("Login", "Home");
        }

        // GET: /User/SendMessage
        [HttpGet]
        public IActionResult SendMessage()
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (Context.UserAccs.Where(s => s.UserAccID == userId && !s.IsAdmin).Any())
            {
                UserAcc Account = Context.UserAccs.Where(s => s.UserAccID == userId).FirstOrDefault();

                var UsersList = Context.UserAccs.Where(s => !s.IsAdmin && s.Active && s.UserAccID != userId).Select(s => s.AcadMail).ToList();

                ViewBag.UsersList = UsersList;
                return View(new Messages());
            }
            else
                return RedirectToAction("Login", "Home");
        }

        // POST: /User/SendMessage
        [HttpPost]
        public async Task<IActionResult> SendMessageAsync(Messages messageData, List<IFormFile> AttFile)
        {
            if (ModelState.IsValid)
            {
                var Id = HttpContext.Session.GetInt32("ID");

                string selectedOptions = Request.Form["MessageDestinationVisible"];
                messageData.SendDate = DateTime.Now;
                messageData.UserId = Id;
                var userEmail = Context.UserAccs.AsNoTracking().Where(ac => ac.UserAccID == Id).FirstOrDefault()?.AcadMail;
                messageData.MessageAddress = userEmail.ToString();
                messageData.MessageDestination = selectedOptions;

                // Save attachments to server and database
                List<Attachment> attachments = new List<Attachment>();
                foreach (var file in AttFile)
                {
                    if (file != null && file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(_environment.WebRootPath, "Attachments", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        attachments.Add(new Attachment { AttFile = fileName, AttDesc = filePath });
                    }
                }

                messageData.Attachments = attachments;

                Context.Message.Add(messageData);
                Context.SaveChanges();


                // Split the string into an array of values
                string[] recipientEmails = selectedOptions.Split(",");
                foreach (var recipientEmail in recipientEmails)
                {
                    var userId = Context.UserAccs.Where(ac => ac.AcadMail.Equals(recipientEmail)).AsNoTracking().FirstOrDefault()?.UserAccID;
                    if (userId != null)
                    {
                        ListUserMes userMesg = new ListUserMes()
                        {
                            UserId = userId.Value,
                            MessageId = messageData.MessagesId
                        };

                        Context.ListUserMes.Add(userMesg);
                    }
                }

                await Context.SaveChangesAsync();

                return RedirectToAction("ViewMessage", new { id = messageData.MessagesId });
            }
            else
            {
                ModelState.AddModelError("", "Failed to save data");
            }

            return View(messageData);
        }

        // GET: /User/ViewMessage
        public IActionResult ViewMessage(int id)
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (userId.HasValue && Context.UserAccs.Any(s => s.UserAccID == userId.Value && !s.IsAdmin))
            {
                var messageDetails = Context.Message.FirstOrDefault(c => c.MessagesId == id);

                if (messageDetails != null)
                {
                    // Set Cache-Control header to prevent caching
                    Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                    Response.Headers["Pragma"] = "no-cache";
                    Response.Headers["Expires"] = "0";

                    return View(messageDetails);
                }
                else
                {
                    return NotFound();
                }
            }

            return RedirectToAction("Login", "Home");
        }

        // GET: /User/editProfile
        [HttpGet]
        public IActionResult editProfile()
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (Context.UserAccs.Where(s => s.UserAccID == userId).Any())
            {
                UserAcc Account = Context.UserAccs.Where(s => s.UserAccID == userId).FirstOrDefault();
                return View(Account);
            }
            else
                return RedirectToAction("Login", "Home");
        }

        // POST: /User/editProfile
        [HttpPost]
        public IActionResult editProfile(UserAcc Account)
        {
            var Id = HttpContext.Session.GetInt32("ID");

            var findAccount = Context.UserAccs.Where(u => u.UserAccID == Id).FirstOrDefault();
            findAccount.UserName = Account.UserName ?? findAccount.UserName;
            findAccount.Phone = Account.Phone ?? findAccount.Phone;
            findAccount.Pass = Account.Pass ?? findAccount.Pass;

            Context.UserAccs.Update(findAccount);
            Context.SaveChanges();

            return RedirectToAction("Profile");
        }

        // GET: /User/editUser
        [HttpGet]
        public IActionResult editUser(int Id)
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (Context.UserAccs.Where(s => s.UserAccID == userId && s.IsAdmin).Any())
            {
                UserAcc Account = Context.UserAccs.Where(s => s.UserAccID == Id).AsNoTracking().FirstOrDefault();
                if (Account == null)
                {
                    return new NotFoundResult();
                }
                return View(Account);
            }
            else
                return RedirectToAction("Login", "Home");

        }

        // POST: /User/editUser
        [HttpPost]
        public IActionResult editUser(UserAcc Account)
        {
            if (ModelState.IsValid)
            {
                Account.Active = true;

                // Load the entity into a new instance of the DbContext
                using (var newContext = new Archive2Context())
                {
                    newContext.UserAccs.Update(Account);
                    newContext.SaveChanges();
                }

                return RedirectToAction("ShowAll", "Home");
            }

            return View(Account);
        }

        // GET: /User/Profile
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (Context.UserAccs.Where(s => s.UserAccID == userId).Any())
            {
                UserAcc Account = Context.UserAccs.Where(s => s.UserAccID == userId).FirstOrDefault();
                var Id = HttpContext.Session.GetInt32("ID");
                UserAcc user = Context.UserAccs.Where(s => s.UserAccID == Id).FirstOrDefault();
                // Set Cache-Control header to prevent caching
                Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";
                return View(user);
            }
            else
                return RedirectToAction("Login", "Home");

        }

        // GET: /User/Delete
        [HttpGet]
        public IActionResult Delete(int ID)
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (Context.UserAccs.Where(s => s.UserAccID == userId && s.IsAdmin).Any())
            {
                UserAcc Account = Context.UserAccs.Where(s => s.UserAccID == userId).FirstOrDefault();
                var user = Context.UserAccs.Where(s => s.UserAccID == ID).AsNoTracking().FirstOrDefault();
                return View(user);
            }
            else
                return RedirectToAction("Login", "Home");

        }

        // POST: /User/Delete
        [HttpPost]
        public IActionResult Delete(UserAcc user)
        {
            var userAccount = Context.UserAccs.Where(s => s.UserAccID == user.UserAccID).AsNoTracking().FirstOrDefault();
            using (var newContext = new Archive2Context())
            {
                userAccount.Active = false;
                Context.Update(userAccount);
                Context.SaveChanges();
            }

            RemoveUser User = new RemoveUser()
            {
                UserName = userAccount.UserName,
                AcadMail = userAccount.AcadMail,
                JobTitle = userAccount.JobTitle,
                Department = userAccount.Department
            };
            Context.RemoveUsers.Add(User);
            Context.SaveChanges();

            return RedirectToAction("ShowAll", "Home");
        }

        // GET: /User/AttachmentView
        [HttpGet]
        public IActionResult AttachmentView(int id)
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (Context.UserAccs.Where(s => s.UserAccID == userId && !s.IsAdmin).Any())
            {
                UserAcc Account = Context.UserAccs.Where(s => s.UserAccID == userId).FirstOrDefault();
                //var Id = HttpContext.Session.GetInt32("MsID");
                List<Attachment> msgAttachments = Context.Attachments.Where(am => am.MessageId == id).ToList();
                ViewBag.MessageAttachments = msgAttachments;
                // Set Cache-Control header to prevent caching
                Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";
                return View();
            }
            else
                return RedirectToAction("Login", "Home");

        }

        [HttpGet]
        public IActionResult ShowAllRemoveUsers(string query)
        {
            var userId = HttpContext.Session.GetInt32("ID");

            if (Context.UserAccs.Where(s => s.UserAccID == userId && s.IsAdmin).Any())
            {
                IQueryable<RemoveUser> allRemovedUsersQuery = Context.RemoveUsers.AsNoTracking();
                if (!string.IsNullOrEmpty(query))
                {
                    allRemovedUsersQuery = allRemovedUsersQuery.Where(s => s.UserName.Contains(query) || s.AcadMail.Contains(query));
                }
                List<RemoveUser> RemoveUsers = allRemovedUsersQuery.OrderBy(m => m.UserName)
                                                .ThenBy(m => m.AcadMail)
                                                .ToList();
                ViewBag.RemoveUsers = RemoveUsers;
                return View();
            } else
                return RedirectToAction("Login", "Home");

        }

        [HttpGet]
        public IActionResult MesgDelUser(string email)
        {
            var userid = Context.UserAccs.Where(e => e.AcadMail.Equals(email)).FirstOrDefault().UserAccID;
            var userId = HttpContext.Session.GetInt32("ID");

            if (Context.UserAccs.Where(s => s.UserAccID == userId && s.IsAdmin).Any())
            {
                UserAcc Account = Context.UserAccs.Where(s => s.UserAccID == userId).FirstOrDefault();
                var DelUserMesg = Context.Message.Where(u => u.UserId == userid).AsNoTracking().ToList();
                Dictionary<int, List<string>> ReciversEmailmesg = new Dictionary<int, List<string>>();
                List<string> Emails = new List<string>();
                foreach (var mesg in DelUserMesg)
                {
                    var recipients = Context.ListUserMes
                                       .Where(lum => lum.MessageId == mesg.MessagesId)
                                       .Include(lum => lum.User)
                                       .Select(lum => lum.User.AcadMail)
                                       .ToList();

                    ReciversEmailmesg[mesg.MessagesId] = recipients;
                }
                ViewBag.Messages = DelUserMesg;
                ViewBag.Recipients = ReciversEmailmesg;
                return View();
            }
            else
                return RedirectToAction("Login", "Home");

        }
    }
}
