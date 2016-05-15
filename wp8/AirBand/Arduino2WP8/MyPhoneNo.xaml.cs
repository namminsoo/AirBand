using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.UserData;
using System.IO.IsolatedStorage;
using System.IO;
using System.Text.RegularExpressions;

namespace Arduino2WP8
{
    public partial class MyPhoneNo : PhoneApplicationPage
    {
        IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;

        //List<UserData> everynames = new List<UserData>();
        IEnumerable<Contact> contacts;
        int flag1, flag2, flag3 = 0;

        public MyPhoneNo()
        {

            InitializeComponent();

            if (setting.Contains("myNoKey"))
            {
                join.Text = "번호 수정하기";
                autoInsuranceNo.Text = setting["autoInsuranceNoKey"].ToString();
                emergencyNo1.Text = setting["emergencyKey1"].ToString();
                emergencyNo2.Text = setting["emergencyKey2"].ToString();
                emergencyNo3.Text = setting["emergencyKey3"].ToString();
            }

            Contacts cons = new Contacts();
            //Identify the method that runs after the asynchronous search completes.
            cons.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(Contacts_SearchCompleted);
            //Start the asynchronous search.
            cons.SearchAsync(String.Empty, FilterKind.None, "Contacts Test #1");
            ContactResultsData.IsEnabled = false;
        }

        private void SearchNo_Click(object sender, RoutedEventArgs e)
        {
            ContactResultsData.IsEnabled = true;

        }

        public void Contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            contacts = e.Results;
            try
            {
                //Bind the results to the user interface.
                ContactResultsData.DataContext = e.Results;

            }
            catch (System.Exception)
            {
                //No results
            }

        }

        private void ContactResults_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            string phoneNo = null;
            //string name = null;
            foreach (var item in contacts)
            {
                if (clickedButton.Content.ToString() == item.DisplayName)
                {
                    // name = item.DisplayName;
                    phoneNo = (item.PhoneNumbers.Count() > 0 ? (item.PhoneNumbers.FirstOrDefault()).PhoneNumber : "");
                }
            }
            if (flag1 == 1)
            {
                //setting["name1"] = name.ToString();
                emergencyNo1.Text = phoneNo.ToString();
                flag1 = 0;
            }
            else if (flag2 == 1)
            {
                // setting["name2"] = name.ToString();
                emergencyNo2.Text = phoneNo.ToString();
                flag2 = 0;
            }
            else if (flag3 == 1)
            {
                // setting["name3"] = name.ToString();
                emergencyNo3.Text = phoneNo.ToString();
                flag3 = 0;
            }
            //searchNo.IsEnabled = false;
            ContactResultsData.IsEnabled = false;
        }

        private void emergencyNo1_Click(object sender, RoutedEventArgs e)
        {
            //searchNo.IsEnabled = true;
            flag1 = 1;
            ContactResultsData.IsEnabled = true;
        }
        private void emergencyNo2_Click(object sender, RoutedEventArgs e)
        {
            //searchNo.IsEnabled = true;
            flag2 = 1;
            ContactResultsData.IsEnabled = true;
        }

        private void emergencyNo3_Click(object sender, RoutedEventArgs e)
        {
            //searchNo.IsEnabled = true;
            flag3 = 1;
            ContactResultsData.IsEnabled = true;
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            Regex reg = new Regex(@"^(\d)+$");
            if (!reg.IsMatch(autoInsuranceNo.Text))
            {
                MessageBox.Show("Only Number");
            }
            else if (!reg.IsMatch(emergencyNo1.Text))
            {
                MessageBox.Show("Only Number");
            }
            else if (!reg.IsMatch(emergencyNo2.Text))
            {
                MessageBox.Show("Only Number");
            }
            else if (!reg.IsMatch(emergencyNo3.Text))
            {
                MessageBox.Show("Only Number");
            }
            else if (autoInsuranceNo.Text != "" && emergencyNo1.Text != "" && emergencyNo2.Text != "" && emergencyNo3.Text != "")
            {
                string phoneNo = null;

                setting["autoInsuranceNoKey"] = autoInsuranceNo.Text;
                setting["emergencyKey1"] = emergencyNo1.Text;
                setting["emergencyKey2"] = emergencyNo2.Text;
                setting["emergencyKey3"] = emergencyNo3.Text;

                foreach (var item in contacts)
                {
                    phoneNo = (item.PhoneNumbers.Count() > 0 ? (item.PhoneNumbers.FirstOrDefault()).PhoneNumber : "");
                    if (emergencyNo1.Text == phoneNo)
                    {
                        setting["name1"] = item.DisplayName;
                    }
                    else if (emergencyNo2.Text == phoneNo)
                    {
                        setting["name2"] = item.DisplayName;
                    }
                    else if (emergencyNo3.Text == phoneNo)
                    {
                        setting["name3"] = item.DisplayName;
                    }
                }

                if (!setting.Contains("name1"))
                {
                    setting["name1"] = emergencyNo1.Text;
                }
                if (!setting.Contains("name2"))
                {
                    setting["name2"] = emergencyNo2.Text;
                }
                if (!setting.Contains("name3"))
                {
                    setting["name3"] = emergencyNo3.Text;
                }


                setting["mainFlag"] = "check";

                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));


            }
            else
            {
                MessageBox.Show("Please enter all details");
            }

        }

        private void direct_Click1(Object sender, RoutedEventArgs e)
        {
            //searchNo.IsEnabled = true;
            ContactResultsData.IsEnabled = true;
        }
        private void direct_Click2(Object sender, RoutedEventArgs e)
        {
            //searchNo.IsEnabled = true;
            flag1 = 1;
            ContactResultsData.IsEnabled = true;
        }
        private void direct_Click3(Object sender, RoutedEventArgs e)
        {
            //searchNo.IsEnabled = true;
            flag2 = 1;
            ContactResultsData.IsEnabled = true;
        }
        private void direct_Click4(Object sender, RoutedEventArgs e)
        {
            //searchNo.IsEnabled = true;
            flag3 = 1;
            ContactResultsData.IsEnabled = true;
        }






    }
}