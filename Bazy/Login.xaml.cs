using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        CardReaderDB mDB;
        Frame x;
        MainWindow mw;
        public Login(Frame x, MainWindow mw)
        {
            InitializeComponent();
            this.x = x;
            this.mw = mw;
            mDB = new CardReaderDB();
           // mDB = null;
        }

      

       

       

        public static byte[] compute(byte[] bytes, byte[] Salt, HashAlgorithm alg, Boolean use_salt)
        {
            if (!use_salt)
                return alg.ComputeHash(bytes);
            byte[] salted_input = new byte[Salt.Length + bytes.Length];
            Salt.CopyTo(salted_input, 0);
            bytes.CopyTo(salted_input, Salt.Length);
            return alg.ComputeHash(salted_input);
        }



        public static byte[] sha(byte[] bytes)
        {
            byte[] Salt = System.Text.Encoding.Default.GetBytes("DajPan3AlboNawet4Albo4.5BoChybaNie5?");
            SHA256CryptoServiceProvider instance = new SHA256CryptoServiceProvider();

            return compute(bytes, Salt, instance, true);
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            String imie = nameText.Text;
            String nazwisko = surnameText.Text;


            String haslo = passwordBox.Password;
            byte[] bypass = System.Text.Encoding.Default.GetBytes(haslo);

            String hashed = ByteArrayToString(sha(bypass));
            
            
           

            
            IQueryable<prowadzacy> lista = (from x in mDB.prowadzacy where x.imie.Equals(imie) && x.nazwisko.Equals(nazwisko) && x.skrot_hasla.Equals(hashed) select x);

            if (lista.Count() == 0)
                MessageBox.Show("Nie znaleziono prowadzącego o podanych danych.");
            else
            {
                prowadzacy logged = lista.First();

                this.Width = 1200;
                this.Height = 700;
                new LoggedIn(logged, mDB).Show();
                mw.Close();

            }
            

        }

      

      
        private void nameText_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (nameText.Text.Equals("Imie prowadzącego"))
                nameText.Text = String.Empty;
        }

        private void surnameText_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

            if (surnameText.Text.Equals("Nazwisko prowadzącego"))
                surnameText.Text = String.Empty;
        }

        private void passwordBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (passwordBox.Password.Equals("Hasło"))
                passwordBox.Password = String.Empty;
        }
    }
}
