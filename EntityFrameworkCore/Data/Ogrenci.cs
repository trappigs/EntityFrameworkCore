using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCore.Data
{
    public class Ogrenci
    {
        // id => primary key
        [Key]
        public int OgrenciId { get; set; }

        public string? OgrenciAd { get; set; }
        public string? OgrenciSoyad { get; set; }

        public string AdSoyad
        {
            get
            { return this.OgrenciAd + " " + this.OgrenciSoyad; }
        }

        public string? Eposta { get; set; }
        public string? Telefon { get; set; }

        // ogrenci-id: 1 => kurs-id: 5
        // ICollection => List, Array, HashSet
        // ICollection => birden fazla kayıt tutabileceğimiz bir veri yapısı
        // KursKayitlari => Ogrenci tablosundaki ogrenci-id: 1 olan öğrencinin hangi kurslara kayıt olduğunu tutacak
        // KursKayitlari => Kurs tablosundaki kurs-id: 5 olan kursa hangi öğrencilerin kayıt olduğunu tutacak
        // Kısacası, KursKayit.cs biglilerini bu koleksiyon içerisinde tutacağız.
        public ICollection<KursKayit> KursKayitlari { get; set; } = new List<KursKayit>();
    }
}
