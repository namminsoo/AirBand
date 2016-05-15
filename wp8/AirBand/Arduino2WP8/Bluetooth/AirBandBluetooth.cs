using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Arduino2WP8.Bluetooth
{


    class AirBandBluetooth
    {

        IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;

        public int saveIndex = 999;

        public StreamSocket BTSock; // Socket used to Communicate with the Arduino/BT Module
                                    /* This is used for our Speech App
                                     * SpeechRecognizer mySpeech; // Used for Recognizing App Speech (not yet in this App)
                                     * SpeechSynthesizer mySpeechSS = new SpeechSynthesizer();
                                     * 
                                     * This is used to Grab Content from the Net, we'll be using GZIP in the end
                                     * WebClient wc = new WebClient(); // Setting up our WebClient so we can just use "wc" and could be used to get an API
                                    */

        // Let's Store our Strings
        public string BTStatus = ""; // Used to Store if we can send Message (e.g. yes or no)
                                     /*string BT_Received = ""; // We'll use to store Bluetooth Received Data
                                     string whattosay = ""; // Used later to accept input for Speech */


        public AirBandBluetooth()
        {
            // Let's Define what we'll use throughout our App





        }

        public bool deviceAvailableCheck()
        {

            bool result = false;

            
            // Let's make sure the Emulator isn't loaded...
            if (Microsoft.Devices.Environment.DeviceType == Microsoft.Devices.DeviceType.Emulator)
            {
                return result; // Close
            }
            else
            {
                result = true;
            }


            return result;
        }



        public async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            PeerFinder.Start();
            PeerFinder.AlternateIdentities["Bluetooth:Paired"] = ""; // Find/Get All Paired BT Devices
            var peers = await PeerFinder.FindAllPeersAsync(); // Make peers the container for All BT Devices

            ReceiveMessage(sender, e);

            // Only want 1 Device to Show? Uncomment Below
            // lstBTPaired.Items.Add(peers[0].DisplayName); // 1 Paired Device to Show 

            // Show only Specific Device
            // peers[0].DisplayName.Contains("RN42-5");

            // Let's show only the first 2 Devices Paired


            for (int i = 0; i < peers.Count; i++)
            {
                //lstBTPaired.Items.Add(peers[i].DisplayName);
                if (peers[i].DisplayName.Equals("AIRBAND"))
                {
                    saveIndex = i;

                }


            }
            if (peers.Count <= 2)
            {
            }

        }

        public async void connectedStatus()
        {
            try
            {
                PeerFinder.AlternateIdentities["Bluetooth:Paired"] = ""; // Grab Paired Devices
                var PF = await PeerFinder.FindAllPeersAsync(); // Store Paired Devices
                BTSock = new StreamSocket(); // Create a new Socket Connection
                await BTSock.ConnectAsync(PF[saveIndex].HostName, "1"); // Connect using Socket to Selected Item

                setting["bluetoothFlag"] = "1";



                        //await BTSock.ConnectAsync(PF[saveIndex].HostName, "{00001101-0000-1000-8000-00805f9b34fb}"); // Connect using Socket to Selected Item




                        // Once Connected, let's give Arduino a HELLO


                        var datab = GetBufferFromByteArray(Encoding.UTF8.GetBytes("HELLO")); // Create Buffer/Packet for Sending
                await BTSock.OutputStream.WriteAsync(datab); // Send Arduino Buffer/Packet Message
                
            }
            catch (Exception e)
            {

            }

        }

        public async void ReceiveMessage(object sender, RoutedEventArgs e)
        {
            try
            {
                while (true)
                {

                    if (BTSock == null) // If we don't have a connection, Send Error Control
                    {
                        // MessageBox.Show("Please connect to a device first."); // Alert the user with a Notification (Optional)
                        //  readText.Text = "No connection found. Try again!"; // Alert the UI
                        return; // Stop
                    }
                    else
                        if (BTSock != null) // Since we have a Connection
                    {

                        byte[] bytes = new byte[128];
                        await BTSock.InputStream.ReadAsync(bytes.AsBuffer(), (uint)bytes.Length, InputStreamOptions.Partial);
                        bytes = bytes.TakeWhile((v, index) => bytes.Skip(index).Any(w => w != 0x00)).ToArray();
                        string str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                        MainPage.str = "Arduino : " + str;

                        //readText.Text = "Arduino : " + str;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void disconnectFuc()
        {
            try
            {
                BTSock.Dispose();

            }
            catch (Exception e)
            {

            }
        }
        
        // FUNCTION PROVIDED BY SPHERO
        private IBuffer GetBufferFromByteArray(byte[] package)
        {
            using (DataWriter dw = new DataWriter())
            {
                dw.WriteBytes(package);
                return dw.DetachBuffer();
            }
        }

        /*


                void PeerFinder_TriggeredConnectionStateChanged(object sender, TriggeredConnectionStateChangedEventArgs args)
        {
            // This will be used to Get Data from our Hardware soon

            if (args.State == TriggeredConnectState.Failed)
            {
                BTStatus = "no"; // Not connected
                return;
            }

            if (args.State == TriggeredConnectState.Completed)
            {
                BTStatus = "yes"; // Means we are connected

                MessageBox.Show("hi");
            }
        }




        private async void BT2Arduino_Send(string WhatToSend)
        {
            if (BTSock == null) // If we don't have a connection, Send Error Control
            {
                // MessageBox.Show("Please connect to a device first."); // Alert the user with a Notification (Optional)
                return; // Stop
            }
            else
                if (BTSock != null) // Since we have a Connection
            {
                var datab = GetBufferFromByteArray(UTF8Encoding.UTF8.GetBytes(WhatToSend)); // Create Buffer/Packet for Sending
                await BTSock.OutputStream.WriteAsync(datab); // Send our Message to Connected Arduino

            }
        }



        private async void BT2Arduino_Read()
        {
            if (BTSock == null) // If we don't have a connection, Send Error Control
            {
                // MessageBox.Show("Please connect to a device first."); // Alert the user with a Notification (Optional)

                return; // Stop
            }
            else
                if (BTSock != null) // Since we have a Connection
            {

                IBuffer buffer = new byte[1024].AsBuffer();
                await BTSock.InputStream.ReadAsync(buffer, buffer.Capacity, InputStreamOptions.Partial);

                string test = buffer.ToString();

                readText.Text = "Arduino : " + test;
            }

        }


              private void btnStatusControl(bool flag)
        {
            if (flag == true) //블루투스 연결상태
            {
                disconnectBtn.IsEnabled = flag;
                readyBtn.Content = "Connected";
                readyBtn.IsEnabled = false; // Allow commands to be sent via ready Button (Enabled)
                readyBtn.Background = new SolidColorBrush(Colors.Blue);
            }
            else if (flag == false) //블루투스 연결 해제 상태
            {
                disconnectBtn.IsEnabled = flag;
                readyBtn.IsEnabled = true; // Allow commands to be sent via ready Button (Enabled)
                readyBtn.Background = new SolidColorBrush(Colors.Black);
            }
        }

        */

    }
}
