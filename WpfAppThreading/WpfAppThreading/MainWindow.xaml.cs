using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

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
            resultsTextBox.Clear();
        }

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            startButton.IsEnabled = false;
            resultsTextBox.Clear();
            await SumPageSizesAsync();
            resultsTextBox.Text += "\r\nControl returned to startButton_Click";
            startButton.IsEnabled = true;
        }

        private async Task SumPageSizesAsync()
        {
            List<string> urlList = SetUpURLList();

            IEnumerable<Task<int>> downloadTasksQuery = from url in urlList select ProcessURLAsync(url);

            Task<int>[] downloadTasks = downloadTasksQuery.ToArray();

            var lengths = await Task.WhenAll(downloadTasks);

            int total = lengths.Sum();

            resultsTextBox.Text += string.Format("\r\n\r\nTotal bytes returned: {0} \r\n", total);
        }

        private List<string> SetUpURLList()
        {
            var urls = new List<string>
            {
                "http://msdn.microsoft.com/library/windows/apps/br211380.aspx",
                "http://msdn.microsoft.com",
                "http://msdn.microsoft.com/en-us/library/hh290136.aspx",
                "http://msdn.microsoft.com/en-us/library/ee256749.aspx",
                "http://msdn.microsoft.com/en-us/library/hh290138.aspx",
                "http://msdn.microsoft.com/en-us/library/hh290140.aspx",
                "http://msdn.microsoft.com/en-us/library/dd470362.aspx",
                "http://msdn.microsoft.com/en-us/library/aa578028.aspx",
                "http://msdn.microsoft.com/en-us/library/ms404677.aspx",
                "http://msdn.microsoft.com/en-us/library/ff730837.aspx"
            };

            return urls;
        }

        private async Task<byte[]> GetURLContentsAsync(string url)
        {
            var content = new MemoryStream();

            var webReq = (HttpWebRequest)WebRequest.Create(url);

            using (WebResponse respone = await webReq.GetResponseAsync())
            {
                using (Stream responseStream = respone.GetResponseStream())
                {
                    await responseStream.CopyToAsync(content);
                }
            }

            return content.ToArray();
        }

        private void DisplayResults(string url, byte[] content)
        {
            var bytes = content.Length;

            resultsTextBox.Text += string.Format("\n{0,-58} {1,8}", url, bytes);
        }

        private async Task<int> ProcessURLAsync(string url)
        {
            var byteArray = await GetURLContentsAsync(url);
            DisplayResults(url, byteArray);
            return byteArray.Length;
        }
    }
}