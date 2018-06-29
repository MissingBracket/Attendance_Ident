using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
    /// <summary>
    /// Interaction logic for AddPrzedmiot.xaml
    /// </summary>
    public partial class AddPrzedmiot : Page
    {
        private CardReaderDB mDB;
        private prowadzacy prowadzacy;
        public AddPrzedmiot(CardReaderDB mDB, prowadzacy prowadzacy)
        {
            InitializeComponent();
            this.mDB = mDB;
            this.prowadzacy = prowadzacy;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mDB.przedmiot.Add(new przedmiot { nazwa = nazwaText.Text, sala = salaText.Text, dzien = ((ComboBoxItem)dzienBox.SelectedItem).Content.ToString(), godzina = ((ComboBoxItem)godzinaBox.SelectedItem).Content.ToString(), prow_id = prowadzacy.id_prow });
                if (mDB.SaveChanges() == 1)
                    MessageBox.Show("Pomyślnie dodano.");
                else
                    MessageBox.Show("Wystąpił błąd i nie dodano.");
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
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                przedmiot to_delete = (from x in mDB.przedmiot where (x.nazwa.Equals(nazwaText.Text) && x.prow_id == prowadzacy.id_prow) select x).First();
                mDB.przedmiot.Remove(to_delete);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Nie ma przedmiotu o takiej nazwie dla zalogowanego prowadzącego.");
                return;
            }



            try
            {
                mDB.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Błąd bazy danych - nie wykonano operacji.");
                mDB = new CardReaderDB();
                return;
            }
            MessageBox.Show("Pomyślnie usunięto.");
        }
    }
}
