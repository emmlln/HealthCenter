using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class ScheduleProcedureModel : INotifyPropertyChanged
    {
        public int ID { get; set; }

        private int room;
        public int Room
        {
            get { return room; }
            set
            {
                room = value;
                OnPropertyChanged("Room");
            }
        }

        public int DayID { get; set; }

        public int ProcedureID { get; set; }

        private int count;
        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                OnPropertyChanged("Count");
            }
        }

        private string procedure;
        public string Procedure
        {
            get { return procedure; }
            set
            {
                procedure = value;
                OnPropertyChanged("Procedure");
            }
        }

        public bool Closed { get; set; }

        public ScheduleProcedureModel()
        {

        }
        public ScheduleProcedureModel(ScheduleProcedure schedule)
        {
            DBAccess DB = new DBAccess();
            ID = schedule.ID;
            Room = schedule.Room;
            Count = schedule.Count;
            ProcedureID = schedule.ProcedureID;
            DayID = schedule.DayID;
            Closed = schedule.Closed;
            Procedure = DB.GetTypeProc(ProcedureID).Type;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
