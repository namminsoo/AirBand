using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Devices.Geolocation;
using Newtonsoft.Json;

namespace Arduino2WP8
{
    public partial class MyGps : PhoneApplicationPage
    {

        private string address = "";
        GoogleGeoCodeResponse rootObject;

        public string Address
        {
            get
            {
                return address;
            }

            set
            {
                address = value;
            }
        }

        public MyGps()
        {
            InitializeComponent();

            //WebClient webClient = new WebClient();
            //webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            //webClient.DownloadStringAsync(new Uri("http://maps.googleapis.com/maps/api/geocode/json?sensor=false&language=ko&latlng=37.55170483,127.07356353")); 
        }


        public class GoogleGeoCodeResponse
        {

            public string status { get; set; }
            public results[] results { get; set; }

        }

        public class results
        {
            public string formatted_address { get; set; }
            public geometry geometry { get; set; }
            public string[] types { get; set; }
            public address_component[] address_components { get; set; }
        }

        public class geometry
        {
            public string location_type { get; set; }
            public location location { get; set; }
        }

        public class location
        {
            public string lat { get; set; }
            public string lng { get; set; }
        }

        public class address_component
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public string[] types { get; set; }
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

             rootObject = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(e.Result);
            /*
           string address = "";

           Address = rootObject.results.ElementAt(0).formatted_address.ToString();
           */
        }

        public string returnValueAddress()
        {
            address = rootObject.results.ElementAt(0).formatted_address.ToString();
            return address;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                     maximumAge: TimeSpan.FromMinutes(5),
                     timeout: TimeSpan.FromSeconds(10)
                    );

                //With this 2 lines of code, the app is able to write on a Text Label the Latitude and the Longitude, given by {{Icode|geoposition}}
                geolocation.Text = "GPS:" + geoposition.Coordinate.Latitude.ToString("0.00000000") + ", " + geoposition.Coordinate.Longitude.ToString("0.00000000");
            }
            //If an error is catch 2 are the main causes: the first is that you forgot to include ID_CAP_LOCATION in your app manifest. 
            //The second is that the user doesn't turned on the Location Services
            catch (Exception ex)
            {
                //exception
            }
        }

    }
}