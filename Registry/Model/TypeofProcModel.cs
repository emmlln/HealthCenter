using DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class TypeofProcModel : INotifyPropertyChanged
    {
        public int ID { get; set; }
        private string type;
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        public TypeofProcModel()
        {

        }
        public TypeofProcModel(TypeofProc proc)
        {
            ID = proc.ID;
            Type = proc.Type;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
