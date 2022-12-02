using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class PatientModel : INotifyPropertyChanged
    {
        private string fullName;
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            }
        }

        public string Male { get; set; }

        public int ID { get; set; }

        public string Address { get; set; }

        public string PlaceOfWork { get; set; }

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

        public bool Closed { get; set; }

        public string Document { get; set; }
        private int placeOfSee;
        public int PlaceOfSee
        {
            get { return placeOfSee; }
            set
            {
                placeOfSee = value;
                OnPropertyChanged("PlaceOfSee");
            }
        }

        public int StreetID { get; set; }

        public PatientModel()
        {

        }

        public PatientModel(Patient patient)
        {
            DBAccess DB = new DBAccess();
            FullName = patient.FullName;
            Male = patient.Male;
            ID = patient.ID;
            StreetID = patient.StreetID;
            Address = patient.Address;
            PlaceOfWork = patient.PlaceOfWork;
            DateOfBirth = patient.DateOfBirth.Date.ToString().Substring(0, 10);
            Document = patient.Document;
            PlaceOfSee = DB.GetPlaceOfSee(StreetID);
            Closed = patient.Closed;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
