using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class NurceModel : INotifyPropertyChanged
    {
        public int ID { get; set; }

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
        public bool Closed { get; set; }
        private int workEx;
        public int WorkEx
        {
            get { return workEx; }
            set
            {
                workEx = value;
                OnPropertyChanged("WorkEx");
            }
        }

        public ObservableCollection<TypeofProcModel> Procedures { get; set; }
        public List<MakingProcedureModel> MakingProcedures { get; set; }

        public NurceModel()
        {

        }
        public NurceModel(Nurce nurce)
        {
            ID = nurce.ID;
            FullName = nurce.FullName;
            WorkEx = nurce.WorkEx;
            Closed = nurce.Closed;
            Procedures = new ObservableCollection<TypeofProcModel>();
        }

        public void SetProcedures()
        {
            DBAccess DB = new DBAccess();
            MakingProcedures = DB.GetMakingProcedures(ID);
            for (int i = 0; i < MakingProcedures.Count; i++)
            {
                Procedures.Add(DB.GetTypeProc(MakingProcedures[i].ProcedureID));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
