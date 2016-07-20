using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace WpfAppThreading
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public async void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            textBox.Text += "\n";

            try
            {
                int length = await ExampleMethodAsync();

                textBox.Text += String.Format("length: {0}\n", length);
            }
            catch (Exception)
            {
                textBox.Text += "Exception occured";
            }

            textBox.SelectionStart = textBox.Text.Length;
            textBox.ScrollToEnd();
        }

        public async Task<int> ExampleMethodAsync()
        {
            var httpClient = new HttpClient();
            int length = (await httpClient.GetStringAsync("http://www.google.com")).Length;
            return length;
        }

        public void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        

    }
}
