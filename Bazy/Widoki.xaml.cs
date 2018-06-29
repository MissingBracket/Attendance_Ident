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
using System.Data.Entity.Core.Metadata.Edm;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CardReaderClient
{
    /// <summary>
    /// Interaction logic for Widoki.xaml
    /// </summary>
    public partial class Widoki : Page
    {


        public Widoki()
        {
            InitializeComponent();
            
            List<String> list = new List<String>();

            CardReaderClient.CardReaderDB context = new CardReaderClient.CardReaderDB();
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            var tables = metadata.GetItemCollection(DataSpace.SSpace)
                .GetItems<EntityContainer>()
                .Single()
                .BaseEntitySets
                .OfType<EntitySet>()
                .Where(s => !s.MetadataProperties.Contains("Type")
                || s.MetadataProperties["Type"].ToString() == "Tables");

            foreach (var table in tables)
            {
                var tableName = table.MetadataProperties.Contains("Table")
                    && table.MetadataProperties["Table"].Value != null
                    ? table.MetadataProperties["Table"].Value.ToString()
                    : table.Name;

                list.Add(tableName);
            }


            listView.ItemsSource = list;
            
        }

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CardReaderClient.CardReaderDB mdb = new CardReaderClient.CardReaderDB();
            
            if (    (    ((ListView)(sender)).SelectedItem).Equals("przedmiot")            )
         
            dataGrid.ItemsSource = mdb.przedmiot.ToList();


            if ((((ListView)(sender)).SelectedItem).Equals("prowadzacy"))

                dataGrid.ItemsSource = mdb.prowadzacy.ToList();

            if ((((ListView)(sender)).SelectedItem).Equals("obecnosc"))

                dataGrid.ItemsSource = mdb.obecnosc.ToList();


            if ((((ListView)(sender)).SelectedItem).Equals("obowiazek_obecnosci"))

                dataGrid.ItemsSource = mdb.obowiazek_obecnosci.ToList();

            if ((((ListView)(sender)).SelectedItem).Equals("student"))

                dataGrid.ItemsSource = mdb.student.ToList();


            if ((((ListView)(sender)).SelectedItem).Equals("zajecia"))

                dataGrid.ItemsSource = mdb.zajecia.ToList();


        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            int ind = dataGrid.SelectedIndex;

            DataTable x = new DataTable(listView.SelectedItem.ToString());
            CardReaderClient.CardReaderDB mdb = new CardReaderClient.CardReaderDB();

        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            //NavigationService navService = NavigationService.GetNavigationService(this);
            //AddStudents asd = new AddStudents();
            //navService.Navigate(asd);



        }
    }
}
