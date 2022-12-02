using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Registry.Model;

namespace Registry.ViewModel
{
    class ScheduleViewModel : INotifyPropertyChanged
    {
        public List<string> Places { get; set; }
        public List<SpecializationModel> Specializations { get; set; }
        private ObservableCollection<ScheduleStringModel> schedules;
        public ObservableCollection<ScheduleStringModel> Schedules
        {
            get { return schedules; }
            set
            {
                schedules = value;
                OnPropertyChanged("Schedules");
            }
        }
        private bool close;
        private bool error;
        public bool Error
        {
            get { return error; }
            set
            {
                error = value;
                OnPropertyChanged("Error");
            }
        }
        private DoctorModel doctor;
        public List<ShiftModel> Shifts { get; set; }
        public List<DaysModel> Days { get; set; }
        private ScheduleModel selectedSchedule;
        public ScheduleModel SelectedSchedule
        {
            get { return selectedSchedule; }
            set
            {
                selectedSchedule = null;
                OnPropertyChanged("SelectedSchedule"); // иначе не сбросится выбор в соседних строках
                selectedSchedule = value;
                if (value != null)
                {
                    selectedDoctor = null;
                    OnPropertyChanged("SelectedDoctor");
                    doctor = dbAccess.GetDoctor(selectedSchedule.DoctorID);
                    checkError();
                    string[] fio = doctor.FullName.Split(' ');
                    FIO = fio[0] + ' ';
                    if (fio.Length > 2)
                    {
                        FIO += fio[1][0] + ". ";
                    }
                    if (fio.Length > 2)
                    {
                        FIO += fio[2][0] + ".";
                    }
                    SelectedDay = Days.Where(i => i.ID == selectedSchedule.dayofWeek).FirstOrDefault();
                    SelectedShift = Shifts.Where(i => i.ID == selectedSchedule.ShiftID).FirstOrDefault();
                    Room = selectedSchedule.Room.ToString();
                }
                else
                {
                    SelectedDay = null;
                    SelectedShift = null;
                    Room = "";
                    FIO = "";
                }
                OnPropertyChanged("SelectedSchedule");
            }
        }

        public void checkError()
        {
            if (doctor != null && selectedShift != null && selectedDay != null)
            {
                Error = false;
                close = false;
                List<ScheduleModel> schedules = new List<ScheduleModel>();
                schedules = dbAccess.GetDoctorMainSchedule(doctor.ID);
                foreach (ScheduleModel sc in schedules)
                {
                    if (sc.dayofWeek == selectedDay.ID)
                    {
                        if (sc.DoctorID == doctor.ID)
                        {
                            close = true;
                        }
                        else
                        {
                            if (sc.ShiftID == selectedShift.ID)
                            {
                                close = true;
                                Error = true;
                            }
                        }
                    }
                }
            }
        }

        private ScheduleStringModel selectedDoctor;
        public ScheduleStringModel SelectedDoctor
        {
            get { return selectedDoctor; }
            set
            {
                selectedSchedule = null;
                OnPropertyChanged("SelectedSchedule");
                selectedDoctor = value;
                if (selectedDoctor != null)
                {
                    doctor = selectedDoctor.Doctor;
                }
                checkError();
                if (selectedDoctor != null)
                {
                    FIO = selectedDoctor.Doctor.FullName;
                }
                else
                {
                    FIO = "";
                }
                OnPropertyChanged("SelectedDoctor");
            }
        }
        private int currentPlace;
        public int CurrentPlace
        {
            get { return currentPlace; }
            set
            {
                currentPlace = value;
                setSchedule();
                OnPropertyChanged("CurrCurrentPlaceentDay");
            }
        }
        private int currentSpecialization;
        public int CurrentSpecialization
        {
            get { return currentSpecialization; }
            set
            {
                currentSpecialization = value;
                setSchedule();
                OnPropertyChanged("CurrentSpecialization");
            }
        }

        private DaysModel selectedDay;
        public DaysModel SelectedDay
        {
            get { return selectedDay; }
            set
            {
                selectedDay = value;
                checkError();
                OnPropertyChanged("SelectedDay");
            }
        }

        private ShiftModel selectedShift;
        public ShiftModel SelectedShift
        {
            get { return selectedShift; }
            set
            {
                selectedShift = value; 
                checkError();
                OnPropertyChanged("SelectedShift");
            }
        }

        private string room;
        public string Room
        {
            get { return room; }
            set
            {
                room = value;
                OnPropertyChanged("Room");
            }
        }

        private string fio;
        public string FIO
        {
            get { return fio; }
            set
            {
                fio = value;
                OnPropertyChanged("FIO");
            }
        }

        private DBAccess dbAccess;
        private Window thisWindow;

        public ScheduleViewModel(Window thisWindow)
        {
            dbAccess = new DBAccess();
            Shifts = dbAccess.GetShifts();
            Days = dbAccess.GetDays();
            Schedules = new ObservableCollection<ScheduleStringModel>();
            Specializations = new List<SpecializationModel>();
            Specializations.Add(new SpecializationModel());
            Error = false;
            Specializations.AddRange(dbAccess.GetSpecializations());
            Places = new List<string>();
            Places.Add("");
            var Streets = dbAccess.GetStreets();
            foreach (StreetsModel st in Streets)
            {
                if (Places.Where(i => st.PlaceOfSee.ToString() == i).ToList().Count == 0)
                {
                    Places.Add(st.PlaceOfSee.ToString());
                }
            }
            this.thisWindow = thisWindow;
            setSchedule();
            commands();
        }

        private void setSchedule()
        {
            Schedules.Clear();
            SelectedSchedule = null;
            var Doctors = dbAccess.GetDoctors().OrderBy(i => i.PlaceOfSee).ToList();
            if (CurrentPlace > 0)
            {
                Doctors = Doctors.Where(i => i.PlaceOfSee == int.Parse(Places[currentPlace])).ToList();
            }
            if (currentSpecialization > 0)
            {
                Doctors = Doctors.Where(i => i.SpecializationID == Specializations[currentSpecialization].ID).ToList();
            }
            foreach (DoctorModel d in Doctors)
            {
                string[] fio = d.FullName.Split(' ');
                d.FullName = fio[0] + ' ';
                if (fio.Length > 2)
                {
                    d.FullName += fio[1][0] + ". ";
                }
                if (fio.Length > 2)
                {
                    d.FullName += fio[2][0] + ".";
                }
                Schedules.Add(new ScheduleStringModel(d));
                var buffer = dbAccess.GetDoctorSchedule(d.ID).OrderBy(i => i.dayofWeek).ToList();
                buffer = buffer.Where(i => i.DoctorID == d.ID).ToList();
                foreach (ScheduleModel sc in buffer)
                {
                    Schedules.Last().Schedules.Add(sc);
                }
            }
        }
        private void commands()
        {
            Exit = new Command(obj =>
            {
                RegistrationMain window = new RegistrationMain();
                window.WindowState = thisWindow.WindowState;
                window.Top = thisWindow.Top;
                window.Left = thisWindow.Left;
                window.Height = thisWindow.Height;
                window.Width = thisWindow.Width;
                window.Show();
                thisWindow.Close();
            });
            Create = new Command(obj =>
            {
                ScheduleModel sc = new ScheduleModel();
                sc.Room = int.Parse(room);
                sc.DoctorID = doctor.ID;
                sc.dayofWeek = selectedDay.ID;
                sc.ShiftID = selectedShift.ID;
                dbAccess.AddSchedule(sc);
                setSchedule();
            }, obj =>
            {
                return
                room != "" && int.TryParse(room, out int a) &&
                selectedDay != null && selectedShift != null &&
                !close && (selectedDoctor != null || selectedSchedule != null) && doctor != null &&
                schedules.Where(i => i.Schedules.Where(j => j.ShiftID == selectedShift.ID &&
                j.dayofWeek == selectedDay.ID && j.Room == int.Parse(room)).Count() > 0).ToList().Count() == 0;
            });

            Update = new Command(obj =>
            {
                selectedSchedule.Room = int.Parse(room);
                if (selectedSchedule.ShiftID != selectedShift.ID)
                {
                    double raznica = selectedShift.TimeofBegin.TotalMinutes - dbAccess.GetShift(selectedSchedule.ShiftID).TimeofBegin.TotalMinutes;
                    selectedSchedule.ShiftID = selectedShift.ID;
                    var buf = dbAccess.GetDoctorSees(doctor.ID).Where(i => i.Visited == null && i.Closed == false && i.DateTime >= DateTime.Now.Date && dbAccess.GetDayWeekId(i.DateTime.DayOfWeek.ToString()) == selectedDay.ID).ToList();
                    foreach (DoctorSeeModel ds in buf)
                    {
                        ds.DateTime = ds.DateTime.AddMinutes(raznica);
                        dbAccess.UpdateDoctorSee(ds);
                    }
                }
                dbAccess.UpdateSchedule(selectedSchedule);
                setSchedule();
            }, obj =>
            {
                return
                selectedSchedule != null &&
                (doctor.ZamEnd == null || doctor.ZamEnd.Value.Date < DateTime.Now.Date) &&
                room != "" && int.TryParse(room, out int a) &&
                selectedDay.ID == selectedSchedule.dayofWeek && selectedShift != null &&
                (selectedDoctor != null || selectedSchedule != null) && doctor != null &&
                schedules.Where(i => i.Schedules.Where(j => j.ShiftID == selectedShift.ID &&
                j.dayofWeek == selectedDay.ID && j.Room == int.Parse(room)).Count() > 0).ToList().Count() == 0;
            });

            Delete = new Command(obj =>
            {
                var buf = dbAccess.GetDoctorSees(doctor.ID).Where(i => i.Visited == null && i.Closed == false && i.DateTime >= DateTime.Now.Date && dbAccess.GetDayWeekId(i.DateTime.DayOfWeek.ToString()) == selectedSchedule.dayofWeek).ToList();
                foreach (DoctorSeeModel ds in buf)
                {
                    ds.Closed = true;
                    dbAccess.UpdateDoctorSee(ds);
                }
                dbAccess.removeSchedule(selectedSchedule.ID);
                setSchedule();
            }, obj =>
            {
                return
                selectedSchedule != null;
            });

            Print = new Command(obj =>
            {
                var schedules = dbAccess.GetScheduleProcedures();
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add("Расписание процедурных");
                workSheet.DefaultRowHeight = 70;
                workSheet.DefaultColWidth = 28;
                int y = 1;
                foreach (ScheduleStringModel sc in Schedules)
                {
                    if (sc.Schedules.Count() > 0)
                    {
                        workSheet.Cells[y, 1].Value = sc.Doctor.FullName + "\n" + "Участок: " + sc.Doctor.PlaceOfSee + "\n" + "Специализация: " + sc.Doctor.Specialization;
                        for (int x = 2; x <= sc.Schedules.Count() + 1; x++)
                        {
                            var shift = dbAccess.GetShift(sc.Schedules[x - 2].ShiftID);
                            string text = sc.Schedules[x - 2].Day + "\n" + "Кабинет: " + sc.Schedules[x - 2].Room + "\n" + shift.TimeofBegin.ToString().Remove(5) + " - " + shift.TimeofEnd.ToString().Remove(5);
                            if (sc.Doctor.ZamEnd != null && sc.Doctor.ZamStart != null && DateTime.Now >= sc.Doctor.ZamStart && DateTime.Now <= sc.Doctor.ZamEnd)
                            {
                                text += "\nС " + sc.Doctor.ZamStart.ToString().Substring(0, 10) + "\n" + "До " + sc.Doctor.ZamEnd.ToString().Substring(0, 10);
                                if (sc.Schedules[x - 2].ZamID != null)
                                {
                                    text += "\nЗамена на" + "\n" + dbAccess.GetDoctor(sc.Schedules[x - 2].ZamID.Value).FullName;
                                }
                                else
                                {
                                    text += "\nПрием отменен";
                                }
                            }
                            workSheet.Cells[y, x].Value = text;
                        }
                        y++;
                    }
                }
                string p_strPath = "расписание_врачей" + DateTime.Now.ToString().Replace(" ", "").Replace(".", "").Replace(":", "") + ".xlsx";
                FileStream objFileStrm = File.Create(p_strPath);
                objFileStrm.Close();
                File.WriteAllBytes(p_strPath, excel.GetAsByteArray());
            });
        }
        public Command Exit { get; set; }
        public Command Create { get; set; }
        public Command Update { get; set; }
        public Command Delete { get; set; }
        public Command Print { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
