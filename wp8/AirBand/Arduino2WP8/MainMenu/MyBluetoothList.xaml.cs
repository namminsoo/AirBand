using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using Windows.Networking.Proximity;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Arduino2WP8.MainMenu
{
    public partial class MyBluetoothList : PhoneApplicationPage
    {

        StackPanel[] sp;
        TextBlock[] textBlock;
        Image[] sel_image;
        BitmapImage image;
        ImageBrush brush1, brush2;
         

        public MyBluetoothList()
        {
            InitializeComponent();

            image = new BitmapImage(new Uri("/Assets/Images/bluetooth/sp1.png", UriKind.Relative));
            brush1 = new ImageBrush();
            brush1.ImageSource = image;
            image = new BitmapImage(new Uri("/Assets/Images/bluetooth/sp2.png", UriKind.Relative));
            brush2 = new ImageBrush();
            brush2.ImageSource = image;
            Loaded += MainPage_Loaded;


        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            PeerFinder.Start();
            PeerFinder.AlternateIdentities["Bluetooth:Paired"] = ""; // Find/Get All Paired BT Devices
            var peers = await PeerFinder.FindAllPeersAsync(); // Make peers the container for All BT Devices

            // Only want 1 Device to Show? Uncomment Below
            // lstBTPaired.Items.Add(peers[0].DisplayName); // 1 Paired Device to Show 

            // Show only Specific Device
            // peers[0].DisplayName.Contains("RN42-5");

            // Let's show only the first 2 Devices Paired

            int peersSize = peers.Count;

            sp = new StackPanel[peersSize];
            for (int i = 0; i < peersSize; i++) { sp[i] = new StackPanel(); }

            textBlock = new TextBlock[peersSize];
            for (int i = 0; i < peersSize; i++) { textBlock[i] = new TextBlock(); }

            sel_image = new Image[peersSize];
            for (int i = 0; i < peersSize; i++) { sel_image[i] = new Image(); }

            for (int i = 0; i < peers.Count; i++)
            {
                sel_image[i].Source = new BitmapImage(new Uri("/Assets/Images/bluetooth/sign.png", UriKind.RelativeOrAbsolute));
                sel_image[i].Margin = new Thickness(250, 0, 25, 0);
                sel_image[i].Width = 30;
                sel_image[i].Height = 30;
                textBlock[i].Text = peers[i].DisplayName;
                setTextTb(textBlock[i]); // 이름 메서드, 이름이 들어갈 텍스트블러의 높이, 넓이 등 지정

                sp[i].Orientation = System.Windows.Controls.Orientation.Horizontal;
                sp[i].Margin = new Thickness(5, 5, 0, 5);
                sp[i].Children.Add(textBlock[i]);
                sp[i].Children.Add(sel_image[i]);



                if (i % 2 == 0)
                {
                    sp[i].Background = brush1;
                }
                else if (i % 2 == 1)
                {
                    sp[i].Background = brush2;
                }
                listBox.Items.Insert(i, sp[i]); // 한명씩 listboxItem 형식으로 추가
            }
            if (peers.Count <= 2)
            {
                //txtBTStatus.Text = "Found " + peers.Count + " Devices";
            }

        }

        public void setTextTb(object obj) // 이름, 폰번호 설정 메서드
        {
            TextBlock textBlock = (TextBlock)obj;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Margin = new Thickness(25, 10, 0, 10);
            textBlock.FontSize = 25;
        }


        private void lstBTPaired_Tap_1(object sender, GestureEventArgs e)
        {
            if (lstBTPaired.SelectedItem == null) // To prevent errors, make sure something is Selected
            {
                //btnConnectArduino.IsEnabled = false; // Make sure it's False if you want to use a Button
                //txtBTStatus.Text = "No Device Selected! Try again..."; // Set UI Output
                return;
            }
            else
                if (lstBTPaired.SelectedItem != null) // Just making sure something was Selected
            {



             
            }
        }

    }
}