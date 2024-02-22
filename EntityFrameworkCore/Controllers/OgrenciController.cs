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
            var ogrenci = await _context.Ogrenciler.FirstOrDefaultAsync(m => m.OgrenciId == id);

            if (ogrenci == null)
            {
                return NotFound();
            }

            return View("Edit", ogrenci);
        }

        [HttpPost]
        // ValidateAntiForgeryToken ile güvenlik önlemi alınıyor
        // bu sayede sadece formdan gelen verilerin işlenebileceği garanti altına alınıyor
        // bu sayede bir saldırganın formdan gelen verileri değiştirerek veritabanına zarar vermesi engellenmiş oluyor
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Ogrenci model)
        {
            if (id != model.OgrenciId)
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
                    if (!_context.Ogrenciler.Any(o => o.OgrenciId == model.OgrenciId))
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
            _context.Ogrenciler.Add(model);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var ogrenci = await _context.Ogrenciler.FindAsync(id);

            if (ogrenci == null)
            {
                return NotFound();
            }

            // ogrenci nesnesi view'e gönderiliyor
            return View(ogrenci);
        }

        [HttpPost]
        // fromform ile formdan gelen veriler alınıyor
        public async Task<IActionResult> Delete([FromForm]int id)
        {
            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            
            if (ogrenci == null)
            {
                return NotFound();
            }

            _context.Ogrenciler.Remove(ogrenci);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



    }
}
