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

            return RedirectToAction("List", "Kurs");
        }

        public IActionResult List()
        {
            var kurslar = _context.Kurslar.ToList();
            return View(kurslar);
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
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Kurslar = _context.Kurslar.FirstOrDefault(m => m.KursId == id);

            return View("Edit", Kurslar);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Kurs Kurslar)
        {

            if (Kurslar == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(Kurslar);

                await _context.SaveChangesAsync();
            }


            return RedirectToAction("List", "Kurs");
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

            return RedirectToAction("List", "Kurs");
        }
    }
}
