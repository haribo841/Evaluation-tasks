using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

public class UserController : Controller
{
    private readonly UserXmlService _userXmlService;

    public UserController(UserXmlService userXmlService)
    {
        _userXmlService = userXmlService;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Index()
    {
        List<User> users = _userXmlService.ReadUsersFromXML();
        return View(users);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Protection against CSRF attacks
    public IActionResult Create(User user)
    {
        if (ModelState.IsValid) // Validation check
        {
            _userXmlService.WriteUsersToXML(user);
            return RedirectToAction("Index");
        }

        return View(user); // Displaying a view with validation errors
    }

    [HttpGet]
    [Authorize]
    public IActionResult Edit(int id)
    {
        User user = _userXmlService.GetUserById(id);
        if (user == null)
        {
            return NotFound(); // Handling no user
        }

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(User user)
    {
        if (ModelState.IsValid)
        {
            _userXmlService.UpdateUser(user);
            return RedirectToAction("Index");
        }

        return View(user);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Delete(int id)
    {
        User user = _userXmlService.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _userXmlService.DeleteUser(id);
        return RedirectToAction("Index");
    }
}
