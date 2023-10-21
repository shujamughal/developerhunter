using Microsoft.AspNetCore.Mvc;
using Jobverse.Controllers;

namespace Jobverse.Controllers
{
    public class TestNotification
    {
        public string UserEmail { get; set; }
        public string Applier { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public bool Status { get; set; }
    }

    public class NotificationController : Controller
    {
        public IActionResult Notification()
        {
            List<TestNotification> testNotifications = new List<TestNotification>
        {
            new TestNotification
            {
                UserEmail = "user1@example.com",
                Applier = "John Doe",
                Title = "Senior SQ Engineer",
                Time = DateTime.Now,
                Status = true
            },
            new TestNotification
            {
                UserEmail = "user2@example.com",
                Applier = "Jane Smith",
                Title = "Junior Graphic Designer",
                Time = DateTime.Now,
                Status = true
            },
            new TestNotification
            {
                UserEmail = "user1@example.com",
                Applier = "John Doe",
                Title = "Senior SQ Engineer",
                Time = DateTime.Now,
                Status = false
            },
            new TestNotification
            {
                UserEmail = "user2@example.com",
                Applier = "Jane Smith",
                Title = "Junior Graphic Designer",
                Time = DateTime.Now,
                Status = true
            },
        };
            return View(testNotifications);
        }
    }

}
