using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCore.Data
{
    public class KursKayit
    {
        [Key]
        public int KayitId { get; set; }

        public int OgrenciId { get; set; }

        public int KursId { get; set; }

        public DateTime KayitTarihi { get; set; }


        // OgrenciId ve KursId alanları ile Ogrenci ve Kurs tabloları arasında ilişki kuruldu.
        // Bu sayede Ogrenci ve Kurs tablolarındaki verilere ulaşabileceğiz.
        // Bu alanlar ile veritabanında birer foreign key oluşturulacak.
        // Ogrenci değişkeni, Ogrenci tablosunun barındırdığı sütunlara erişebilecek
        // Kurs değişkeni, Kurs tablosunun barındırdığı sütunlara erişebilecek
        public Ogrenci Ogrenci { get; set; } = null!; // ogrenci-id: 1 => kurs-id: 5

        public Kurs Kurs { get; set; } = null!;


        // ogrenci-id: 1 => kurs-id: 5
    }
}
