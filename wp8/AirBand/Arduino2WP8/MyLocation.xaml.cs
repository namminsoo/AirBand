using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using Windows.Devices.Geolocation;

namespace Arduino2WP8
{
    public partial class MyLocation : PhoneApplicationPage
    {


        private double lat;
        private double lon;

        public MyLocation()
        {
            InitializeComponent();
            Map MyMap = new Map();

            //  http://maps.googleapis.com/maps/api/geocode/json?sensor=false&language=ko&latlng=37.55170483,127.07356353

            mylocation();
        }

      


        public async void mylocation()
        {
            double lat;
            double lon;

            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                     maximumAge: TimeSpan.FromMinutes(5),
                     timeout: TimeSpan.FromSeconds(10)
                    );

                //With this 2 lines of code, the app is able to write on a Text Label the Latitude and the Longitude, given by {{Icode|geoposition}}


                lat = Convert.ToDouble(geoposition.Coordinate.Latitude.ToString("0.00000000"));
                lon = Convert.ToDouble(geoposition.Coordinate.Longitude.ToString("0.00000000"));
                
               MyMap.Center = new GeoCoordinate(37.5255663, 126.9009004);

            }
            //If an error is catch 2 are the main causes: the first is that you forgot to include ID_CAP_LOCATION in your app manifest. 
            //The second is that the user doesn't turned on the Location Services
            catch (Exception ex)
            {
                //exception
            }

           // MyMap.Center = new GeoCoordinate(47.6097, -122.3331);
            MyMap.ZoomLevel = 16;
            MyMap.LandmarksEnabled = true;
            MyMap.PedestrianFeaturesEnabled = true;
            ContentPanel.Children.Add(MyMap);

        }



        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);


            /*
            string msg = "";
            int temp;
            if (NavigationContext.QueryString.TryGetValue("lat", out msg))
            {
                temp = Convert.ToInt32(msg);
                Lat = temp;
            }

            if (NavigationContext.QueryString.TryGetValue("lon", out msg))
            {
                temp = Convert.ToInt32(msg);
                Lon = temp;
            }
            */




        }

        private void back_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        public double Lat
        {
            get
            {
                return lat;
            }

            set
            {
                lat = value;
            }
        }

        public double Lon
        {
            get
            {
                return lon;
            }

            set
            {
                lon = value;
            }
        }

        private void back_Click_1(object sender, EventArgs e)
        {

        }
    }
}