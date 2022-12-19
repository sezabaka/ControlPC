using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPC.Entity
{
    public class PCInfor : INotifyPropertyChanged
    {
        private int no;
        public int No
        {
            get { return no; }
            set
            {
                no = value;
                OnPropertyChanged("No");
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string ipAddress;
        public string IPAddress
        {
            get { return ipAddress; }
            set
            {
                ipAddress = value;
                OnPropertyChanged("IPAddress");
            }
        }

        private byte ttl;
        public byte TTL
        {
            get { return ttl; }
            set
            {
                ttl = value;
                OnPropertyChanged("TTL");
            }
        }

        private bool p80;
        public bool P80
        {
            get { return p80; }
            set
            {
                p80 = value;
                OnPropertyChanged("P80");
            }
        }

        /*private bool p22;
        public bool P22
        {
            get { return p22; }
            set
            {
                p22 = value;
                OnPropertyChanged("P22");
            }
        }*/

        private bool p23;
        public bool P23
        {
            get { return p23; }
            set
            {
                p23 = value;
                OnPropertyChanged("P23");
            }
        }

        private bool p902;
        public bool P902
        {
            get { return p902; }
            set
            {
                p902 = value;
                OnPropertyChanged("P902");
            }
        }

        /*private bool p445;
        public bool P445
        {
            get { return p445; }
            set
            {
                p445 = value;
                OnPropertyChanged("P445");
            }
        }

        private bool p3389;
        public bool P3389
        {
            get { return p3389; }
            set
            {
                p3389 = value;
                OnPropertyChanged("P3389");
            }
        }*/

        private bool exist = false;
        public bool Exist
        {
            get { return exist; }
            set
            {
                exist = value;
                OnPropertyChanged("Exist");
            }
        }

        private bool none = false;
        public bool None
        {
            get { return none; }
            set
            {
                none = value;
                OnPropertyChanged("None");
            }
        }

        private int threadID = -1;
        public int ThreadID
        {
            get { return threadID; }
            set
            {
                threadID = value;
                OnPropertyChanged("ThreadID");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string s)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(s));
            }
        }
    }
}
