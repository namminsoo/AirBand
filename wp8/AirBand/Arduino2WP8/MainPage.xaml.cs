using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Reactive;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Threading;
using System.Windows.Navigation;
using System.Windows.Media;

using Windows.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.MobileServices;
using System.IO.IsolatedStorage;

using Windows.Devices.Geolocation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Windows.Threading;
using Microsoft.Phone.UserData;
using System.Collections.Generic;
using Arduino2WP8.Bluetooth;
using System.Windows.Media.Animation;
using System.Windows.Input;
using Windows.Phone.Speech.Synthesis;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Tasks;

namespace Arduino2WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;
        public static StorageFile storageFile = null;
        Geolocator geolocator = new Geolocator();
        Geoposition geoposition;
        DispatcherTimer timer;
        private IMobileServiceTable<EventLog> logTable = App.MobileService.GetTable<EventLog>();
        public static string str = "";

        Contacts cons;
        StackPanel[] sp;
        Ellipse[] ellipse;
        TextBlock[] textBlock;
        TextBlock[] textBlock_num;
        BitmapImage bm;

        List<int> list = new List<int>();
        String[] numArr = new String[5];// 자기번호, 보험사, 응급1, 응급2, 응급3 
        int contactsSize = 0;
        MyGps myGps;

        //public const int NumberOfContacts = 3;


        public bool bluetoothStatus = false; //블루투스 상태를 체크해줄 메서드
        public int saveIndex = 999;

        // Let's Define what we'll use throughout our App
        StreamSocket BTSock; // Socket used to Communicate with the Arduino/BT Module
        /* This is used for our Speech App
         * SpeechRecognizer mySpeech; // Used for Recognizing App Speech (not yet in this App)
         * SpeechSynthesizer mySpeechSS = new SpeechSynthesizer();
         * 
         * This is used to Grab Content from the Net, we'll be using GZIP in the end
         * WebClient wc = new WebClient(); // Setting up our WebClient so we can just use "wc" and could be used to get an API
        */

        // Let's Store our Strings
        string BTStatus = ""; // Used to Store if we can send Message (e.g. yes or no)
                              /*string BT_Received = ""; // We'll use to store Bluetooth Received Data
                              string whattosay = ""; // Used later to accept input for Speech */



        PhoneNumberChooserTask phoneNumberChooserTask;

        // Constructor
        public MainPage()
        {
            InitializeComponent(); // NEVER REMOVE!
            myGps = new MyGps();



            phoneNumberChooserTask = new PhoneNumberChooserTask();
            phoneNumberChooserTask.Completed += new EventHandler<PhoneNumberResult>(phoneNumberChooserTask_Completed);



            /*
            airBandBluetooth = new AirBandBluetooth();
             // Grid.SetRow(listBox, 1); //메인페이지에 비상연락망이 표현될 listBox Set
             if (airBandBluetooth.deviceAvailableCheck() == false)
             {
                 MessageBox.Show("Sorry, Bluetooth isn't compatible in this enviornment. Please use your Phone.", "Error: Device Required", MessageBoxButton.OK);
                 return; // Close
             }
             Loaded += airBandBluetooth.MainPage_Loaded; // We need Async, so Use _Loaded
             // PeerFinder.TriggeredConnectionStateChanged += PeerFinder_TriggeredConnectionStateChanged; // Check Connection State
             Loaded += airBandBluetooth.ReceiveMessage;
             */

            // Let's make sure the Emulator isn't loaded...
            if (Microsoft.Devices.Environment.DeviceType == Microsoft.Devices.DeviceType.Emulator)
            {
                // Send an Error Message to User
                MessageBox.Show("Sorry, Bluetooth isn't compatible in this enviornment. Please use your Phone.", "Error: Device Required", MessageBoxButton.OK);
                return; // Close
            }

            Loaded += MainPage_Loaded; // We need Async, so Use _Loaded
            // PeerFinder.TriggeredConnectionStateChanged += PeerFinder_TriggeredConnectionStateChanged; // Check Connection State
            Loaded += ReceiveMessage;


        }



        void phoneNumberChooserTask_Completed(object sender, PhoneNumberResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                MessageBox.Show("The phone number for " + e.DisplayName + " is " + e.PhoneNumber);

                //Code to start a new call using the retrieved phone number.
                //PhoneCallTask phoneCallTask = new PhoneCallTask();
                //phoneCallTask.DisplayName = e.DisplayName;
                //phoneCallTask.PhoneNumber = e.PhoneNumber;
                //phoneCallTask.Show();
            }
        }



        private async void ReceiveMessage(object sender, RoutedEventArgs e)
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
                        readText.Text = "Arduino : " + str;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
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
                    saveIndex = i;

            }
            if (peers.Count <= 2)
            {
            }

        }



        private void Make_timer()
        {
            try
            {
                if (timer == null)
                {
                    timer = new DispatcherTimer();
                    timer.Interval = new TimeSpan(00, 0, 2);
                    timer.Tick += timer_Tick;
                }
                timer.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            // bool enabled = timer.IsEnabled;
        }

        async void timer_Tick(object sender, object e)
        {
            //실행
            byte[] bytes = new byte[128];
            //    await airBandBluetooth.BTSock.InputStream.ReadAsync(bytes.AsBuffer(), (uint)bytes.Length, InputStreamOptions.Partial);
            await BTSock.InputStream.ReadAsync(bytes.AsBuffer(), (uint)bytes.Length, InputStreamOptions.Partial);

            bytes = bytes.TakeWhile((v, index) => bytes.Skip(index).Any(w => w != 0x00)).ToArray();
            string str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);


            if (str.Contains("aa"))
            {
                timer.Stop();
                //                readText.Text = "ff";



                string message = "Emergency Notification System";
                //     message += myGps.returnValueAddress();
                string caption = "Warning !!!";
                MessageBoxButton buttons = MessageBoxButton.OKCancel;
                // Show message box
                MessageBoxResult result = MessageBox.Show(message, caption, buttons);


                try
                {
                    geoposition = await geolocator.GetGeopositionAsync(
                          maximumAge: TimeSpan.FromMinutes(5),
                          timeout: TimeSpan.FromSeconds(10)
                         );

                    numNullCheck();

                    var logItem = new EventLog
                    {
                        PhoneNum = numArr[0]
                            ,
                        eventTime = DateTime.Now.AddHours(9)
                            ,
                        EventGpsX = geoposition.Coordinate.Latitude.ToString("0.00000000")
                            ,
                        EventGpsY = geoposition.Coordinate.Longitude.ToString("0.00000000")
                            ,
                        AutoinsuranceNum = numArr[1]
                            ,
                        EmergencyNum1 = numArr[2]
                            ,
                        EmergencyNum2 = numArr[3]
                            ,
                        EmergencyNum3 = numArr[4]
                    };



                     await InsertLogItem(logItem);

                    //NavigationService.Navigate(new Uri("/CameraPage.xaml", UriKind.Relative));

                    // await UpdateLogImg(logItem);
                    //여기서 storageFile을 이용하여 db 업데이트를 해줘야됨.


                }
                //If an error is catch 2 are the main causes: the first is that you forgot to include ID_CAP_LOCATION in your app manifest. 
                //The second is that the user doesn't turned on the Location Services
                catch (Exception ex)
                {
                    string ex_caption = "Transmission failure";
                    string ex_message = ex.Message;
                    MessageBoxButton ex_buttons = MessageBoxButton.OKCancel;
                    // Show message box
                    MessageBoxResult ex_result = MessageBox.Show(ex_message, ex_caption, ex_buttons);
                }
            }
            else
            {


                //readText.Text = "\n" + "arduino " + "\n";
            }
        }

        void numNullCheck()
        {
            try { numArr[0] = setting["myNoKey"].ToString(); }
            catch (Exception e) { }
            try { numArr[1] = setting["autoInsuranceNoKey"].ToString(); }
            catch (Exception e) { }


            //   MessageBox.Show(numArr[0] + "\n" + numArr[1]);
            /*
            try { numArr[2] = setting["name1"].ToString(); }
            catch (Exception e) { }
            try { numArr[3] = setting["name2"].ToString(); }
            catch (Exception e) { }
            try { numArr[4] = setting["name3"].ToString(); }
            catch (Exception e) { }
            */

            for (int i = 0; i < numArr.Count(); i++)
            {
                if (numArr[i] == null)
                {
                    numArr[i] = "no data";
                }
            }
        }

        //=민수 끝=================================================================================================================================================================

        private async Task InsertLogItem(EventLog logItem)
        {
            string errorString = string.Empty;
            await logTable.InsertAsync(logItem);
        }

        private async Task UpdateLogImg(EventLog logItem)
        {
            string errorString = string.Empty;

            //사진을 사용 할 때
            if (storageFile != null)
            {
                // Set blob properties of TodoItem.
                logItem.ContainerName = "eventlogimages";

                // Use a unigue GUID to avoid collisions.
                logItem.ResourceName = Guid.NewGuid().ToString();

                // Send the item to be inserted. When blob properties are set this
                //  generates an SAS in the response.
                await logTable.UpdateAsync(logItem);
            }

            // 사진 사용 할 때
            //If we have a returned SAS, then upload the blob.
            if (!string.IsNullOrEmpty(logItem.SasQueryString))
            {
                // Get the URI generated that contains the SAS 
                // and extract the storage credentials.
                StorageCredentials cred = new StorageCredentials(logItem.SasQueryString);
                var imageUri = new Uri(logItem.ImageUri);

                // Instantiate a Blob store container based on the info in the returned item.
                CloudBlobContainer container = new CloudBlobContainer(
                    new Uri(string.Format("https://{0}/{1}",
                        imageUri.Host, logItem.ContainerName)), cred);

                // Get the new image as a stream.
                using (var inputStream = await storageFile.OpenReadAsync())
                {
                    // Upload the new image as a BLOB from the stream.
                    CloudBlockBlob blobFromSASCredential =
                        container.GetBlockBlobReference(logItem.ResourceName);
                    await blobFromSASCredential.UploadFromStreamAsync((System.IO.Stream)inputStream);
                }

                // When you request an SAS at the container-level instead of the blob-level,
                // you are able to upload multiple streams using the same container credentials.
            }
        }

        //=민수=================================================================================================================================================================

        //=상욱=================================================================================================================================================================

        public void checkEmergencyContacts(object sender, ContactsSearchEventArgs e)  //메인페이지에 비상연락망을 표시하기 위한 메서드
        {
            int listSize = 0;
            try
            {
                contactsSize = e.Results.Count();
                listBox.Items.Clear();
                list.Clear();

                for (int i = 0; i < contactsSize; i++)
                {
                    if (setting.Contains("name" + i.ToString()))
                    {
                        list.Add(i);
                    }
                }
                listSize = list.Count();

                numNullCheck(); // for numArr[0~1]

                for (int i = 0; i < listSize; i++)
                {
                    try
                    {
                        int temp = list.ElementAt(i);
                        string tempNum = Convert.ToString(temp);
                        numArr[i + 2] = setting["phoneNumber" + tempNum].ToString();
                    }
                    catch (Exception exce)
                    {
                        // MessageBox.Show(exce.ToString());
                    }
                }
            }
            catch (System.Exception)
            {
                //No results
            }
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (setting.Contains("mainFlag"))
            {
                if (setting["mainFlag"].Equals("check"))
                {
                    cons = new Contacts();
                    //Identify the method that runs after the asynchronous search completes.
                    //cons.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(Contacts_SearchCompleted);
                    cons.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(checkEmergencyContacts);
                    //Start the asynchronous search.
                    cons.SearchAsync(String.Empty, FilterKind.None, "Contacts Test #1");
                }
            }
            else
            {
            }
        }

        // FUNCTION PROVIDED BY SPHERO
        private IBuffer GetBufferFromByteArray(byte[] package)
        {




            bluetoothStatus = true;
            Make_timer();


            using (DataWriter dw = new DataWriter())
            {
                dw.WriteBytes(package);
                return dw.DetachBuffer();
            }
        }

        private async void Airband_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (bluetoothStatus == false)
            {
                if (setting.Contains("mainFlag"))
                {
                    if (setting["mainFlag"].Equals("check"))
                    {
                        try
                        {

                            sb.Begin();

                            //    Thread.Sleep(1000);
                            BitmapImage bm = new BitmapImage(new Uri("/Assets/Images/main_images/stop.png", UriKind.RelativeOrAbsolute));
                            Airband.Source = bm;
                            //    airBandBluetooth.connectedStatus();

                            PeerFinder.AlternateIdentities["Bluetooth:Paired"] = ""; // Grab Paired Devices
                            var PF = await PeerFinder.FindAllPeersAsync(); // Store Paired Devices
                            BTSock = new StreamSocket(); // Create a new Socket Connection
                            await BTSock.ConnectAsync(PF[saveIndex].HostName, "1"); // Connect using Socket to Selected Item



                            SpeechSynthesizer synth = new SpeechSynthesizer();

                            await synth.SpeakTextAsync("Connected.");

                            // Once Connected, let's give Arduino a HELLO
                            var datab = GetBufferFromByteArray(Encoding.UTF8.GetBytes("HELLO")); // Create Buffer/Packet for Sending
                            await BTSock.OutputStream.WriteAsync(datab); // Send Arduino Buffer/Packet Message




                            /*
                            if (setting.Contains("bluetoothFlag"))
                            {
                                if (setting["bluetoothFlag"].Equals("1"))
                                {
                                    int save = airBandBluetooth.saveIndex;
                                    //setting["statuscheck"] = "connect"; //연결이 되면 격리저장소에 statuscheck라는 키 값으로 상태 저장
                                    bluetoothStatus = true;
                                }
                            }

                            */
                        }
                        catch (Exception ex)
                        {
                            //  MessageBox.Show("Connection is failure.");
                        }

                        // 블루투스가 연결되면 애저 서버로 시그널을 보내기 시작
                        try
                        {


                            if (bluetoothStatus)
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Make_timer_error");
                        }
                    }   //  if (setting["mainFlag"].Equals("check"))
                }   // if (setting.Contains("mainFlag"))
                else
                {
                    string message = "Do you want to go to the Settings screen?";
                    string caption = "Setting";
                    MessageBoxButton buttons = MessageBoxButton.OKCancel;
                    // Show message box
                    MessageBoxResult result = MessageBox.Show(message, caption, buttons);
                    if (MessageBoxResult.OK == result)
                    {
                        NavigationService.Navigate(new Uri("/MyContacts.xaml", UriKind.Relative));
                    }
                }
            } // if(bluetoothStatus == false)

            else if (bluetoothStatus == true)//블루투스가 연결되어 있으면 버튼을 눌를때 연결을 해제한다.
            {
                if (setting.Contains("bluetoothFlag"))
                {
                    setting.Remove("bluetoothFlag");
                    bluetoothStatus = false;
                }
            }
        }

        private void set_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MyContacts.xaml", UriKind.Relative));
            //Windows.System.Launcher.LaunchUriAsync(new Uri("ms-people:", UriKind.RelativeOrAbsolute));
        }

        private void bletooth_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/MainMenu/MyBluetoothList.xaml", UriKind.Relative));
            Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:", UriKind.RelativeOrAbsolute));
        }


        private void contacts_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //string Name = "TimeAgent";
            //RemoveTask(Name);

            //PeriodicTask task = new PeriodicTask(Name);
            //task.Description = "현재 시간을 알려줍니다.";

            //try
            //{
            //    ScheduledActionService.Add(task);
            //    ScheduledActionService.LaunchForTest(Name, TimeSpan.FromSeconds(60));
            //    MessageBox.Show("단기 작업이 등록되었습니다. 이 프로그램은 종료하십시오.");

            //}
            //catch (Exception)
            //{

            //}

            //phoneNumberChooserTask.Show();
            Windows.System.Launcher.LaunchUriAsync(new Uri("ms-people:", UriKind.RelativeOrAbsolute));


        }

        void RemoveTask(string name)
        {
            PeriodicTask task = (PeriodicTask)ScheduledActionService.Find(name);
            if (task != null)
            {
                ScheduledActionService.Remove(name);
            }
        }


        private void gps_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MyLocation.xaml", UriKind.Relative));
            //RemoveTask("TimeAgent");
            //MessageBox.Show("단기 작업이 해제되었습니다.");

        }


        // 접속 해제
        private async void disconnectBtn_Click(object sender, EventArgs e)
        {


            if (bluetoothStatus == true)
            {
                BTSock.Dispose();
                BitmapImage bm = new BitmapImage(new Uri("/Assets/Images/main_images/start.png", UriKind.RelativeOrAbsolute));
                Airband.Source = bm;
                SpeechSynthesizer synth = new SpeechSynthesizer();
                await synth.SpeakTextAsync("Disconnected..");
                bluetoothStatus = false;
            }
        }
        //=상욱=================================================================================================================================================================
    } //end of public partial class MainPage : PhoneApplicationPage
} //end of namespace Arduino2WP8

