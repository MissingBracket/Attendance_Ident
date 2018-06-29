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
using CardReaderClient;
using System.Data.Entity.Infrastructure;

namespace CardReaderClient
{
    /// <summary>
    /// Interaction logic for AddStudents.xaml
    /// </summary>
    public partial class AddStudents : Page
    {

        private CardReaderDB mDB;
        public AddStudents(CardReaderDB mDB)
        {
            InitializeComponent();
            this.mDB = mDB;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                mDB.student.Add(new student { imie = imieStudentaText.Text, nazwisko = nazwiskoStudentaText.Text, indeks = Int32.Parse(indeksText.Text) });
                if (mDB.SaveChanges() == 1)
                    MessageBox.Show("Pomyślnie dodano studenta.");
                else
                    MessageBox.Show("Wystąpił błąd i nie dodano studenta");
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

        private void deleteStudentButton_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                int a = Int32.Parse(indeksText.Text);
                student to_delete = (from x in mDB.student where x.indeks == a select x).First();
                mDB.student.Remove(to_delete);
            }

            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Nie ma studenta o takim indeksie.");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
