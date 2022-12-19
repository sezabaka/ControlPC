using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPC.Entity
{
    public class PCWithUSB : INotifyPropertyChanged
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

        private string dept;
        public string Dept
        {
            get { return dept; }
            set
            {
                dept = value;
                OnPropertyChanged("Dept");
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

        private string wpd;
        public string WPD
        {
            get { return wpd; }
            set
            {
                wpd = value;
                OnPropertyChanged("WPD");
            }
        }

        private string proName;
        public string ProName
        {
            get { return proName; }
            set
            {
                proName = value;
                OnPropertyChanged("ProName");
            }
        }

        private string proCore;
        public string ProCore
        {
            get { return proCore; }
            set
            {
                proCore = value;
                OnPropertyChanged("ProCore");
            }
        }

        private string proThread;
        public string ProThread
        {
            get { return proThread; }
            set
            {
                proThread = value;
                OnPropertyChanged("ProThread");
            }
        }

        private bool hasSEP = false;
        public bool HasSEP
        {
            get { return hasSEP; }
            set
            {
                hasSEP = value;
                OnPropertyChanged("HasSEP");
            }
        }

        private string winCap;
        public string WinCap
        {
            get { return winCap; }
            set
            {
                winCap = value;
                OnPropertyChanged("WinCap");
            }
        }

        private string winBuild;
        public string WinBuild
        {
            get { return winBuild; }
            set
            {
                winBuild = value;
                OnPropertyChanged("WinBuild");
            }
        }

        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }

        private string mainName;
        public string MainName
        {
            get { return mainName; }
            set
            {
                mainName = value;
                OnPropertyChanged("MainName");
            }
        }

        private string mainProduct;
        public string MainProduct
        {
            get { return mainProduct; }
            set
            {
                mainProduct = value;
                OnPropertyChanged("MainProduct");
            }
        }

        /*private string biosName;
        public string BiosName
        {
            get { return biosName; }
            set
            {
                biosName = value;
                OnPropertyChanged("BiosName");
            }
        }

        private string biosVersion;
        public string BiosVersion
        {
            get { return biosVersion; }
            set
            {
                biosVersion = value;
                OnPropertyChanged("BiosVersion");
            }
        }*/

        private string diskSeri;
        public string DiskSeri
        {
            get { return diskSeri; }
            set
            {
                diskSeri = value;
                OnPropertyChanged("DiskSeri");
            }
        }

        private string diskSize;
        public string DiskSize
        {
            get { return diskSize; }
            set
            {
                diskSize = value;
                OnPropertyChanged("DiskSize");
            }
        }


        private string ramSeri;
        public string RamSeri
        {
            get { return ramSeri; }
            set
            {
                ramSeri = value;
                OnPropertyChanged("RamSeri");
            }
        }

        private string ramCapa;
        public string RamCapa
        {
            get { return ramCapa; }
            set
            {
                ramCapa = value;
                OnPropertyChanged("RamCapa");
            }
        }

        private string ramSpeed;
        public string RamSpeed
        {
            get { return ramSpeed; }
            set
            {
                ramSpeed = value;
                OnPropertyChanged("RamSpeed");
            }
        }

        private string mac;
        public string MAC
        {
            get { return mac; }
            set
            {
                mac = value;
                OnPropertyChanged("MAC");
            }
        }

        private string dhcp;
        public string DHCP
        {
            get { return dhcp; }
            set
            {
                dhcp = value;
                OnPropertyChanged("DHCP");
            }
        }

        

        private string keyboardID;
        public string KeyboardID
        {
            get { return keyboardID; }
            set
            {
                keyboardID = value;
                OnPropertyChanged("KeyboardID");
            }
        }

        private string mouseID;
        public string MouseID
        {
            get { return mouseID; }
            set
            {
                mouseID = value;
                OnPropertyChanged("MouseID");
            }
        }

        private string monitorId;
        public string MonitorId
        {
            get { return monitorId; }
            set
            {
                monitorId = value;
                OnPropertyChanged("MonitorId");
            }
        }

        private string printerID;
        public string PrinterID
        {
            get { return printerID; }
            set
            {
                printerID = value;
                OnPropertyChanged("PrinterID");
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

        /*private string exception;
        public string Exception
        {
            get { return exception; }
            set
            {
                exception = value;
                OnPropertyChanged("Exception");
            }
        }*/

        /*private string verName;
        public string VerName
        {
            get { return verName; }
            set
            {
                verName = value;
                OnPropertyChanged("VerName");
            }
        }*/

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
