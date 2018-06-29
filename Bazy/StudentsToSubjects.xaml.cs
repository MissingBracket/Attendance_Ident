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
using System.Collections;
using System.Data.Entity.Infrastructure;

namespace CardReaderClient
{
   
    /// <summary>
    /// Interaction logic for StudentsToSubjects.xaml
    /// </summary>
    public partial class StudentsToSubjects : Page
    {
        private IEnumerable<przedmiot_coll> przedmiot_cols;
        private CardReaderDB mDB;
        private prowadzacy prowadzacy;
        private ArrayList studenci;
        private ArrayList przedmioty;
        private SolidColorBrush white;
        private SolidColorBrush green;
        public StudentsToSubjects(CardReaderDB mDB, prowadzacy prowadzacy)
        {
            InitializeComponent();
            this.mDB = mDB;
            this.prowadzacy = prowadzacy;
            przedmiot_cols = new List<przedmiot_coll>();
            studenci = new ArrayList();
            przedmioty = new ArrayList();
            white = new SolidColorBrush(Colors.White);
            green = new SolidColorBrush(Colors.Green);
        }
        public void clear()
        {
            studenci.Clear();
            przedmioty.Clear();
      
        }
        public void refresh()
        {
            dataGrid.ItemsSource = mDB.student.ToList();
            przedmiot_cols = (from x in mDB.przedmiot where x.prow_id == prowadzacy.id_prow select new przedmiot_coll { przedmiot_info = new przedmiot_info { nazwa = x.nazwa, dzien = x.dzien, godzina = x.godzina, sala=x.sala }, przed_id = x.id_przed }).ToList();
            dataGrid_Copy.ItemsSource = (from x in przedmiot_cols select x.przedmiot_info);
        }
        private void RowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow rowContainer = (DataGridRow)sender;
            student stud = (student)rowContainer.Item;
            if (studenci.Contains(stud))
            {
                studenci.Remove(stud);
                rowContainer.Background = white;
            }
            else
            {
                studenci.Add(stud);
                rowContainer.Background = green;
            }  
            


        }
        private void RowDoubleClickPrzedmiot(object sender, MouseButtonEventArgs e)
        {
            DataGridRow rowContainer = (DataGridRow)sender;
            przedmiot_info przed = (przedmiot_info)rowContainer.Item;
            if (przedmioty.Contains(przed))
            {
                przedmioty.Remove(przed);
                rowContainer.Background = white;
            }
            else
            {
                przedmioty.Add(przed);
                rowContainer.Background = green;
            }



        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (student stud in studenci)
                {
                    foreach (przedmiot_info przed in przedmioty)
                    {
                        int przed_ids = (from x in przedmiot_cols where x.przedmiot_info.Equals(przed) select x.przed_id).First();
                        obowiazek_obecnosci temp = new obowiazek_obecnosci { przed_id = przed_ids, stud_id = stud.indeks };
                        if (mDB.obowiazek_obecnosci.ToList().Contains(temp))
                        {
                            MessageBox.Show("Student: " + temp.stud_id + " już jest przypisany do przedmiotu: " + (from x in przedmiot_cols where x.przed_id == temp.przed_id select x.przedmiot_info.nazwa).First());
                        }
                        else mDB.obowiazek_obecnosci.Add(temp);


                    }
                }
                int res = mDB.SaveChanges();
                if (res == 0)
                {
                    MessageBox.Show("Nie wybrano żadnego studenta lub przedmiotu.");
                    return;
                }
                else
                    if (res == studenci.Count * przedmioty.Count)
                    MessageBox.Show("Pomyślnie dodano.");
                else
                    MessageBox.Show("Wystąpił błąd i nie dodano części studentów - być może część z nich była już zapisana na przedmiot.");
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Błąd bazy danych - nie wykonano operacji.");
                mDB = new CardReaderDB();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            clear();
            refresh();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            
            refresh();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            clear();
            refresh();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
           
                foreach (student stud in studenci)
                {
                    foreach (przedmiot_info przed in przedmioty)
                    {
                        try {

                        int a = (from y in przedmiot_cols where y.przedmiot_info.Equals(przed) select y.przed_id).First();
                        IEnumerable<obowiazek_obecnosci> temp = (from x in mDB.obowiazek_obecnosci where x.przed_id == a && x.stud_id == stud.indeks select x);
                        mDB.obowiazek_obecnosci.Remove(temp.First());
                        mDB.obowiazek_obecnosci.RemoveRange(temp);
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show("Nie ma obowiązku uczęszczania dla studenta: " + stud.indeks +", na przedmiot: "+przed.nazwa);

                        }

                    }
                }
            
           



            try
            {
                if(mDB.SaveChanges() >0)
                    MessageBox.Show("Pomyślnie usunięto.");
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Błąd bazy danych - nie wykonano operacji.");
                mDB = new CardReaderDB();
                return;
            }
            clear();
            refresh();
            
        }
    }
}
