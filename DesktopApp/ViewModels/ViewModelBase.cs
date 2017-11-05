using System.ComponentModel;

namespace DesktopApp.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            this.PropertyChanged?.Invoke(sender, args);
        }
    }
}
