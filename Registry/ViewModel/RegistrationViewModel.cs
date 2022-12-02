using PdfSharp.Drawing;
using PdfSharp.Pdf;
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
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        private List<string> daysList = new List<string>{ "пн", "вт", "ср", "чт", "пт", "сб", "вс" };
        private DBAccess dbAccess;

        public ObservableCollection<string> Times { get; set; }
        private string days;
        public string Days
        {
            get { return days; }
            set
            {
                days = value;
                OnPropertyChanged("Days");
            }
        }
        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                if (value <= DateTime.Now.Date.AddDays(14) && value.Date >= DateTime.Now.Date)
                {
                    selectedDate = value;
                }
                else
                {
                    selectedDate = DateTime.Now.Date;
                }
                setDoctors();
                OnPropertyChanged("SelectedDate");
            }
        }
        public ObservableCollection<PatientModel> Patients { get; set; }

        private PatientModel selectedPatient;
        public PatientModel SelectedPatient
        {
            get { return selectedPatient; }
            set
            {
                selectedPatient = value;
                Days = "";
                SelectedSpecialization = null;
                setDoctors();
                OnPropertyChanged("SelectedPatient");
            }
        }

        private int selectedTime;
        public int SelectedTime
        {
            get { return selectedTime; }
            set
            {
                selectedTime = value;
                OnPropertyChanged("SelectedTime");
            }
        }
        public ObservableCollection<SpecializationModel> Specializations { get; set; }

        private SpecializationModel selectedSpecialization;
        public SpecializationModel SelectedSpecialization
        {
            get { return selectedSpecialization; }
            set
            {
                selectedSpecialization = value;
                if (selectedSpecialization != null)
                {
                    setDays();
                    setDoctors();
                }
                OnPropertyChanged("SelectedSpecialization");
            }
        }
        private ScheduleModel schedule;
        private DoctorModel doctor { get; set; }
        public DoctorModel Doctor
        {
            get { return doctor; }
            set
            {
                doctor = value;
                OnPropertyChanged("Doctor");
            }
        }

        public Command Submit { get; set; }
        public Command Main { get; set; }
        private void setDoctors()
        {
            if (Times != null)
            {
                Times.Clear();
            }
            if (SelectedPatient != null && SelectedSpecialization != null)
            {
                var doctor = dbAccess.GetDoctors().Where(i => i.SpecializationID == selectedSpecialization.ID && i.PlaceOfSee == selectedPatient.PlaceOfSee).FirstOrDefault();
                if (doctor != null)
                {
                    schedule = dbAccess.GetDoctorMainSchedule(doctor.ID).Where(i => i.dayofWeek == dbAccess.GetDayWeekId(selectedDate.DayOfWeek.ToString())).FirstOrDefault();
                    if (schedule != null)
                    {
                        if (doctor.ZamEnd == null || doctor.ZamEnd < selectedDate.Date)
                        {
                            Doctor = doctor;
                        }
                        else
                        {
                            if (schedule.ZamID != null)
                            {
                                Doctor = dbAccess.GetDoctor(schedule.ZamID.Value);
                            }
                            else
                            {
                                Doctor = null;
                            }
                        }
                    }
                }
                if (schedule != null)
                {
                    setTimes();
                }
            }
        }

        private string daysToString(string s)
        {
            switch (s.ToLower())
            {
                case "понедельник":
                    return daysList[0];
                case "вторник":
                    return daysList[1];
                case "среда":
                    return daysList[2];
                case "четверг":
                    return daysList[3];
                case "пятница":
                    return daysList[4];
                case "суббота":
                    return daysList[5];
                case "воскресенье":
                    return daysList[6];
            }
            return "";
        }

        private void setDays()
        {
            var doctor = dbAccess.GetDoctors().Where(i => i.PlaceOfSee == selectedPatient.PlaceOfSee && i.SpecializationID == selectedSpecialization.ID).FirstOrDefault();
            if (doctor != null)
            {
                var schedules = dbAccess.GetDoctorMainSchedule(doctor.ID).OrderBy(i => i.dayofWeek);
                foreach (ScheduleModel sc in schedules)
                {
                    Days += daysToString(sc.Day) + " ";
                }
            }
            else
            {
                Days = null;
            }
        }

        private void setTimes()
        {
            if (doctor != null)
            {
                List<DoctorSeeModel> doctorSees = dbAccess.GetDoctorSees(schedule.DoctorID);
                if (doctorSees != null)
                {
                    SelectedTime = -1;
                    doctorSees = doctorSees.Where(i => i.Visited == null && i.DateTime.Date == SelectedDate.Date).ToList();
                    Times.Clear();
                    if (schedule != null)
                    {
                        ShiftModel shift = dbAccess.GetShift(schedule.ShiftID);
                        if (shift != null)
                        {
                            int start = Int32.Parse(shift.TimeofBegin.TotalMinutes.ToString());
                            int end = Int32.Parse(shift.TimeofEnd.TotalMinutes.ToString());
                            for (int i = start; i <= end - 15; i += 15)
                            {
                                if (doctorSees.Where(j => j.DateTime.TimeOfDay.TotalMinutes == i).FirstOrDefault() == null)
                                {
                                    String h;
                                    if (i / 60 > 9)
                                    {
                                        h = (i / 60).ToString();
                                    }
                                    else
                                    {
                                        h = '0' + (i / 60).ToString();
                                    }
                                    String m;
                                    if (i % 60 > 9)
                                    {
                                        m = (i % 60).ToString();
                                    }
                                    else
                                    {
                                        m = '0' + (i % 60).ToString();
                                    }
                                    Times.Add(h + ':' + m);
                                }
                            }
                        }
                    }
                }
            }
        }
        public RegistrationViewModel(Window window)
        {
            dbAccess = new DBAccess();
            Times = new ObservableCollection<string>();
            SelectedTime = -1;
            Doctor = null;
            List<PatientModel> patients = dbAccess.GetPatients(true);
            if (patients != null)
            {
                Patients = new ObservableCollection<PatientModel>(patients);
            }
            List<SpecializationModel> specializations = dbAccess.GetSpecializations();
            Specializations = new ObservableCollection<SpecializationModel>(specializations);
            SelectedPatient = null;
            SelectedDate = DateTime.Now;
            SelectedSpecialization = null;

            Submit = new Command(obj =>
            {
                DoctorSeeModel doctorSee = new DoctorSeeModel();
                doctorSee.DateTime = DateTime.Parse(SelectedDate.Date.ToString().Substring(0, 10) + ' ' + Times[SelectedTime].ToString());
                doctorSee.PatientID = selectedPatient.ID;
                if (schedule.ZamID != null && schedule.ZamID.Value != -1 && dbAccess.GetDoctor(schedule.DoctorID).ZamEnd.Value.Date >= selectedDate.Date)
                {
                    doctorSee.ZamID = schedule.ZamID.Value;
                }
                else
                {
                    doctorSee.ZamID = null;
                }
                doctorSee.DoctorID = schedule.DoctorID;
                doctorSee.Visited = null;
                doctorSee.Closed = false;
                List<DoctorSeeModel> doctorSees = dbAccess.GetDoctorSees(doctor.ID); // проверка что место еще не занято
                doctorSees = doctorSees.Where(i => i.Visited == null && i.DateTime.Date == SelectedDate.Date).ToList();
                if (doctorSees.Where(j => j.DateTime.TimeOfDay.TotalMinutes == doctorSee.DateTime.TimeOfDay.TotalMinutes).FirstOrDefault() == null)
                {
                    doctorSee.ID = dbAccess.addDoctorSee(doctorSee);
                    doctorSees = dbAccess.GetDoctorSees(doctor.ID); // перепроверка на дублирование
                    doctorSees = doctorSees.Where(i => i.Visited == null && i.DateTime.Date == SelectedDate.Date).ToList();
                    if (doctorSees.Where(j => j.DateTime.TimeOfDay.TotalMinutes == doctorSee.DateTime.TimeOfDay.TotalMinutes).ToList().Count > 1)
                    {
                        dbAccess.removeDoctorSee(doctorSee.ID);
                        MessageBox.Show("Место уже занято", "Ошибка");
                        setTimes();
                    }
                    else
                    {
                        print(doctorSee, schedule);
                        MessageBox.Show("Добавлено", "Успех");
                        SelectedTime = -1;
                        doctor = null;
                        SelectedPatient = null;
                        SelectedDate = DateTime.Now;
                        SelectedSpecialization = null;
                        Times.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Место уже занято", "Ошибка");
                    setTimes();
                }
            }, obj => {
                return SelectedTime != -1 && (SelectedDate.Date != DateTime.Now.Date || DateTime.Parse(Times[SelectedTime]).TimeOfDay.TotalMinutes > DateTime.Now.TimeOfDay.TotalMinutes);
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

        private void print(DoctorSeeModel doctorSee, ScheduleModel schedule)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics xg = XGraphics.FromPdfPage(page);
            int str = 20;
            var img = Registry.Properties.Resources.hospital;
            MemoryStream ms = new MemoryStream();
            img.Save(ms, img.RawFormat);
            XImage image = XImage.FromStream(ms);
            ms.Close();
            xg.DrawImage(image, 20, 64, 64, 64);
            xg.DrawString("Healthy".ToString(), new XFont("Times New Roman", 18), XBrushes.Black, new XRect(94, 64, page.Width - 94, 64), XStringFormats.CenterLeft);
            str += 100;
            xg.DrawString("Талон №" + doctorSee.ID.ToString(), new XFont("Times New Roman", 24), XBrushes.Black, new XRect(20, str, page.Width - 20, 24), XStringFormats.TopCenter);
            str += 30;
            xg.DrawString("Пациент: " + SelectedPatient.FullName, new XFont("Times New Roman", 16), XBrushes.Black, new XRect(20, str, page.Width - 20, 18), XStringFormats.TopLeft);
            str += 30;
            string name;
            if (doctorSee.ZamID == null)
            {
                name = dbAccess.GetDoctor(doctorSee.DoctorID).FullName;
            }
            else
            {
                name = dbAccess.GetDoctor(doctorSee.ZamID.Value).FullName;
            }
            xg.DrawString("Врач: " + name, new XFont("Times New Roman", 16), XBrushes.Black, new XRect(20, str, page.Width - 20, 18), XStringFormats.TopLeft);
            str += 30;
            xg.DrawString("Кабинет: " + schedule.Room, new XFont("Times New Roman", 16), XBrushes.Black, new XRect(20, str, page.Width - 20, 18), XStringFormats.TopLeft);
            str += 30;
            xg.DrawString("Время: " + doctorSee.DateTime, new XFont("Times New Roman", 16), XBrushes.Black, new XRect(20, str, page.Width - 20, 18), XStringFormats.TopLeft);
            document.Save(doctorSee.ID.ToString() + ".pdf");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
