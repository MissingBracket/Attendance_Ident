using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace CardReaderClient
{

    public class przedmiot_info
    {
        public string nazwa { get; set; }
       
        
        public string dzien { get; set; }
      
        
        public string godzina { get; set; }
        public string sala { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is przedmiot_info)) return false;
            if (((przedmiot_info)obj).nazwa.Equals(this.nazwa) &&
                ((przedmiot_info)obj).dzien.Equals(this.dzien) &&
               
                ((przedmiot_info)obj).godzina.Equals(this.godzina) &&
                ((przedmiot_info)obj).sala.Equals(this.sala))
                return true;
            return false;
        }

    }
    public class przedmiot_coll
    {
        public int przed_id { get; set; }
        public int prow_id { get; set; }
        public przedmiot_info przedmiot_info { get; set; }
    }

    /// <summary>
    /// Interaction logic for MySubjects.xaml
    /// </summary>
    public partial class MySubjects : Page
    {
        private CardReaderDB mDB;
        private prowadzacy prowadzacy;
        private przedmiot_info przedmiot = null;
        private IEnumerable<przedmiot_coll> przedmiot_cols;
        public MySubjects(CardReaderDB mDB, prowadzacy prowadzacy)
        {
            InitializeComponent();
            this.mDB = mDB;
            this.prowadzacy = prowadzacy;
            przedmiot_cols = new List<przedmiot_coll>();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            refresh();
        }
        public void refresh()
        {
            przedmiot_cols = (from x in mDB.przedmiot where x.prow_id == prowadzacy.id_prow select new przedmiot_coll { przedmiot_info = new przedmiot_info { nazwa = x.nazwa, dzien = x.dzien, godzina = x.godzina, sala=x.sala }, przed_id = x.id_przed, prow_id=x.prow_id }).ToList();
            dataGrid.ItemsSource = (from x in przedmiot_cols select x.przedmiot_info);
        }

        private void RowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow rowContainer = (DataGridRow)sender;
            przedmiot = (przedmiot_info)rowContainer.Item;


            dataGridObec.ItemsSource = getData().DefaultView;



        }
        private DataTable getData()
        {
            DataTable dt = new DataTable();
            int id_przed = (from x in przedmiot_cols where x.przedmiot_info.Equals(przedmiot) select x.przed_id).First();
            List<int> obowiazki = (from x in mDB.obowiazek_obecnosci where x.przed_id == id_przed select x.stud_id).ToList();
            List<student> studenci = (from x in mDB.student where obowiazki.Contains(x.indeks) select x).ToList();
            dt.Columns.Add("Temat");
            foreach (student stud in studenci)
            {
                dt.Columns.Add(stud.nazwisko +" "+ stud.indeks, typeof(Boolean));
            }
            List<zajecia> zajecia = (from x in mDB.zajecia where x.typ_zaj == id_przed select x).ToList();
            foreach (zajecia zaj in zajecia)
            {
                DataRow row = dt.NewRow();
                row[0] = zaj.nazwa;
                int i = 1;
                foreach (student stud in studenci)
                {
                    Boolean temp = false;
                    List<obecnosc> obecnosci = (from x in mDB.obecnosc where x.zaj_id == zaj.id_zajec && x.stud_id == stud.indeks select x).ToList();
                    if (obecnosci.Count() > 0)
                        temp = true;
                    row[i] = temp;
                   
                    i++;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            mDB = new CardReaderDB();
            if (przedmiot != null)
            {
                dataGridObec.ItemsSource = getData().DefaultView;
            }
        }
    }
}
