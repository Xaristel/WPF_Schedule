using System.Windows;
using WPF_Schedule.ViewModel;

namespace WPF_Schedule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ScheduleVIewModel();
        }
    }
}
