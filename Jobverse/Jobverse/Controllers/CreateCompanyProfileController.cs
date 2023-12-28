using Microsoft.AspNetCore.Mvc;

namespace Jobverse.Controllers
{
    public class CreateCompanyProfileController : Controller
    {
        public IActionResult CreateCompanyProfile()
        {
            return View("~/Views/CompanyProfile/CreateCompanyProfile.cshtml");
        }
        public IActionResult SignupEmployer()
        {
            return View("~/Views/CompanyProfile/SignupEmployer.cshtml");
        }
        public IActionResult LoginEmployer()
        {
            return View("~/Views/CompanyProfile/LoginEmployer.cshtml");
        }
    }
}
