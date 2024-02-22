using EntityFrameworkCore.Data;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCore.Controllers
{
    public class KursController : Controller
    {
        private readonly DataContext _context;

        public KursController(DataContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Kurs model)
        {
            _context.Kurslar.Add(model);

            await _context.SaveChangesAsync();

            return RedirectToAction("List","Kurs");
        }

        public IActionResult List()
        {
            var kurslar = _context.Kurslar.ToList();
            return View(kurslar);
        }


    }
}
