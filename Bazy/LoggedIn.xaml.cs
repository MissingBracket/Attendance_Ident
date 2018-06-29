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
using System.Windows.Shapes;

namespace CardReaderClient
{
    /// <summary>
    /// Interaction logic for LoggedIn.xaml
    /// </summary>
    public partial class LoggedIn : Window
    {
        private CardReaderDB mDB;
        private prowadzacy logged;
        public LoggedIn(prowadzacy logged, CardReaderDB mDB)
        {
            InitializeComponent();
            welcome.Content = "Witaj "+ logged.imie + " "+logged.nazwisko+"!";
            this.logged = logged;
            this.mDB = mDB;
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new AddStudents(mDB);
        }

        private void AddPrzedmiot_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new AddPrzedmiot(mDB, logged);
        }

        private void AddProwadzacy_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new AddProwadzacy(mDB, logged);
        }

        private void AddObowiazek_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new StudentsToSubjects(mDB, logged);
        }

        private void MySubjects_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new MySubjects(mDB, logged);
        }

        private void StartLesson_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new StartLesson(mDB, logged);
        }
    }
}
