using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class DoctorModel : INotifyPropertyChanged
    {
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
        public bool Closed { get; set; }
        public int SpecializationID { get; set; }
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
        public int ID { get; set; }
        public int? ZamID { get; set; }
        public int CategoryID { get; set; }

        public bool Certificate { get; set; }
        public DateTime? ZamEnd { get; set; }
        public DateTime? ZamStart { get; set; }
        public int Workload { get; set; }
        public int CurrentWorkload { get; set; }
        public List<string> Shifts { get; set; }
        public List<int> Days { get; set; }
        private string specialization;
        public string Specialization
        {
            get { return specialization; }
            set
            {
                specialization = value;
                OnPropertyChanged("Specialization");
            }
        }

        public DoctorModel()
        {

        }

        public DoctorModel(Doctor doctor)
        {
            DBAccess DB = new DBAccess();
            Shifts = new List<string>();
            Days = new List<int>();
            PlaceOfSee = doctor.PlaceOfSee;
            Specialization = DB.GetSpecializations().Where(i => i.ID == doctor.SpecializationID).FirstOrDefault().SpecializationName;
            SpecializationID = doctor.SpecializationID;
            FullName = doctor.FullName;
            ID = doctor.ID;
            CategoryID = doctor.CategoryID;
            Certificate = doctor.Certificate;
            ZamEnd = doctor.ZamEnd;
            ZamStart = doctor.ZamStart;
            Closed = doctor.Closed;
            ZamID = doctor.ZamID;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
