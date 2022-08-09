using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ConsoleApp.Data.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConsoleApp
{
    public class CustomerDemo
    {
        public CustomerDemo()
        {
            Orders = new List<OrderDemo>();  //  Burada OrdersDemoyu Orders adı altında CustomerDemoya constrakte ettik. Çağırdımızda gelsin diye.
        }
        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public int OrderCount { get; set; }
        public List<OrderDemo> Orders { get; set; } // OrderDemo tablosunun ilişkisini bu şekilde gösterdik. Sonuçta bir müşterinin birden fazla 
                                                    // siparişi olabilir.
    }
    public class OrderDemo
    {
        public int OrderId { get; set; }
        public decimal Total { get; set; }
        public List<ProductDemo> Products { get; set; }
    }

    public class ProductDemo
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TemelSqlSorguYontemi();
        }
        static void A() // Şirket isimlerini getir.
        {
            using (var db = new NothwindContext())
            {
                var customers = db.Customers.ToList();
                foreach (var item in customers)
                {
                    Console.WriteLine(item.CompanyName);
                }
            }
        }
        static void B() // Şirket isim ve Idleri seçip getir.
        {
            using (var db = new NothwindContext())
            {
                var customers = db.Customers.Select(c => c.CompanyName + " ---- " + c.CustomerId).ToList();
                foreach (var item in customers)
                {
                    Console.WriteLine(item);
                }
            }
        }
        static void C() // London da olan müşterileri isim sırasına göre getir.
        {
            using (var db = new NothwindContext())
            {
                var customers = db.Customers.Where(c => c.City == "London").Select(x =>
                 new
                 {
                     //Burada city ve CompanyName diye isimlendirmemizin sebebi anonim Type olışturduk ve bu type 'ları
                     //kontrol edebilmek için onları isimlendirdik ve consolda buna göre çağırdık.
                     City = x.City,
                     CompanyName = x.CompanyName
                 }).ToList();
                foreach (var item in customers)
                {
                    Console.WriteLine($"City = {item.City}, CompanyName = {item.CompanyName}");
                }
            }

        }
        static void D() //Baverage categorisine ayit ürünleri getir.
        {
            using (var db = new NothwindContext())
            {
                var product = db.Products.Where(c => c.CategoryId == 1).Select(p =>
                  new { ProductName = p.ProductName }).ToList();

                foreach (var item in product)
                {
                    Console.WriteLine($"Baverage Kategorisine ait ürümler = {item.ProductName}");
                }
            }
        }
        static void E() //En son eklenen 5 ürünü getir.
        {
            using (var db = new NothwindContext())
            {
                var products = db.Products.OrderByDescending(i => i.ProductId).Take(5);
                foreach (var item in products)
                {
                    Console.WriteLine(item.ProductName);
                }
            }
        }
        static void F() // Fiyatı 10 ile 30 arasında olan ürünlerin isim ve fiyat bilgisini getir.
        {
            using (var db = new NothwindContext())
            {
                var products = db.Products
                                          .Where(p => p.UnitPrice > 10 && p.UnitPrice < 30)
                                          .Select(i =>
                                          new
                                          {
                                              ProductName = i.ProductName,
                                              UnitPrice = i.UnitPrice
                                          }).ToList();
                foreach (var item in products)
                {
                    Console.WriteLine($"Ürün Adı = {item.ProductName} ----- Ürün Fiyatı = {item.UnitPrice}");
                }
            }
        }
        static void G() // Baverage Categorisindeki ürünlerin ortalama fiyatlarını getir.
        {
            using (var db = new NothwindContext())
            {
                var products = db.Products.Where(c => c.CategoryId == 1)
                                           .Average(x => x.UnitPrice)
                                           .ToString();
                Console.WriteLine($"Ortalama Fiyat ={products}");
            }
        }
        static void H() // Baverage Categorisinde kaç tane ürün vardır ?
        {
            using (var db = new NothwindContext())
            {
                var products = db.Products.Count(c => c.CategoryId == 1).ToString();
                Console.WriteLine($"Baverage Kategorisindeki Ürün Sayısı {products}");
            }
        }
        static void S() // Baverage ve Condiment Categorisindeki ürünler toplam fiyatı ?
        {
            using (var db = new NothwindContext())
            {
                var products = db.Products.Where(c => c.CategoryId == 1 || c.CategoryId == 2)
                .Sum(i => i.UnitPrice);
                Console.WriteLine($"Toplam Fiyat = {products}");
            }
        }
        static void T() // Lou kelimesini içeren ürünleri getiriniz. 
        {
            using (var db = new NothwindContext())
            {
                var products = db.Products.Where(n => n.ProductName.Contains("Lou")).ToList();
                foreach (var item in products)
                {
                    Console.WriteLine($"İçinde Lou Geçen Ürünler {item.ProductName}");
                }
            }
        }
        static void Y() // En pahalı ve en ucuz ürün hangisidir isim ve fiyat olarak getir.
        {
            using (var db = new NothwindContext())
            {
                var maxPrice = db.Products.Max(m => m.UnitPrice).ToString();
                var minPrice = db.Products.Min(m => m.UnitPrice).ToString();
                Console.WriteLine($"En düşük fiyat = {minPrice} ------ En yüksek fiyat = {maxPrice}");

                var product = db.Products.Where(i => i.UnitPrice == (db.Products.Min(m => m.UnitPrice))).FirstOrDefault();

                var product1 = db.Products.Where(i => i.UnitPrice == (db.Products.Max(n => n.UnitPrice))).FirstOrDefault();
                Console.WriteLine();

                Console.WriteLine($"En yüksek fiyatlı ürün = {product1.ProductName} ------- Fiyatı = {maxPrice}");
                Console.WriteLine($"En düşük fiyatlı ürün = {product.ProductName} ------- Fiyatı = {minPrice}");

                Console.WriteLine();



            }
        }
        static void U() // Birden çok tabloyla ilişki kurarak şirketlerin ismi, verdikleri sipariş sayıları, sipariş numaraları ve toplam ödediği fiyatlar.
        {
            using (var db = new NothwindContext())
            {
                var customers = db.Customers.Where(i => i.Orders.Count() > 0)   //Customer ana tablosuna bir where ve select sorguu attık. Bu select sorgusunda 
                                            .Select(i => new CustomerDemo
                                            {  // custmer ana tablosundaki ulaşabildiğimiz bilgileri CustomerDemo Tablosuna tanıttık.
                                                CustomerId = i.CustomerId, // Aşağıda Orders'a geldiğimizde ikinci bir select sorgusunu attık.Aynı zamanda OrderDemo
                                                OrderCount = i.Orders.Count(), // tablosunu oluşturduk ve CustomerDemo tablosuyla constrakte ettik. İlişkilendirdik. 
                                                CompanyName = i.CompanyName,  // CustomerDemo tablosundaki orders propuna bilgiyi atıp select sorgusunu atıp, OrderDemo proplarının 
                                                Orders = i.Orders.Select(a => new OrderDemo
                                                {   //  bilgilerini girmeye başladık. OrderId bilgisi ve total bilgileri tersten gidersek önce 
                                                    OrderId = a.OrderId, // constrakte olduğu customer demo sonra onun ilişkili olduğu customer ana tablosu ve customer ana tablosunun ilişkili
                                                    Total = a.OrderDetails.Sum(a => a.UnitPrice * a.Quantity), // olduğu orders ve order details tablolarından geliyor. 
                                                    Products = a.OrderDetails.Select(p => new ProductDemo
                                                    {
                                                        ProductId = p.ProductId,
                                                        Name = p.Product.ProductName
                                                    }).ToList()
                                                }).ToList()
                                            }).ToList();
                foreach (var customer in customers) // Burada Customer ana tablosundan gelen customerleri döndürdük ve bilgileri yazdık
                {
                    Console.WriteLine(customer.CustomerId + " " + customer.CompanyName + " " + customer.OrderCount);
                    Console.WriteLine();
                    foreach (var order in customer.Orders) // Burada ise bir customerin verdiği orders sayısı var. Adam 4 sipariş verdiyse, her bir sipariş dönecek tek tek dönecek.  
                    {
                        Console.WriteLine($"Order Id = { order.OrderId} ---- Total{order.Total}"); // Bu dönen her bir siparişin console da istenilen bilgilerini tek tek bastırıp bir
                        Console.WriteLine();
                        foreach (var item in order.Products)
                        {
                            Console.WriteLine($"Product Id = {item.ProductId} ---- Product Name = {item.Name}");
                            Console.WriteLine();

                        }
                    }
                }
            }
        }

        static void TemelSqlSorguYontemi()
        {
            using (var db = new CustomNorthwindContext())
            {
                var customers = db.CustomerOrders
                .FromSqlRaw("select c.CustomerID as CustomerId , c.CompanyName as CompanyName, Count(*) as OrderCount from customers c inner join Orders o on c.CustomerID = o.CustomerID group by c.CustomerID, c.CompanyName").ToList();
                
                foreach (var item in customers)
                {
                    Console.WriteLine($"customer id = {item.CustomerId} -- company name = {item.CompanyName} -- order count = {item.OrderCount}");                    
                }
            }

        }
    }
}

