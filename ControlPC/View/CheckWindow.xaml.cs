using System;
using System.Windows;
using ControlPC.ViewModel;

namespace ControlPC.View
{
    public partial class CheckWindow : Window
    {
    CheckViewModel viewmodel;

        public CheckWindow()
        {
            InitializeComponent();
            viewmodel = new CheckViewModel();
            this.DataContext = viewmodel;
        }
    }
}
