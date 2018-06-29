using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Windows.Controls.Primitives;
using System.Data;
using System.Data.Linq;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Data.Entity.Validation;
using LinqToDB.SqlQuery;
using System.Net.Mail;
using System.Net;
using MongoDB.Driver;
using System.Security.Cryptography;

//1406 data too long
//1048 cannot be null
//1062 duplicate
namespace CardReaderClient
{

    public class student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int indeks { get; set; }
        public String imie { get; set; }
        public String nazwisko { get; set; }

    }

    public class przedmiot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_przed { get; set; }
        public string sala { get; set; }
        public int prow_id { get; set; }
       
        public string dzien { get; set; }
        public string godzina { get; set; }
        
        public string nazwa { get; set; }
        //public DateTime? DataDolaczenia { get; set; }



/*
        public przedmiot(int idTean, int id, string kolor)
        {
            idDruzyny = idTean;
            idMeczu = id;
            Kolor_druzyny = kolor;
        }

        public druzyna()
        {

        }
        public override string ToString()
        {
            return "Drużyna: " + idDruzyny + " z meczu: " + idMeczu + " o kolorze: " + Kolor_druzyny;
        }
        */
    }

    public class zajecia
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int id_zajec { get; set; }
            public DateTime? data { get; set; }
            public string nazwa { get; set; }
            
            public int typ_zaj { get; set; }
           
        }
        public class prowadzacy
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int id_prow { get; set; }
            public string imie { get; set; }
            public string nazwisko { get; set; }
            public string skrot_hasla { get; set; }
    }
    public class obowiazek_obecnosci
        {
            [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public int stud_id { get; set; }
        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int przed_id { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is  obowiazek_obecnosci)) return false;
            if (((obowiazek_obecnosci)obj).przed_id == this.przed_id &&
                ((obowiazek_obecnosci)obj).stud_id == this.stud_id
               )
                return true;
            return false;
        }
    }
    public class obecnosc
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int zaj_id { get; set; }
        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int stud_id { get; set; }
    }

        


    public class CardReaderDB : DbContext
        {
            public DbSet<student> student { get; set; }
        //public DbSet<raport_dzienny> raport_dzienny { get; set; }
        public DbSet<zajecia> zajecia { get; set; }
            public DbSet<prowadzacy> prowadzacy { get; set; }
            public DbSet<przedmiot> przedmiot { get; set; }
            public DbSet<obowiazek_obecnosci> obowiazek_obecnosci { get; set; }
            public DbSet<obecnosc> obecnosc { get; set; }
            
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Entity<student>().ToTable("student");
            modelBuilder.Entity<zajecia>().ToTable("zajecia");
            modelBuilder.Entity<prowadzacy>().ToTable("prowadzacy");
            modelBuilder.Entity<przedmiot>().ToTable("przedmiot");
            modelBuilder.Entity<obowiazek_obecnosci>().ToTable("obowiazek_obecnosci");
            modelBuilder.Entity<obecnosc>().ToTable("obecnosc");
        }
    }


        public partial class MainWindow : Window
        {/*
            private gracz get_gracz()
            {
                int ids = -1;
                string nicks;
                string steams;
                string nazwas;

                try
                {
                    if (id.Text.Equals(""))
                        ids = -1;
                    else ids = Convert.ToInt32(id.Text);
                }
                catch (System.FormatException e)
                {
                    MessageBox.Show("Zły format ID");
                    return null;
                }
                catch (System.OverflowException e)
                {
                    MessageBox.Show("Przekroczenie wartości zmiennej ID (Int32)");
                    return null;
                }

                if (nick.Text.Equals(""))
                    nicks = null;
                else nicks = nick.Text;
                if (steam.Text.Equals(""))
                    steams = null;
                else
                    steams = steam.Text;
                if (nazwa.Text.Equals(""))
                    nazwas = null;
                else
                    nazwas = nazwa.Text;



                return new gracz { IDGracza = ids, Nick = nicks, SteamID = steams, NazwaKontaWGrze = nazwas };
            }
            */
            public MainWindow()
            {
                InitializeComponent();
                Main.Content = new Login(Main, this);

         
            }


       

    
    }
    
}