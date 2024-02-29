using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCore.Data
{
    public class Ogretmen
    {
        [Key]
        public int OgretmenId { get; set; }

        public string? Ad { get; set; }

        public string? Soyad { get; set; }

        public string AdSoyad
        {
            get
            {
                return this.Ad + " " + this.Soyad;
            }
        }


        public string? Eposta { get; set; }

        public string? Telefon { get; set; }


        // Bu özelliğin veri türü DateTime olacak ve sadece tarih kısmını alacak.
        [DataType(DataType.Date)]
        // Bu özelliğin görüntülenme biçimi belirlenecek.
        // {0:yyyy-MM-dd} ifadesi, yalnızca yıl, ay ve gün bilgilerini gösterir.
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}")]
        public DateTime BaslamaTarihi { get; set; }

        public ICollection<Kurs> Kurslar { get; set; } = new List<Kurs>();
    }
}
