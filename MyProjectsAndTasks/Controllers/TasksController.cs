using Microsoft.AspNetCore.Mvc;

namespace MyProjectsAndTasks.Controllers;

public class TasksController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
