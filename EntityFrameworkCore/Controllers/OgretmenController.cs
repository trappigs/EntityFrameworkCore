using EntityFrameworkCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Controllers
{
    public class OgretmenController : Controller
    {
        private readonly DataContext _context;

        public OgretmenController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Ogretmenler.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ogretmen model)
        {
            // _context üzerinden Ogrenciler tablosuna model nesnesi ekleniyor
            _context.Ogretmenler.Add(model);

            // değişiklikler veritabanına kaydediliyor
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // id'ye göre öğrenci bulunuyor
            // await olarak yollayabilmek için FindAsync() metodu kullanılıyor
            // id'ye göre öğrenci bulunamazsa NotFound() döndürülüyor
            // var ogrenci = await _context.Ogrenciler.FindAsync(id);

            // alternatif olarak FirstOrDefaultAsync() metodu kullanılabilir
            // FirstOrDefaultAsync() metodundaki fark ise, eğer id'ye göre öğrenci bulunamazsa null döndürür
            // aynı zamanda id değilde farklı parametrelere göre de arama yapılabilir
            var entity = await _context
                .Ogretmenler
                .FirstOrDefaultAsync(m => m.OgretmenId == id);

            if (entity == null)
            {
                return NotFound();
            }

            return View("Edit", entity);
        }

        [HttpPost]
        // ValidateAntiForgeryToken ile güvenlik önlemi alınıyor
        // bu sayede sadece formdan gelen verilerin işlenebileceği garanti altına alınıyor
        // bu sayede bir saldırganın formdan gelen verileri değiştirerek veritabanına zarar vermesi engellenmiş oluyor
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Ogretmen model)
        {
            if (id != model.OgretmenId)
            {
                return NotFound();
            }

            // model nesnesi doğrulama işleminden geçiriliyor
            // eğer model nesnesi doğrulama işleminden geçemezse aynı view tekrar döndürülüyor
            if (ModelState.IsValid)
            {
                try
                {
                    // _context üzerinden Ogrenciler tablosundaki model nesnesi güncelleniyor
                    _context.Update(model);
                    // değişiklikler veritabanına kaydediliyor
                    // bunu yazmasaydık güncelleme işlemi gerçekleşmeyecekti
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // eğer güncelleme işlemi sırasında bir hata oluşursa
                    //  
                    if (!_context.Ogretmenler.Any(o => o.OgretmenId == model.OgretmenId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }

            // _context üzerinden Ogrenciler tablosuna model nesnesi ekleniyor
            _context.Ogretmenler.Add(model);

            return RedirectToAction("Index");
        }

    }
}
