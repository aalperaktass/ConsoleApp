using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Data.EfCore
{
    public class CustomerOrder
    {
        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public int OrderCount { get; set; }
    }

    public class CustomNorthwindContext : NothwindContext  
    {
        public  DbSet<CustomerOrder> CustomerOrders { get; set; }
        public CustomNorthwindContext()
        {
            
        }
        public CustomNorthwindContext(DbContextOptions<NothwindContext>options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
             modelBuilder.Entity<CustomerOrder>(entity =>
            {
                entity.HasNoKey();
            });
        }
    }
}
// Basit sql sorgusunda linq gibi farklı yerlere de sorgu atabilmek için ayrı bir yer oluşturduk.
// Bu oluşturduğumuz yerin içine asıl databaseye bağlı yeni bir context class'ı oluşturduk. 
// Bu dosyanın içinde bizim sorgu atacağımız yerlerden gelen bilgileri map edebileceğimiz ayrı bir class daha oluşturduk.
// Bu oluşturduğumuz class yeni bir tablo  olduğu için context class'ımıza  db set olarak tanımladık.
// Burada iki tane constraction oluşturduk biri parametreli diğeri ise parametresiz.
//Parametreli olanın amacı bizim bu context dosyasını ve ana database üzerinden bir şeyler çağırabilmemize yardımcı olur.
// Çünkü bu constractionun yönlendirildiği yer ana databasedir. Yani NothwindContext dosyasıdır.
// Parametresiz oluşturduğumuz constraction metodu ise arka tarafta daha tablo oluşturulmadan OnModelCreating metodu içine
//yazılan "entity." ifadesiyle başlayan parametreler sayesinde anahtarsız bir yapı türü oluşturduk.
//Bu parametreler değişebilir. 

