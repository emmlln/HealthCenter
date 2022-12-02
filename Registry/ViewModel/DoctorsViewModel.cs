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
    public class DoctorsViewModel : INotifyPropertyChanged
    {
        public List<string> Places { get; set; }
        public List<string> NPlaces { get; set; }
        public List<SpecializationModel> NSpecializations { get; set; }
        public List<SpecializationModel> Specializations { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public ObservableCollection<DoctorModel> doctors;
        public ObservableCollection<DoctorModel> Doctors
        {
            get { return doctors; }
            set
            {
                doctors = value;
                OnPropertyChanged("Doctors");
            }
        }
        public DoctorModel changingDoctor;
        public DoctorModel ChangingDoctor
        {
            get { return changingDoctor; }
            set
            {
                changingDoctor = value;
                OnPropertyChanged("ChangingDoctor");
            }
        }
        private DoctorModel selectedDoctor;
        public DoctorModel SelectedDoctor
        {
            get { return selectedDoctor; }
            set
            {
                selectedDoctor = value;
                if (value != null)
                {
                    FIO = selectedDoctor.FullName;
                    if (selectedDoctor.Certificate == true)
                    {
                        SelectedCertificate = 0;
                    }
                    else
                    {
                        SelectedCertificate = 1;
                    }
                    SelectedCategory = Categories.Where(i => i.ID == selectedDoctor.CategoryID).FirstOrDefault();
                    SelectedSpecialization = Specializations.Where(i => i.ID == selectedDoctor.SpecializationID).FirstOrDefault();
                    SelectedPlace = Places.IndexOf(selectedDoctor.PlaceOfSee.ToString());
                }
                else
                {
                    FIO = "";
                    SelectedCertificate = 0;
                    SelectedCategory = null;
                    SelectedSpecialization = null;
                    SelectedPlace = -1;
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
                if (currentPlace > -1)
                {
                    setDoctors();
                }
                OnPropertyChanged("CurrentPlace");
            }
        }
        private int currentSpecialization;
        public int CurrentSpecialization
        {
            get { return currentSpecialization; }
            set
            {
                currentSpecialization = value;
                if (currentSpecialization > -1)
                {
                    setDoctors();
                }
                OnPropertyChanged("CurrentSpecialization");
            }
        }
        private int selectedPlace;
        public int SelectedPlace
        {
            get { return selectedPlace; }
            set
            {
                selectedPlace = value;
                ChangingDoctor = null;
                if (selectedSpecialization != null && selectedPlace > -1)
                {
                    var b = dbAccess.GetDoctors().Where(i => i.SpecializationID == selectedSpecialization.ID && i.PlaceOfSee.ToString() == Places[selectedPlace]).FirstOrDefault();
                    if (b != null)
                    {
                        ChangingDoctor = b;
                    }
                }
                OnPropertyChanged("SelectedPlace");
            }
        }
        private SpecializationModel selectedSpecialization;
        public SpecializationModel SelectedSpecialization
        {
            get { return selectedSpecialization; }
            set
            {
                selectedSpecialization = value;
                OnPropertyChanged("SelectedSpecialization");
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

        private int selectedCertificate;
        public int SelectedCertificate
        {
            get { return selectedCertificate; }
            set
            {
                selectedCertificate = value;
                OnPropertyChanged("SelectedCertificate");
            }
        }
        private CategoryModel selectedCategory;
        public CategoryModel SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }

        private DBAccess dbAccess;
        private Window thisWindow;

        public DoctorsViewModel(Window thisWindow)
        {
            dbAccess = new DBAccess();
            Categories = dbAccess.GetCategories();
            NSpecializations = new List<SpecializationModel>();
            NSpecializations.Add(new SpecializationModel());
            Specializations = dbAccess.GetSpecializations();
            NSpecializations.AddRange(Specializations);
            Places = new List<string>();
            NPlaces = new List<string>();
            NPlaces.Add("");
            var Streets = dbAccess.GetStreets();
            foreach (StreetsModel st in Streets)
            {
                if (Places.Where(i => st.PlaceOfSee.ToString() == i).ToList().Count == 0)
                {
                    Places.Add(st.PlaceOfSee.ToString());
                }
            }
            NPlaces.AddRange(Places);
            CurrentPlace = 0;
            this.thisWindow = thisWindow;

            commands();
        }

        private void setDoctors()
        {
            SelectedDoctor = null;
            var buf = dbAccess.GetDoctors();
            if (currentPlace > 0)
            {
                buf = buf.Where(i => i.PlaceOfSee == int.Parse(NPlaces[currentPlace])).ToList();
            }
            if (currentSpecialization > 0)
            {
                buf = buf.Where(i => i.SpecializationID == NSpecializations[currentSpecialization].ID).ToList();
            }
            Doctors = new ObservableCollection<DoctorModel>(buf);
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
                DoctorModel doctor = new DoctorModel();
                doctor.FullName = fio;
                if (selectedCertificate == 0)
                {
                    doctor.Certificate = true;
                }
                else
                {
                    doctor.Certificate = false;
                }
                doctor.SpecializationID = selectedSpecialization.ID;
                doctor.CategoryID = selectedCategory.ID;
                doctor.PlaceOfSee = int.Parse(Places[selectedPlace]);
                doctor.ID = dbAccess.CreateDoctor(doctor);
                var oldDoctor = dbAccess.GetDoctors().Where(i => i.PlaceOfSee.ToString() == Places[selectedPlace]).FirstOrDefault();
                if (oldDoctor != null)
                {
                    oldDoctor.Closed = true;
                    dbAccess.UpdateDoctor(oldDoctor);
                    var firstsc = dbAccess.GetDoctorSchedule(oldDoctor.ID);
                    var firstDS = dbAccess.GetDoctorSees(oldDoctor.ID).Where(i => i.Visited == null && i.Closed == false).ToList();
                    foreach (ScheduleModel s in firstsc)
                    {
                        if (s.DoctorID == oldDoctor.ID)
                        {
                            s.DoctorID = doctor.ID;
                        }
                        else
                        {
                            s.ZamID = doctor.ID;
                        }
                        dbAccess.UpdateSchedule(s);
                    }
                    foreach (DoctorSeeModel p in firstDS)
                    {
                        if (p.DoctorID == oldDoctor.ID)
                        {
                            p.DoctorID = doctor.ID;
                        }
                        else
                        {
                            p.ZamID = doctor.ID;
                        }
                        dbAccess.UpdateDoctorSee(p);
                    }
                }
                if (currentPlace == 0 || NPlaces[currentPlace] == doctor.PlaceOfSee.ToString() && currentSpecialization == 0 || NSpecializations[currentSpecialization].ID == doctor.SpecializationID)
                {
                    doctor.Specialization = selectedSpecialization.SpecializationName;
                    Doctors.Add(doctor);
                }
            }, obj =>
            {
                return
                fio != "" &&
                (selectedCertificate == 0 || selectedCertificate == 1) &&
                selectedCategory != null &&
                selectedPlace > -1 &&
                selectedSpecialization != null;
            });
            Update = new Command(obj =>
            {
                selectedDoctor.FullName = fio;
                if (selectedCertificate == 0)
                {
                    selectedDoctor.Certificate = true;
                }
                else
                {
                    selectedDoctor.Certificate = false;
                }
                selectedDoctor.CategoryID = selectedCategory.ID;
                dbAccess.UpdateDoctor(selectedDoctor);
                if (selectedDoctor.PlaceOfSee.ToString() != Places[selectedPlace])
                {
                    var firstsc = dbAccess.GetDoctorSchedule(selectedDoctor.ID);
                    var secondsc = dbAccess.GetDoctorSchedule(changingDoctor.ID);
                    var firstDS = dbAccess.GetDoctorSees(selectedDoctor.ID).Where(i => i.Visited == null && i.Closed == false).ToList();
                    var secondDS = dbAccess.GetDoctorSees(changingDoctor.ID).Where(i => i.Visited == null && i.Closed == false).ToList();
                    foreach (ScheduleModel s in firstsc)
                    {
                        if (s.DoctorID == selectedDoctor.ID)
                        {
                            s.DoctorID = changingDoctor.ID;
                        }
                        else
                        {
                            s.ZamID = changingDoctor.ID;
                        }
                        dbAccess.UpdateSchedule(s);
                    }
                    foreach (ScheduleModel s in secondsc)
                    {
                        if (s.DoctorID == changingDoctor.ID)
                        {
                            s.DoctorID = selectedDoctor.ID;
                        }
                        else
                        {
                            s.ZamID = selectedDoctor.ID;
                        }
                        dbAccess.UpdateSchedule(s);
                    }
                    foreach (DoctorSeeModel p in firstDS)
                    {
                        if (p.DoctorID == selectedDoctor.ID)
                        {
                            p.DoctorID = changingDoctor.ID;
                        }
                        else
                        {
                            p.ZamID = changingDoctor.ID;
                        }
                        dbAccess.UpdateDoctorSee(p);
                    }
                    foreach (DoctorSeeModel p in secondDS)
                    {
                        if (p.DoctorID == changingDoctor.ID)
                        {
                            p.DoctorID = selectedDoctor.ID;
                        }
                        else
                        {
                            p.ZamID = selectedDoctor.ID;
                        }
                        dbAccess.UpdateDoctorSee(p);
                    }
                    int place = selectedDoctor.PlaceOfSee;
                    SelectedDoctor.PlaceOfSee = changingDoctor.PlaceOfSee;
                    ChangingDoctor.PlaceOfSee = place;
                    dbAccess.UpdateDoctor(selectedDoctor);
                    dbAccess.UpdateDoctor(changingDoctor);
                    setDoctors();
                }
            }, obj =>
            {
                return
                 fio != "" && selectedDoctor != null &&
                 (selectedCertificate == 0 || selectedCertificate == 1) &&
                 selectedCategory != null &&
                 selectedPlace > -1 && dbAccess.GetDoctors().Where(i => i.PlaceOfSee.ToString() == Places[selectedPlace]).FirstOrDefault() != null &&
                 selectedSpecialization != null && selectedSpecialization.ID == selectedDoctor.SpecializationID;
            });
        }

        public Command Exit { get; set; }
        public Command Create { get; set; }
        public Command Update { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
