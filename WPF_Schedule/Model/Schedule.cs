using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace WPF_Schedule.Model
{
    [Table("Schedule")]
    public class Schedule : INotifyPropertyChanged
    {
        private int _id;
        private string _city;
        private string _shop;
        private string _employee;
        private string _brigade;
        private string _shift;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged("City");
            }
        }

        public string Shop
        {
            get => _shop;
            set
            {
                _shop = value;
                OnPropertyChanged("Shop");
            }
        }

        public string Employee
        {
            get => _employee;
            set
            {
                _employee = value;
                OnPropertyChanged("Employee");
            }
        }

        public string Brigade
        {
            get => _brigade;
            set
            {
                _brigade = value;
                OnPropertyChanged("Brigade");
            }
        }

        public string Shift
        {
            get => _shift;
            set
            {
                _shift = value;
                OnPropertyChanged("Shift");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
