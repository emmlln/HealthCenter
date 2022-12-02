using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Registry.Model;

namespace Registry.ViewModel
{
    public class ChangingViewModel : INotifyPropertyChanged
    {
        private bool first;
        private DBAccess dbAccess;
        private Window window;
        public ObservableCollection<ScheduleModel> Changings { get; set; }
        public ObservableCollection<ScheduleModel> ChangingsRight { get; set; }
        private ObservableCollection<DoctorModel> cdoctors;
        public ObservableCollection<DoctorModel> Cdoctors
        {
            get { return cdoctors; }
            set
            {
                cdoctors = value;
                OnPropertyChanged("Cdoctors");
            }
        }
        private ObservableCollection<DoctorModel> doctors; 
        public ObservableCollection<DoctorModel> Doctors
        {
            get { return doctors; }
            set
            {
                doctors = value;
                OnPropertyChanged("Doctors");
            }
        }
        private bool changing { get; set; }
        public bool Changing
        {
            get { return changing; }
            set
            {
                changing = value;
                setDoctors();
                OnPropertyChanged("Changing");
            }
        }
        private DateTime endDate { get; set; }
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                if (!first)
                {
                    setScheludes();
                }
                OnPropertyChanged("EndDate");
            }
        }

        private DateTime startDate { get; set; }
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                if (!first)
                {
                    setScheludes();
                }
                OnPropertyChanged("StartDate");
            }
        }
        private DoctorModel selectedCDoctor;
        public DoctorModel SelectedCDoctor
        {
            get { return selectedCDoctor; }
            set
            {
                selectedCDoctor = value;
                ChangingsRight.Clear();
                if (selectedCDoctor != null)
                {
                    setChangeScheludes();
                }
                OnPropertyChanged("SelectedCDoctor");
            }
        }

        private DoctorModel selectedDoctor;
        public DoctorModel SelectedDoctor
        {
            get { return selectedDoctor; }
            set
            {
                selectedDoctor = value;
                if (selectedDoctor != null)
                {
                    Cdoctors = new ObservableCollection<DoctorModel>(doctors.Where(i => i.ID != selectedDoctor.ID && i.SpecializationID == selectedDoctor.SpecializationID).ToList());
                }
                SelectedCDoctor = null;
                if (SelectedDoctor != null)
                {
                    first = true;
                    setScheludes();
                }
                OnPropertyChanged("SelectedDoctor");
            }
        }
        public Command Exit { get; set; }
        public Command Cancel { get; set; }
        public Command Submit { get; set; }

        private void setScheludes()
        {
            Changings.Clear();
            if (first && selectedDoctor.ZamEnd != null && selectedDoctor.ZamEnd >= DateTime.Now.Date)
            {
                EndDate = selectedDoctor.ZamEnd.Value;
                StartDate = selectedDoctor.ZamStart.Value;
                SelectedCDoctor = Cdoctors.Where(i => i.ID == selectedDoctor.ZamID).FirstOrDefault();
            }
            var buffer = dbAccess.GetDoctorSchedule(selectedDoctor.ID);
            foreach (var b in buffer)
            {
                Changings.Add(b);
            }
            first = false;
        }

        private void setChangeScheludes()
        {
            var buffer = dbAccess.GetDoctorSchedule(selectedCDoctor.ID).Where(i => i.ZamID == null || i.DoctorID != selectedDoctor.ID).ToList();
            foreach (var b in buffer)
            {
                ChangingsRight.Add(b);
            }
            foreach (var ch in Changings)
            {
                ch.Compare(ChangingsRight.ToList());
            }
        }

        private void submit()
        {
            if (SelectedDoctor.ZamEnd == null || SelectedDoctor.ZamEnd < endDate.Date)
            {
                SelectedDoctor.ZamEnd = endDate.Date;
            }
            else
            {
                SelectedDoctor.ZamEnd = endDate.Date;
                List<DoctorSeeModel> sees = dbAccess.GetDoctorSees(SelectedDoctor.ID).Where(i => i.DateTime.Date > SelectedDoctor.ZamEnd && i.Closed == false).ToList();
                foreach (DoctorSeeModel s in sees)
                {
                    s.ZamID = null;
                    dbAccess.UpdateDoctorSee(s);
                }
            }
            if (SelectedDoctor.ZamStart == null || SelectedDoctor.ZamStart > startDate.Date)
            {
                SelectedDoctor.ZamStart = startDate.Date;
            }
            else
            {
                SelectedDoctor.ZamStart = startDate.Date;
                List<DoctorSeeModel> sees = dbAccess.GetDoctorSees(SelectedDoctor.ID).Where(i => i.DateTime.Date < SelectedDoctor.ZamStart && i.Closed == false).ToList();
                foreach (DoctorSeeModel s in sees)
                {
                    s.ZamID = null;
                    dbAccess.UpdateDoctorSee(s);
                }
            }
            selectedDoctor.ZamID = selectedCDoctor.ID;
            dbAccess.UpdateDoctor(selectedDoctor);
            foreach (ScheduleModel changing in Changings)
            {
                List<DoctorSeeModel> sees = dbAccess.GetDoctorSees(SelectedDoctor.ID).Where(i => changing.dayofWeek == dbAccess.GetDayWeekId(i.DateTime.DayOfWeek.ToString()).Value && i.DateTime.Date >= SelectedDoctor.ZamStart.Value.Date && i.DateTime.Date <= SelectedDoctor.ZamEnd.Value.Date).ToList();
                if (changing.Free) // производим замену
                {
                    changing.ZamID = SelectedCDoctor.ID;
                    dbAccess.UpdateSchedule(changing);
                    foreach (DoctorSeeModel s in sees)
                    {
                        s.ZamID = changing.ZamID;
                        dbAccess.UpdateDoctorSee(s);
                    }
                }
                else // замена невозможна, отменяем записи
                {
                    changing.ZamID = null;
                    dbAccess.UpdateSchedule(changing);
                    foreach (DoctorSeeModel s in sees)
                    {
                        s.Closed = true;
                        dbAccess.UpdateDoctorSee(s);
                    }
                }
            }
        }

        private void setDoctors()
        {
            Doctors.Clear();
            Doctors = new ObservableCollection<DoctorModel>(dbAccess.GetDoctors());
            SelectedDoctor = null;
        }

        public ChangingViewModel(Window window)
        {
            Changings = new ObservableCollection<ScheduleModel>();
            ChangingsRight = new ObservableCollection<ScheduleModel>();
            dbAccess = new DBAccess();
            Doctors = new ObservableCollection<DoctorModel>();
            setDoctors();
            changing = false;
            endDate = DateTime.Now;
            startDate = DateTime.Now;
            this.window = window;
            Submit = new Command(obj =>
            {
                submit();
            },
            obj => { return selectedCDoctor != null && endDate.Date >= DateTime.Now.Date && startDate.Date <= endDate.Date && (selectedCDoctor.ZamEnd == null || selectedCDoctor.ZamEnd < startDate.Date); });
            Exit = new Command(obj =>
            {
                Window main = new RegistrationMain();
                main.WindowState = window.WindowState;
                main.Top = window.Top;
                main.Left = window.Left;
                main.Height = window.Height;
                main.Width = window.Width;
                main.Show();
                window.Close();
            });
            Cancel = new Command(obj =>
            {
                List<DoctorSeeModel> sees = dbAccess.GetDoctorSees(SelectedDoctor.ID).Where(i => i.DateTime.Date >= SelectedDoctor.ZamStart.Value.Date && i.DateTime.Date <= SelectedDoctor.ZamEnd.Value.Date).ToList();
                foreach (DoctorSeeModel s in sees)
                {
                    s.ZamID = null;
                    dbAccess.UpdateDoctorSee(s);
                }
                selectedDoctor.ZamEnd = null;
                selectedDoctor.ZamStart = null;
                selectedDoctor.ZamID = null;
                dbAccess.UpdateDoctor(selectedDoctor);
            }, obj => { return selectedDoctor != null && selectedDoctor.ZamEnd != null && selectedDoctor.ZamStart != null; });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
