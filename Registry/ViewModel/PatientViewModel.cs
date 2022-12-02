using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Registry.Model;
using Registry.View;

namespace Registry.ViewModel
{
    public class PatientViewModel : INotifyPropertyChanged
    {
        public List<string> Places { get; set; }
        public ObservableCollection<PatientModel> patients;
        public ObservableCollection<PatientModel> Patients
        {
            get { return patients; }
            set
            {
                patients = value;
                OnPropertyChanged("Patients");
            }
        }
        private PatientModel selectedPatient;
        public PatientModel SelectedPatient
        {
            get { return selectedPatient; }
            set
            {
                selectedPatient = value;
                if (value != null)
                {
                    FIO = selectedPatient.FullName;
                    if (selectedPatient.Male == "Мужской")
                    {
                        Male = 0;
                    }
                    else
                    {
                        Male = 1;
                    }
                    Document = selectedPatient.Document;
                    if (selectedPatient.PlaceOfWork != null)
                    {
                        PlaceOfWork = selectedPatient.PlaceOfWork;
                    }
                    else
                    {
                        PlaceOfWork = "";
                    }
                    DateOfBirth = selectedPatient.DateOfBirth.ToString().Substring(0, 10);
                    SelectedStreet = Streets.Where(i => i.ID == selectedPatient.StreetID).FirstOrDefault();
                    Adress = selectedPatient.Address;
                }
                else
                {
                    FIO = "";
                    Male = -1;
                    Document = "";
                    PlaceOfWork = "";
                    DateOfBirth = "";
                    SelectedStreet = null;
                    Adress = "";
                }
                OnPropertyChanged("SelectedPatient");
            }
        }
        public List<StreetsModel> Streets { get; set; }

        private int currentPlace;
        public int CurrentPlace
        {
            get { return currentPlace; }
            set
            {
                currentPlace = value;
                if (currentPlace > -1)
                {
                    setPatients();
                }
                OnPropertyChanged("CurrentPlace");
            }
        }
        private StreetsModel selectedStreet;
        public StreetsModel SelectedStreet
        {
            get { return selectedStreet; }
            set
            {
                selectedStreet = value;
                OnPropertyChanged("SelectedStreet");
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

        private int male;
        public int Male
        {
            get { return male; }
            set
            {
                male = value;
                OnPropertyChanged("Male");
            }
        }
        private string document;
        public string Document
        {
            get { return document; }
            set
            {
                document = value;
                OnPropertyChanged("Document");
            }
        }

        private string placeOfWork;
        public string PlaceOfWork
        {
            get { return placeOfWork; }
            set
            {
                placeOfWork = value;
                OnPropertyChanged("PlaceOfWork");
            }
        }
        private string dateOfBirth;
        public string DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                dateOfBirth = value;
                OnPropertyChanged("DateOfBirth");
            }
        }
        private string adress;
        public string Adress
        {
            get { return adress; }
            set
            {
                adress = value;
                OnPropertyChanged("Adress");
            }
        }
        private DBAccess dbAccess;
        private Window thisWindow;

        public PatientViewModel(Window thisWindow)
        {
            dbAccess = new DBAccess();
            Places = new List<string>();
            Places.Add("");
            Streets = dbAccess.GetStreets();
            foreach (StreetsModel st in Streets)
            {
                if (Places.Where(i => st.PlaceOfSee.ToString() == i).ToList().Count == 0)
                {
                    Places.Add(st.PlaceOfSee.ToString());
                }
            }
            CurrentPlace = 0;
            this.thisWindow = thisWindow;
            
            commands();
        }

        private void setPatients()
        {
            SelectedPatient = null;
            var buf = dbAccess.GetPatients(false);
            if (currentPlace > 0)
            {
                buf = buf.Where(i => i.PlaceOfSee == int.Parse(Places[currentPlace])).ToList();
            }
            Patients = new ObservableCollection<PatientModel>(buf);
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
                PatientModel patient = new PatientModel();
                patient.FullName = fio;
                if (male == 0)
                {
                    patient.Male = "Мужской";
                }
                else
                {
                    patient.Male = "Женский";
                }
                patient.Document = document;
                if (placeOfWork != null)
                {
                    patient.PlaceOfWork = placeOfWork;
                }
                else
                {
                    patient.PlaceOfWork = "";
                }
                patient.DateOfBirth = dateOfBirth;
                patient.StreetID = selectedStreet.ID;
                patient.Address = adress;
                patient.ID = dbAccess.CreatePatient(patient);
                Patients.Add(patient);
            }, obj =>
            {
                return 
                fio != "" &&
                (male == 0 || male == 1) &&
                document != "" &&
                dateOfBirth.Length == 10 && DateTime.TryParse(dateOfBirth, out DateTime b) && 
                selectedStreet != null &&
                adress != "";
            });
            Update = new Command(obj =>
            {
                selectedPatient.FullName = fio;
                if (male == 0)
                {
                    selectedPatient.Male = "Мужской";
                }
                else
                {
                    selectedPatient.Male = "Женский";
                }
                selectedPatient.Document = document;
                if (placeOfWork != null)
                {
                    selectedPatient.PlaceOfWork = placeOfWork;
                }
                else
                {
                    selectedPatient.PlaceOfWork = "";
                }
                selectedPatient.DateOfBirth = dateOfBirth;
                selectedPatient.StreetID = selectedStreet.ID;
                bool f = false;
                if (selectedPatient.PlaceOfSee != selectedStreet.PlaceOfSee && currentPlace != 0)
                {
                    f = true;
                }
                selectedPatient.PlaceOfSee = selectedStreet.PlaceOfSee;
                selectedPatient.Address = adress;
                dbAccess.UpdatePatient(selectedPatient);
                if (f)
                {
                    Patients.Remove(selectedPatient);
                }
            }, obj =>
            {
                return
                selectedPatient != null &&
                fio != "" &&
                (male == 0 || male == 1) &&
                document != "" &&
                dateOfBirth.Length == 10 && DateTime.TryParse(dateOfBirth, out DateTime b) &&
                selectedStreet != null &&
                adress != "";
            });
            Restore = new Command(obj =>
            {
                selectedPatient.Closed = false;
                dbAccess.UpdatePatient(selectedPatient);
                setPatients();
            }, obj =>
            {
                return
                selectedPatient != null && selectedPatient.Closed == true;
            });
            Delete = new Command(obj =>
            {
                selectedPatient.Closed = true;
                dbAccess.UpdatePatient(selectedPatient);
                var sees = dbAccess.GetDoctorSeesForPatient(selectedPatient.ID).Where(i => i.Visited == null);
                foreach (DoctorSeeModel s in sees)
                {
                    s.Closed = true;
                    s.Visited = false;
                    dbAccess.UpdateDoctorSee(s);
                }
                var proc = dbAccess.GetProceduresForPatient(selectedPatient.ID);
                foreach (ProcedureModel p in proc)
                {
                    p.Visited = false;
                    dbAccess.UpdateProcedure(p);
                }
                setPatients();
            }, obj =>
            {
                return
                selectedPatient != null && selectedPatient.Closed == false;
            });
        }

        public Command Exit { get; set; }
        public Command Create { get; set; }
        public Command Update { get; set; }
        public Command Restore { get; set; }
        public Command Delete { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
