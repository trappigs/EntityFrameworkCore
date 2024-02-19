using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Data
{
    public class DataContext: DbContext
    {
        public DbSet<Kurs> Kurslar => Set<Kurs>();

        public DbSet<Ogrenci> Ogrenciler=> Set<Ogrenci>();

        public DbSet<KursKayit> KursKayitlari=> Set<KursKayit>();

    }

    // code-first = şu an yaptığımız
    // code-first => entity, dbcontext => database
    // database-first => sql server
}
