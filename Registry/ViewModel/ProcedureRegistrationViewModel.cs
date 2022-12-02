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
    public class ProcedureRegistrationViewModel : INotifyPropertyChanged
    {
        public class DateDays
        {
            public string Date { get; set; }
            public int ScheduleID;
            public DateDays()
            {

            }
        }

        public class RoomsInfo
        {
            public DateTime Date;
            public int ScheduleID;
            public int Room;
            public string Info { get; set; }
            public RoomsInfo()
            {

            }

            public void SetInfo(int count)
            {
                Info = Room.ToString() + " " + count.ToString();
            }
        }
        private DBAccess dbAccess;
        private List<ScheduleProcedureModel> schedules;
        public ObservableCollection<DateDays> Dates { get; set; }
        
        private DateDays selectedDate;
        public DateDays SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                Rooms.Clear();
                SelectedRoom = null;
                if (selectedDate != null)
                {
                    setRooms();
                }
                OnPropertyChanged("SelectedDate");
            }
        }
        public ObservableCollection<RoomsInfo> Rooms { get; set; }

        private RoomsInfo selectedRoom;
        public RoomsInfo SelectedRoom
        {
            get { return selectedRoom; }
            set
            {
                selectedRoom = value;
                OnPropertyChanged("SelectedRoom");
            }
        }
        public ObservableCollection<PatientModel> Patients { get; set; }
        public ObservableCollection<TypeofProcModel> Procedures { get; set; }

        private PatientModel selectedPatient;
        public PatientModel SelectedPatient
        {
            get { return selectedPatient; }
            set
            {
                SelectedProcedure = null;
                Procedures.Clear();
                selectedPatient = value;
                if (selectedPatient != null)
                {
                    setProcedures();
                }
                OnPropertyChanged("SelectedPatient");
            }
        }

        private TypeofProcModel selectedProcedure;
        public TypeofProcModel SelectedProcedure
        {
            get { return selectedProcedure; }
            set
            {
                SelectedDate = null;
                Dates.Clear();
                selectedProcedure = value;
                if (selectedProcedure != null)
                {
                    setDays();
                }
                OnPropertyChanged("SelectedProcedure");
            }
        }
        public Command Submit { get; set; }
        public Command Main { get; set; }
        private void setProcedures()
        {
            var doproc = dbAccess.GetDoProcForPatient(selectedPatient.ID);
            Procedures.Clear();
            foreach (DoProcModel d in doproc)
            {
                if (Procedures.Where(i => i.ID == d.ProcID).FirstOrDefault() == null && !dbAccess.CheckBusyProcedure(d.ID))
                {
                    Procedures.Add(dbAccess.GetTypeProc(d.ProcID));
                }
            }
        }
        private void setDays()
        {
            Dates.Clear();
            schedules = dbAccess.GetScheduleProceduresByType(selectedProcedure.ID);
            foreach (ScheduleProcedureModel sc in schedules)
            {
                if (Dates.Where(i => dbAccess.GetDayWeekId(DateTime.Parse(i.Date).DayOfWeek.ToString()).Value == sc.DayID).FirstOrDefault() == null)
                {
                    DateTime date = DateTime.Now.Date;
                    while (dbAccess.GetDayWeekId(date.DayOfWeek.ToString()) != sc.DayID)
                    {
                        date = date.AddDays(1);
                    }
                    int count = 0;
                    while (date.Date <= DateTime.Now.Date.AddDays(14))
                    {
                        count = sc.Count - dbAccess.GetBusyProceduresCount(sc.ID, date);
                        if (count > 0)
                        {
                            DateDays buf = new DateDays();
                            buf.ScheduleID = sc.ID;
                            buf.Date = date.ToString().Substring(0, 10);
                            Dates.Add(buf);
                        }
                        date = date.AddDays(7);
                    }
                }
            }
        }

        private void setRooms()
        {
            Rooms.Clear();
            var sch = schedules.Where(i => i.ID == selectedDate.ScheduleID).ToList();
            foreach (ScheduleProcedureModel s in sch)
            {
                int count = s.Count - dbAccess.GetBusyProceduresCount(s.ID, DateTime.Parse(selectedDate.Date).Date);
                if (count > 0)
                {
                    RoomsInfo r = new RoomsInfo();
                    r.Date = DateTime.Parse(selectedDate.Date).Date;
                    r.ScheduleID = s.ID;
                    r.Room = s.Room;
                    r.SetInfo(count);
                    Rooms.Add(r);
                }
            }
        }
        public ProcedureRegistrationViewModel(Window window)
        {
            dbAccess = new DBAccess();
            Procedures = new ObservableCollection<TypeofProcModel>();
            Dates = new ObservableCollection<DateDays>();
            Rooms = new ObservableCollection<RoomsInfo>();
            List<PatientModel> patients = dbAccess.GetPatients(true);
            if (patients != null)
            {
                Patients = new ObservableCollection<PatientModel>(patients);
            }

            Submit = new Command(obj =>
            {
                ProcedureModel procedure = new ProcedureModel();
                procedure.Date = DateTime.Parse(selectedDate.Date);
                procedure.PatientID = selectedPatient.ID;
                procedure.ScheduleID = selectedDate.ScheduleID;
                procedure.ProcID = selectedProcedure.ID;
                dbAccess.addProcedure(procedure);
                SelectedPatient = null;
            }, obj =>
            {
                return selectedRoom != null;
            });

            Main = new Command(obj =>
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
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
