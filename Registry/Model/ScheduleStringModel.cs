using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class ScheduleStringModel : INotifyPropertyChanged
    {
        public DoctorModel Doctor { get; set; }
        public ObservableCollection<ScheduleModel> Schedules { get; set; }
        public ScheduleStringModel()
        {
            Schedules = new ObservableCollection<ScheduleModel>();
        }
        public ScheduleStringModel(DoctorModel doctor)
        {
            Schedules = new ObservableCollection<ScheduleModel>();
            Doctor = doctor;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
