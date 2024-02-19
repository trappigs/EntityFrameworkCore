using EntityFrameworkCore.Data;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCore.Controllers
{
    public class OgrenciController : Controller
    {
        // datacontext sınıfından bir nesne oluşturuluyor ve bu nesne ile veritabanı işlemleri yapılıyor
        private readonly DataContext _context;
        public OgrenciController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ogrenci model)
        {
            // _context üzerinden Ogrenciler tablosuna model nesnesi ekleniyor
            _context.Ogrenciler.Add(model);

            // değişiklikler veritabanına kaydediliyor
            await _context.SaveChangesAsync();

            return RedirectToAction("Index","Home");
        }


    }
}
