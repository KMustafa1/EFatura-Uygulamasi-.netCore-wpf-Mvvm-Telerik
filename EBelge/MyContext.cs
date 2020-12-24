using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EBelge
{
    

    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Model { get; set; }
        public string Price { get; set; }
        public decimal Quantity { get; set; }    
    }

    public class Profil
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfilId { get; set; }
        public string VKN { get; set; }
        public string MERSISNO { get; set; }
        public string TICARETSICILNO { get; set; }
        public string VD { get; set; }
        public string PartyName { get; set; }
        public string Country { get; set; }
        public string CityName { get; set; }
        public string CitySubdivisionName { get; set; }
        public string StreetName { get; set; }
        public string Telephone { get; set; }
        public string Telefax { get; set; }
        public string ElectronicMail { get; set; }
        public string WebsiteURI { get; set; }
    }

    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        public string VKN_TCKN { get; set; }
        public string PartyName { get; set; }
        public string Country { get; set; }
        public string CityName { get; set; }
        public string CitySubdivisionName { get; set; }
        public string StreetName { get; set; }
        public string ElectronicMail { get; set; }
        public virtual List<User> Users { get; set; }
    }

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Unvan { get; set; }
        public string Ad_Soyad { get; set; }
        public string Telefon { get; set; }
        public string E_Posta { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }

    public class MyContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // You might need to update the connection string to suit the setup on your machine 
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
        }

        public DbSet<Profil> Profils { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        
    }
}
