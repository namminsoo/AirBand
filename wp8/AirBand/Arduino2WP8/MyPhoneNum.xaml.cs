using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using Microsoft.Phone.UserData;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Input;

namespace Arduino2WP8
{
    public partial class MyPhoneNum : PhoneApplicationPage
    {
        IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;
        IEnumerable<Contact> contacts;


        ContactsInfo[] contactsInfo;


        StackPanel[] sp;
        Ellipse[] ellipse;
        TextBlock[] textBlock;
        TextBlock[] textBlock_num;
        BitmapImage bm;
        Contacts cons;


        int contactsSize = 0;


        public MyPhoneNum()
        {
            InitializeComponent();
            Grid.SetRow(listBox, 1); //비상연락망이 표현될 listBox Set

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

                sp = new StackPanel[contactsSize];
                for (int i = 0; i < contactsSize; i++) { sp[i] = new StackPanel(); }

                ellipse = new Ellipse[contactsSize];
                for (int i = 0; i < contactsSize; i++) { ellipse[i] = new Ellipse(); }

                textBlock = new TextBlock[contactsSize];
                for (int i = 0; i < contactsSize; i++) { textBlock[i] = new TextBlock(); }

                textBlock_num = new TextBlock[contactsSize];
                for (int i = 0; i < contactsSize; i++) { textBlock_num[i] = new TextBlock(); }

                


                for (int i = 0; i < contactsSize; i++)
                {
                    sp[i].Orientation = System.Windows.Controls.Orientation.Horizontal;
                    sp[i].Margin = new Thickness(35, 10, 0, 10);
                    sp[i].Children.Add(ellipse[i]); // 사진이 들어갈 타원의 높이, 넓이 등 지정
                    sp[i].Children.Add(textBlock[i]); // 이름이 들어갈 텍스트블러의 높이, 넓이 등 지정
                    sp[i].Children.Add(textBlock_num[i]); // 번호가 들어갈 텍스트블러의 높이, 넓이 등 지정



                    setEllipse(ellipse[i]); // 사진 설정 메서드
                    setTextTb(textBlock[i]); // 이름 메서드
                    setTextTb(textBlock_num[i]); // 이름에 해당하는 번호 메서드 
                    listBox.Items.Insert(i, sp[i]); // 한명씩 listboxItem 형식으로 추가
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
            //for (int i = 0; i < NumberOfContacts; i++)
            for (int i = 0; i < contactsSize; i++)

            {
                //프로필 사진 넣는 부분 
                BitmapImage img = new BitmapImage();
                img.SetSource(e.Results.ElementAt(i).GetPicture());
                ImageBrush myBrush = new ImageBrush();
                myBrush.ImageSource = img;

                ellipse[i].Fill = myBrush;
                textBlock[i].Text = e.Results.ElementAt(i).DisplayName; //이름 넣는 부분
                textBlock_num[i].Text = e.Results.ElementAt(i).PhoneNumbers.FirstOrDefault().PhoneNumber; //전화번호 넣어주는 부분
                contactsInfo[i] = new ContactsInfo(i, e.Results.ElementAt(i).DisplayName, e.Results.ElementAt(i).PhoneNumbers.FirstOrDefault().PhoneNumber, ellipse[i]);
                

            }



            /*                Contacts에서 연락망을 추가해서 표시할 부분 시작                                */


        }

       


        private void setEllipse(object obj) // 사진 설정 메서드
        {
            Ellipse ellipse = (Ellipse)obj;
            ellipse.VerticalAlignment = VerticalAlignment.Center;
            ellipse.Margin = new Thickness(0, 0, 15, 0);
            ellipse.Width = 75;
            ellipse.Height = 75;
        }

        public void setTextTb(object obj) // 이름, 폰번호 설정 메서드
        {
            TextBlock textBlock = (TextBlock)obj;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Margin = new Thickness(20, 10, 0, 10);
            textBlock.FontSize = 30;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ListBox sitem = (ListBox)sender;
            int index =   sitem.SelectedIndex;
            string message;
            string caption;
            int count = 0;
            //MessageBox.Show(contactsInfo[index].Index + " / " + contactsInfo[index].Name + " / " + contactsInfo[index].PhoneNumber + " / ");

            if (setting.Contains("name" + index.ToString()))
            {
                message = "Remove the emergency contact number registered to.";
                caption = "Remove";
                MessageBoxButton buttons = MessageBoxButton.OKCancel;
                // Show message box
                MessageBoxResult result = MessageBox.Show(message, caption, buttons);

                if (MessageBoxResult.OK == result)
                {
                    setting.Remove("name" + index.ToString());
                    //setting["name" + index.ToString()].
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
                    setting["name" + index.ToString()] = contactsInfo[index].Index.ToString();
                    setting["mainFlag"] = "check";
                    setting.Save();
                }
            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}