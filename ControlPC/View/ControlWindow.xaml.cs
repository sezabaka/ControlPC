using System;
using System.Windows;
using ControlPC.ViewModel;

namespace ControlPC.View
{
    public partial class ControlWindow : Window
    {
        public ControlWindow()
        {
            InitializeComponent();
            this.DataContext = new ControlPararelViewModel();
        }

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
