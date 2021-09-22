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
using System.Threading;

namespace SocketServerTest.Client
{
    /// <summary>
    /// Client.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Client : Page
    {
        public Client()
        {
            InitializeComponent();
        }

        private ClientLogic logic;
        private void OnConnect(object sender, RoutedEventArgs e)
        {
            logic = new ClientLogic();
        }

        

        private void OnSend(object sender, RoutedEventArgs e)
        {
            List<int> list = new List<int>();
            for(int i = 0; i < 100; i++)
            {
                list.Add(i);
            }

            var item = Parallel.ForEach(list, (x) =>
            {
                logic.Send();
            });
            var t = Task.Run(() =>
            {
                while (item.IsCompleted == false)
                {
                    Thread.Sleep(1);
                }
            });

            t.Wait();
        }
    }
}
