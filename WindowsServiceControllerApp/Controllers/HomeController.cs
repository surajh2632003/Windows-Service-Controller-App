using Microsoft.AspNetCore.Mvc;
using System.ServiceProcess;

namespace YourNamespace.Controllers
{
    public class HomeController : Controller
    {
        private string serviceName = "Worker Service Example";  // Replace with your actual service name

        [HttpPost]
        public IActionResult StartService()
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running);
                    }
                }
                ViewBag.Message = "Service Started Successfully.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult StopService()
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    if (sc.Status == ServiceControllerStatus.Running)
                    {
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped);
                    }
                }
                ViewBag.Message = "Service Stopped Successfully.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        // Method to check service status and return partial view
        [HttpPost]
        public IActionResult CheckServiceStatus()
        {
            string statusMessage;
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    statusMessage = sc.Status.ToString();  // Get the status of the service
                }
            }
            catch (Exception ex)
            {
                statusMessage = $"Error checking service status: {ex.Message}";
            }
            return PartialView("_ServiceStatusPartial", statusMessage);  // Return partial view
        }

        public IActionResult Index()
        {
            using (ServiceController sc = new ServiceController(serviceName))
            {
                ViewBag.ServiceStatus = sc.Status.ToString();
            }
            return View();
        }
    }
}
