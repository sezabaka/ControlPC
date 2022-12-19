using System.ComponentModel;

namespace ControlPC.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        public void RaisePropertyChangingEvent(string s)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(s));
        }

        public void RaisePropertyChangedEvent(string s)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(s));
        }
    }
}
