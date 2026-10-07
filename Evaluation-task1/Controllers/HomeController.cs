using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Diagnostics;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserXmlService _userXmlService;
        private readonly ILogger<HomeController> _logger;
        public HomeController(UserXmlService userXmlService, ILogger<HomeController> logger)
        {
            _userXmlService = userXmlService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Injected service instead of createing a new UserXmlService instance
            var users = _userXmlService.ReadUsersFromXML();
            return View(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                // Adding to XML via DI
                _userXmlService.WriteUsersToXML(user);

                return RedirectToAction("Index");
            }
            // If ModelState.IsValid == false
            var birthDateError = ModelState["BirthDate"];
            if (birthDateError != null && birthDateError.Errors.Count > 0)
            {
                birthDateError.Errors.Clear();
                ModelState.AddModelError("BirthDate", "Podaj poprawną datę urodzenia.");
            }
            return View(user);
        }
        public IActionResult Edit(int id)
        {
            var user = _userXmlService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _userXmlService.UpdateUser(user);
                return RedirectToAction("Index");
            }

            return View(user);
        }
        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;

            return age;
        }
        public IActionResult Delete(int id)
        {
            var user = _userXmlService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user); // Show confirmation
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _userXmlService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            _userXmlService.DeleteUser(id);

            return RedirectToAction("Index");
        }

        public IActionResult GenerateReport()
        {
            var users = _userXmlService.ReadUsersFromXML();

            // Raport data init
            var reportData = users.Select(user => new
            {
                FirstName = user.Name,
                LastName = user.Surname,
                BirthDate = user.BirthDate,
                Gender = user.Gender == Gender.Male ? "Pan" : "Pani",
                Age = CalculateAge(user.BirthDate),
                TelephoneNumber = user.TelephoneNumber,
                Position = user.Position,
                ShoeSize = user.ShoeSize
            });

            // Możesz tutaj również użyć dowolnego mechanizmu generowania raportów, np. biblioteki do generowania PDF lub Excel.

            // Przykład: Użycie biblioteki CsvHelper do generowania pliku CSV
            var csvData = new List<string>();
            csvData.Add("Imię, Nazwisko, Data urodzenia, Tytuł, Wiek, Numer telefonu, Stanowisko, Numer buta");

            foreach (var item in reportData)
            {
                csvData.Add($"{item.FirstName}, {item.LastName}, {item.BirthDate}, {item.Gender}, {item.Age}, {item.TelephoneNumber}, {item.Position}, {item.ShoeSize}");
            }

            var csvContent = string.Join(Environment.NewLine, csvData);

            // Save
            var reportFileName = $"{DateTime.Now:yyyyMMddHHmmss}.csv";
            var reportFilePath = Path.Combine("wwwroot", reportFileName);
            System.IO.File.WriteAllText(reportFilePath, csvContent);

            return File($"/{reportFileName}", "text/csv", reportFileName);
        }
    }
}