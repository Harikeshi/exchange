using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using work.logic;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            //this.Top = 100;//0;
            //this.Left = 100;// 0;
            //this.Height = 300;
            //this.Width = 800;//System.Windows.SystemParameters.PrimaryScreenWidth;
            //this.Topmost = true;

            // Разделитель string('+', 40); между транзакциями

            //TODO1:p
            // Проходим построчно

            // Замечания:
            // 40* где бы не нашли просто определять в массив отдельный, так как эта хрень может быть в любом месте 
            // от 40* до 40*

            TextBox t = new TextBox();



            int i = 0;
        }

        private void BuildTree()
        {



        }



        private void generateButton(object sender, RoutedEventArgs e)
        {
            // скачать EJ по датам и номеру банкомата  
            //
            //string journal = getEJournal();

            string path = "D:\\temp\\work\\ej.txt";
            EJournal ej = new EJournal(path);
            treeTransactions.ItemsSource = ej.Clients;




        }

        private void getEJournal(DateTime dateTime1, object start, DateTime dateTime2, object end, string string1, object type, string string2, object number)
        {
            throw new NotImplementedException();
        }

        void EnterClicked(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                MessageBox.Show("1");
                e.Handled = true;
            }
        }

        private void EnterClicked1(object sender, KeyEventArgs e)
        {
            generateButton(sender, e);
        }
    }
}
