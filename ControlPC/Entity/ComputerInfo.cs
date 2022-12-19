using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlPC.Entity
{
    public class ComputerInfo : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string VerName { get; set; }
        public DateTime lastLogon { get; set; }

        private string pcName;
        public string PCName
        {
            get { return pcName; }
            set
            {
                pcName = value;
                OnPropertyChanged("PCName");
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
