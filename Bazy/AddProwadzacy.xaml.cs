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
    /// Interaction logic for AddProwadzacy.xaml
    /// </summary>
    public partial class AddProwadzacy : Page
    {

        private CardReaderDB mDB;
        private prowadzacy prowadzacy;
        public AddProwadzacy(CardReaderDB mDB, prowadzacy prowadzacy)
        {
            InitializeComponent();
            this.mDB = mDB;
            this.prowadzacy = prowadzacy;
        }


        private String hash(String password)
        {
           
            byte[] bypass = System.Text.Encoding.Default.GetBytes(password);

            return  Login.ByteArrayToString(Login.sha(bypass));
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!(passBox.Password.Equals(repeatBox.Password)))
            {
                MessageBox.Show("Podane hasła są różne");
                return;
            }

            String hashed = hash(passBox.Password);

            try
            {
                mDB.prowadzacy.Add(new prowadzacy { imie = imieText.Text, nazwisko= nazwiskoText.Text, skrot_hasla = hashed });
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
            String hashed = hash(passBox.Password);

            try
            {
                prowadzacy to_delete = (from x in mDB.prowadzacy where (x.imie.Equals(imieText.Text) && x.nazwisko.Equals(nazwiskoText.Text) && x.skrot_hasla.Equals(hashed)) select x).First();
                mDB.prowadzacy.Remove(to_delete);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Nie ma takiego prowadzącego lub złe hasło.");
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
