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
using Client.Service;
using Service;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, Client.Service.IServiceCallback
    {
        
        Service.ServiceClient client;
        string name;
        bool isConnected = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConnectUser();
        }

        void ConnectUser()
        {
            if (!isConnected)
            {
                client = new Service.ServiceClient(new System.ServiceModel.InstanceContext(this));
                client.Connect(textBoxUserName.Text);
                name = textBoxUserName.Text;
                textBoxUserName.IsEnabled = false;
                buttonConnectDisconnect.Content = "Disconnect";
                isConnected = true;
            }
        }

        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(name);
                client = null;
                textBoxUserName.IsEnabled = true;
                buttonConnectDisconnect.Content = "Connect";
                isConnected = false;
            }

        }

        //public void MsgCallback(string msg)
        //{
        //    listBoxMessages.Items.Add(msg);
        //    listBoxMessages.ScrollIntoView(listBoxMessages.Items[listBoxMessages.Items.Count - 1]);
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }

        void Service.IServiceCallback.MsgCallback(string msg)
        {
            listBoxMessages.Items.Add(msg);
            listBoxMessages.ScrollIntoView(listBoxMessages.Items[listBoxMessages.Items.Count - 1]);
        }
    }
}
