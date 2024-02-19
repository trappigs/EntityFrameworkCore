using EntityFrameworkCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // Ogrenciler tablosundaki tüm verileri listeleyen bir view döndürülüyor
        public async Task<IActionResult> Index()
        {
            // await olarak yollayabilmek için ToListAsync() metodu kullanılıyor 
            var ogrenciler = await _context.Ogrenciler.ToListAsync();

            return View(ogrenciler);
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

            return RedirectToAction("Index");
        }


    }
}
