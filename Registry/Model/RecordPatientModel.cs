using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class RecordPatientModel
    {
        public int ID { get; set; }
        public int PatientID { get; set; }
        public DateTime Date { get; set; }
        public string Result { get; set; }
        public string Diagnosis { get; set; }
        public int? DoctorID { get; set; }
        public int? NurseID { get; set; }

        public RecordPatientModel()
        {

        }

        public RecordPatientModel(RecordInPatientFile record)
        {
            ID = record.ID;
            PatientID = record.PatientID;
            Date = record.Date;
            Result = record.Result;
            Diagnosis = record.Diagnosis;
            DoctorID = record.DoctorID;
            NurseID = record.NurseID;
        }

        public string ToText()
        {
            DBAccess db = new DBAccess();
            string result = "";
            if (DoctorID != null)
            {
                result += "Прием у врача: ";
                result += db.GetDoctor(DoctorID.Value).FullName + "\n";
                result += "Пациент: ";
                result += db.GetPatient(PatientID).FullName + "\n";
                result += "Диагноз: ";
                result += Diagnosis + "\n";
                result += "Результаты осмотра: ";
                result += Result + "\n";
                List<DoProcModel> proc = db.GetDoProc(ID);
                if (proc.Count > 0)
                {
                    result += "Назначенные процедуры: \n";
                    for (int i = 0; i < proc.Count; i++)
                    {
                        result += (i + 1) + ". " + db.GetTypeProc(proc[i].ProcID).Type + "\n";
                    }
                }
                result += "Дата приема: ";
                result += Date + "\n";
            }
            else
            {
                result += "Процедура: ";
                result += Diagnosis + "\n";
                result += "Медсестра: ";
                result += db.GetNurce(NurseID.Value).FullName + "\n";
                result += "Пациент: ";
                result += db.GetPatient(PatientID).FullName + "\n";
                result += "Результаты: ";
                result += Result + "\n";
                result += "Дата проведения: ";
                result += Date + "\n";
            }
            return result;
        }
    }
}
