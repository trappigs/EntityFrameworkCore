using EntityFrameworkCore.Data;
using EntityFrameworkCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Controllers
{
    public class KursController : Controller
    {
        private readonly DataContext _context;

        public KursController(DataContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var kurslar = await _context.Kurslar
                .Include(k => k.Ogretmen)
                .ToListAsync();
            return View(kurslar);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // OgretmenId ve Ad bilgileri gerçek tablodan alındığı için, sütun adları doğru girilmeli
            // OgretmenId hangi sütuna göre getirileceği, AdSoyad ile nasıl sergileneceği gösteriliyor
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(),"OgretmenId", "AdSoyad");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KursViewModel model)
        {
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");
            if (ModelState.IsValid)
            {
                _context.Kurslar.Add(new Kurs() { KursId = model.KursId, Baslik = model.Baslik, OgretmenId = model.OgretmenId });

                await _context.SaveChangesAsync();

                return RedirectToAction("Index","Kurs");
            }

            return View(model);
        }

        // Editlenecek Kurs Sayfasının id bilgisi alınıyor
        // id bilgisi ile veritabanından ilgili kurs bilgisi çekiliyor
        // ilgili kurs bilgisi ile birlikte Edit.cshtml sayfasına yönlendiriliyor
        // Edit.cshtml sayfasında ilgili kurs bilgisi gösteriliyor
        // Kullanıcı bu bilgileri düzenleyip kaydetmek istediğinde, bu bilgileri alıp veritabanına kaydediyoruz
        // Edit.cshtml sayfasında düzenlenen bilgileri almak için [HttpPost] olarak işaretlenmiş bir Edit metodu oluşturuyoruz
        // Bu metodu oluştururken, Edit.cshtml sayfasında düzenlenen bilgileri almak için bir Kurs modeli oluşturuyoruz
        // Bu modeli parametre olarak alıyoruz
        // Bu modeli veritabanına kaydediyoruz
        // Veritabanına kaydettikten sonra, kullanıcıyı List.cshtml sayfasına yönlendiriyoruz
        // Bu sayfada, veritabanındaki kurs bilgilerini listeleyip gösteriyoruz
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurs = await _context
                        .Kurslar
                        .Include(k => k.KursKayitlari)
                        .ThenInclude(k => k.Ogrenci)
                        .Select(k => new KursViewModel
                        {
                            KursId = k.KursId,
                            Baslik = k.Baslik,
                            OgretmenId = k.OgretmenId,
                            KursKayitlari = k.KursKayitlari
                        })
                        .FirstOrDefaultAsync(k => k.KursId == id);

            if (kurs == null)
            {
                return NotFound();
            }

            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");

            return View(kurs);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, KursViewModel model)
        {
            if (id != model.KursId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(new Kurs() { KursId = model.KursId, Baslik = model.Baslik, OgretmenId = model.OgretmenId });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!_context.Kurslar.Any(o => o.KursId == model.KursId))
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
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurs = await _context.Kurslar.FindAsync(id);

            if (kurs == null)
            {
                return NotFound();
            }

            // ogrenci nesnesi view'e gönderiliyor
            return View("DeleteConfirm", kurs);
        }

        [HttpPost]
        // fromform ile formdan gelen veriler alınıyor
        public async Task<IActionResult> DeleteConfirm(int id, Kurs kurs)
        {
            if (kurs == null)
            {
                return NotFound();
            }

            _context.Kurslar.Remove(kurs);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Kurs");
        }
    }





}
