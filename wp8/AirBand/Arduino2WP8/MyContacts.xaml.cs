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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.Windows.Media;


using Windows.UI;

namespace Arduino2WP8
{
    public partial class MyContacts : PhoneApplicationPage
    {
        IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;
        IEnumerable<Contact> contacts;


        ContactsInfo[] contactsInfo;
        StackPanel[] sp;
        Ellipse[] ellipse;
        TextBlock[] textBlock;
        Image[] chk_image;
        Image[] sms_image;
        Image[] call_image;

        BitmapImage bm;
        Contacts cons;


        int contactsSize = 0;

        public MyContacts()
        {
            InitializeComponent();


            cons = new Contacts();
            //Identify the method that runs after the asynchronous search completes.
            //cons.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(Contacts_SearchCompleted);

            cons.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(InitEmergencyContacts);
            //Start the asynchronous search.
            cons.SearchAsync(String.Empty, FilterKind.None, "Contacts Test #1");
        }


        public void InitEmergencyContacts(object sender, ContactsSearchEventArgs e)  //메인페이지에 비상연락망을 표시하기 위한 메서드
        {
            contacts = e.Results;
            try
            {
                //Bind the results to the user interface.
                //ContactResultsData.DataContext = e.Results;

                listBox.DataContext = e.Results;
                contactsSize = e.Results.Count();

                if (contactsSize == 0)
                {
                    MessageBox.Show("Contacts is empty.");
                    return;
                }

                sp = new StackPanel[contactsSize];
                for (int i = 0; i < contactsSize; i++) { sp[i] = new StackPanel(); }

                chk_image = new Image[contactsSize];
                for (int i = 0; i < contactsSize; i++) { chk_image[i] = new Image(); }

                ellipse = new Ellipse[contactsSize];
                for (int i = 0; i < contactsSize; i++) { ellipse[i] = new Ellipse(); }

                textBlock = new TextBlock[contactsSize];
                for (int i = 0; i < contactsSize; i++) { textBlock[i] = new TextBlock(); }

                sms_image = new Image[contactsSize];
                for (int i = 0; i < contactsSize; i++) { sms_image[i] = new Image(); }

                call_image = new Image[contactsSize];
                for (int i = 0; i < contactsSize; i++) { call_image[i] = new Image(); }

                for (int i = 0; i < contactsSize; i++)
                {
                    sp[i].Orientation = System.Windows.Controls.Orientation.Horizontal;
                    sp[i].Margin = new Thickness(5, 5, 0, 5);
                    sp[i].Children.Add(chk_image[i]);
                    sp[i].Children.Add(ellipse[i]);
                    sp[i].Children.Add(textBlock[i]);
                    sp[i].Children.Add(call_image[i]);
                    sp[i].Children.Add(sms_image[i]);

                    setEllipse(ellipse[i]); // 사진 설정 메서드, 사진이 들어갈 타원의 높이, 넓이 등 지정
                    setTextTb(textBlock[i]); // 이름 메서드, 이름이 들어갈 텍스트블러의 높이, 넓이 등 지정


                    listBox.Items.Insert(i, sp[i]); // 한명씩 listboxItem 형식으로 추가

                    call_image[i].Width = 25;
                    call_image[i].Height = 25;
                    call_image[i].Margin = new Thickness(150, 0, 0, 0);
                    sms_image[i].Width = 25;
                    sms_image[i].Height = 25;
                    sms_image[i].Margin = new Thickness(25, 0, 0, 0);

                }

            }
            catch (System.Exception)
            {
                //No results
            }

            contactsInfo = new ContactsInfo[contactsSize];
            {

            }
            contactsSize = e.Results.Count();
            for (int i = 0; i < contactsSize; i++)
            {

                if (setting.Contains("name" + i.ToString()))
                {
                    call_image[i].Source = new BitmapImage(new Uri("/Assets/Images/contacts_images/call_sel.png", UriKind.RelativeOrAbsolute));
                    sms_image[i].Source = new BitmapImage(new Uri("/Assets/Images/contacts_images/sms_sel.png", UriKind.RelativeOrAbsolute));
                    chk_image[i].Source = new BitmapImage(new Uri("/Assets/Images/contacts_images/sign.png", UriKind.RelativeOrAbsolute));
                    chk_image[i].Width = 25;
                    chk_image[i].Height = 25;
                    chk_image[i].Margin = new Thickness(0, 0, 25, 0);
                    ellipse[i].Margin = new Thickness(0, 0, 10, 0);
                 }
                else
                {

                    call_image[i].Source = new BitmapImage(new Uri("/Assets/Images/contacts_images/call.png", UriKind.RelativeOrAbsolute));
                    sms_image[i].Source = new BitmapImage(new Uri("/Assets/Images/contacts_images/sms.png", UriKind.RelativeOrAbsolute));
                    ellipse[i].Margin = new Thickness(50, 0, 10, 0);
                }

                //call_image[i].


                //프로필 사진 넣는 부분 
                BitmapImage img = new BitmapImage();
                System.IO.Stream tempStream = e.Results.ElementAt(i).GetPicture();

                if (tempStream == null)
                {
                    ellipse[i].Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 155, 155, 155));
                }
                else
                {
                    img.SetSource(tempStream);
                    ImageBrush myBrush = new ImageBrush();
                    myBrush.ImageSource = img;

                    ellipse[i].Fill = myBrush;
                }

                textBlock[i].Text = e.Results.ElementAt(i).DisplayName; //이름 넣는 부분
                try
                {
                    //  textBlock_num[i].Text = e.Results.ElementAt(i).PhoneNumbers.FirstOrDefault().PhoneNumber; //전화번호 넣어주는 부분
                    contactsInfo[i] = new ContactsInfo(i, e.Results.ElementAt(i).DisplayName, e.Results.ElementAt(i).PhoneNumbers.FirstOrDefault().PhoneNumber, ellipse[i]);





                }
                catch (Exception ex)
                {
                }


            }
            /*                Contacts에서 연락망을 추가해서 표시할 부분 시작                                */
        }

        private void setEllipse(object obj) // 사진 설정 메서드
        {
            Ellipse ellipse = (Ellipse)obj;
            ellipse.VerticalAlignment = VerticalAlignment.Center;
            ellipse.Width = 60;
            ellipse.Height = 60;
        }

        public void setTextTb(object obj) // 이름, 폰번호 설정 메서드
        {
            TextBlock textBlock = (TextBlock)obj;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Margin = new Thickness(10, 10, 0, 10);
            textBlock.FontSize = 25;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ListBox sitem = (ListBox)sender;
            int index = sitem.SelectedIndex;
            string message;
            string caption;
            int count = 0;
            //    MessageBox.Show(contactsInfo[index].Index + " / " + contactsInfo[index].Name + " / " + contactsInfo[index].PhoneNumber + " / ");

            if (setting.Contains("name" + index.ToString()))
            {
                message = "Remove the emergency contact number registered to.";
                caption = "Remove";
                MessageBoxButton buttons = MessageBoxButton.OKCancel;
                // Show message box
                MessageBoxResult result = MessageBox.Show(message, caption, buttons);

                if (MessageBoxResult.OK == result)
                {
                    contactImageConverter(index, false); //선택하지 않은 연락처의 이미지를 변경해주는 메서드
                    ellipse[index].Margin = new Thickness(0, 0, 10, 0);
                    setting.Remove("index" + index.ToString());
                    setting.Remove("name" + index.ToString());
                    setting.Remove("phoneNumber" + index.ToString());
                    setting.Save();
                }
                else
                {
             //       chkBox[index].IsChecked = true;
                }
            }

            else {
                message = "Would you like to save emergency contact?";
                caption = "Register";
                MessageBoxButton buttons = MessageBoxButton.OKCancel;
                // Show message box
                MessageBoxResult result = MessageBox.Show(message, caption, buttons);

                if (MessageBoxResult.OK == result)
                {

                    contactImageConverter(index, true); //선택한 연락처의 이미지를 변경해주는 메서드

                    ellipse[index].Margin = new Thickness(0, 0, 10, 0);
                    setting["index" + index.ToString()] = contactsInfo[index].Index.ToString();
                    setting["name" + index.ToString()] = contactsInfo[index].Name.ToString();
                    setting["phoneNumber" + index.ToString()] = contactsInfo[index].PhoneNumber.ToString();
                    setting["mainFlag"] = "check";
                    setting.Save();
                }
                else
                {
              //      chkBox[index].IsChecked = false;
                }
            }
        }



        private void contactImageConverter(int index, bool flag) // 연락처 선택 유뮤를 구분해줄 메서드
        {
            if(flag == true) // 선택한 연락처의 이미지를 변경해주는 부분
            {

                call_image[index].Source = new BitmapImage(new Uri("/Assets/Images/contacts_images/call_sel.png", UriKind.RelativeOrAbsolute));
                sms_image[index].Source = new BitmapImage(new Uri("/Assets/Images/contacts_images/sms_sel.png", UriKind.RelativeOrAbsolute));
                chk_image[index].Source = new BitmapImage(new Uri("/Assets/Images/contacts_images/sign.png", UriKind.RelativeOrAbsolute));
                chk_image[index].Width = 25;
                chk_image[index].Height = 25;
                chk_image[index].Margin = new Thickness(0, 0, 25, 0);
            }
            else if(flag == false) // 선택하지 않은 연락처의 이미지를 변경해주는 부분
            {
                call_image[index].Source = new BitmapImage(new Uri("/Assets/Images/contacts_images/call.png", UriKind.RelativeOrAbsolute));
                sms_image[index].Source = new BitmapImage(new Uri("/Assets/Images/contacts_images/sms.png", UriKind.RelativeOrAbsolute));
                chk_image[index].Source = null;
            }


        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void myNameTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (myNameTb.Text == "남민수")
            {
                myNameTb.Text = "";
                SolidColorBrush Brush1 = new SolidColorBrush();
                Brush1.Color = Colors.Gray;
                myNameTb.Foreground = Brush1;
            }
        }

        private void myNameTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (myNameTb.Text == String.Empty)
            {
                myNameTb.Text = "남민수";
                SolidColorBrush Brush2 = new SolidColorBrush();
                Brush2.Color = Colors.Blue;
                myNameTb.Foreground = Brush2;
            }
        }

        private void myNumTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (myNumTb.Text == "010-6689-9744")
            {
                myNumTb.Text = "";
                SolidColorBrush Brush1 = new SolidColorBrush();
                Brush1.Color = Colors.Gray;
                myNumTb.Foreground = Brush1;
            }
        }

        private void myNumTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (myNumTb.Text == String.Empty)
            {
                myNumTb.Text = "010-6689-9744";
                SolidColorBrush Brush2 = new SolidColorBrush();
                Brush2.Color = Colors.Blue;
                myNumTb.Foreground = Brush2;
            }
        }

        private void myCarTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (myNameTb.Text == "02-3702-8500")
            {
                myNameTb.Text = "";
                SolidColorBrush Brush1 = new SolidColorBrush();
                Brush1.Color = Colors.Gray;
                myNameTb.Foreground = Brush1;
            }
        }

        private void myCarTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (myCarTb.Text == String.Empty)
            {
                myCarTb.Text = "02-3702-8500";
                SolidColorBrush Brush2 = new SolidColorBrush();
                Brush2.Color = Colors.Blue;
                myCarTb.Foreground = Brush2;
            }
        }

        private void mybloodTb_GotFocus(object sender, RoutedEventArgs e)
        {
            if (mybloodTb.Text == "Rh+ B형")
            {
                mybloodTb.Text = "";
                SolidColorBrush Brush1 = new SolidColorBrush();
                Brush1.Color = Colors.Gray;
                mybloodTb.Foreground = Brush1;
            }
        }

        private void mybloodTb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (mybloodTb.Text == String.Empty)
            {
                mybloodTb.Text = "Rh+ B형";
                SolidColorBrush Brush2 = new SolidColorBrush();
                Brush2.Color = Colors.Blue;
                mybloodTb.Foreground = Brush2;
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            string message;
            string caption;
            message = "When complete all the forms of input, AirBand service is activated. Would you like to register?";
            caption = "Register";
            MessageBoxButton buttons = MessageBoxButton.OKCancel;
            // Show message box
            MessageBoxResult result = MessageBox.Show(message, caption, buttons);

            if (MessageBoxResult.OK == result)
            {
                setting["myNoKey"] = myNumTb.Text.ToString();
                setting["autoInsuranceNoKey"] = myCarTb.Text.ToString();
                setting["mainFlag"] = "check";

                setting.Save();
            }
        }
    }
}