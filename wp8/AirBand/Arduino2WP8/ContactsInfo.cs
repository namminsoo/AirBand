using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Arduino2WP8
{
    class ContactsInfo
    {
        private int index;
        private string name;
        private string phoneNumber;
        private Ellipse profilePicture;

        public ContactsInfo(int index, string name, string phoneNumber, Ellipse profilePicture)
        {
            this.index = index;
            this.name = name;
            this.phoneNumber = phoneNumber;
            this.profilePicture = profilePicture;
        }

        public int Index
        {
            get
            {
                return index;
            }

            set
            {
                index = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }

            set
            {
                phoneNumber = value;
            }
        }

        public Ellipse ProfilePicture
        {
            get
            {
                return profilePicture;
            }

            set
            {
                profilePicture = value;
            }
        }
    }
}
