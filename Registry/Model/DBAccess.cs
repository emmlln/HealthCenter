using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Registry.Model
{
    public class DBAccess
    {
        private void DBException()
        {
            MessageBox.Show("Ошибка подключения к базе, приложение будет закрыто", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            Environment.Exit(0);
        }
        private DBContext setDB()
        {
            DBContext db = new DBContext();
            try
            {
                if (db.Database.Exists())
                {
                    return db;
                }
                else
                {
                    DBException();
                    return null;
                }
            }
            catch (System.InvalidOperationException)
            {
                DBException();
                return null;
            }
        }

        public List<PatientModel> GetPatients(bool closed)
        {
            DBContext DB = setDB();
            if (DB != null)
            {
                var buffer = DB.Patient.ToList();
                if (closed)
                {
                    buffer = buffer.Where(i => i.Closed == false).ToList();
                }
                buffer = buffer.OrderBy(i => i.Closed).ToList();
                return buffer.Select(i => new PatientModel(i)).ToList();
            }
            else
            {
                return null;
            }
        }

        public PatientModel GetPatient(int id)
        {
            DBContext DB = setDB();
            return new PatientModel(DB.Patient.Where(i => i.ID == id).FirstOrDefault());
        }

        public List<DoctorModel> GetDoctors()
        {
            DBContext DB = setDB();
            if (DB != null)
            {
                return DB.Doctor.Where(i => i.Closed == false).ToList().Select(i => new DoctorModel(i)).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<SpecializationModel> GetSpecializations()
        {
            DBContext DB = setDB();
            if (DB != null)
            {
                return DB.Specialization.ToList().Select(i => new SpecializationModel(i)).ToList();
            }
            else
            {
                return null;
            }
        }
        
        public List<DoctorSeeModel> GetDoctorSees(int doctorid)
        {
            DBContext DB = setDB();
            if (DB != null)
            {
                return DB.DoctorSee.Where(i => i.DoctorID == doctorid).ToList().Select(i => new DoctorSeeModel(i)).ToList();
            }
            else
            {
                return null;
            }
        }
        public List<DoctorSeeModel> GetDoctorSeesForPatient(int patientID)
        {
            DBContext DB = setDB();
            if (DB != null)
            {
                return DB.DoctorSee.Where(i => i.PatientID == patientID).ToList().Select(i => new DoctorSeeModel(i)).ToList();
            }
            else
            {
                return null;
            }
        }

        public List<ScheduleModel> GetDayScheduleForDoctors(string day)
        {
            DBContext DB = setDB();
            int? dayId = GetDayWeekId(day);
            if (DB != null && dayId != null)
            {
                return DB.Schedule.Where(i => i.dayofWeek == dayId.Value).ToList().Select(i => new ScheduleModel(i)).ToList();
            }
            else
            {
                return null;
            }
        }

        public int? GetDayWeekId(string day)
        {
            switch (day.ToLower())
            {
                case "monday": day = "понедельник"; break;
                case "tuesday": day = "вторник"; break;
                case "wednesday": day = "среда"; break;
                case "thursday": day = "четверг"; break;
                case "friday": day = "пятница"; break;
                case "saturday": day = "суббота"; break;
                case "sunday": day = "воскресенье"; break;
            }
            DBContext DB = setDB();
            if (DB != null)
            {
                Days cday = DB.Days.Where(i => i.DayOfWeek.ToLower() == day).FirstOrDefault();
                if (cday == null)
                {
                    return null;
                }
                return cday.ID;
            }
            else
            {
                return null;
            }
        }

        public int addDoctorSee(DoctorSeeModel doctorSee)
        {
            DBContext DB = setDB();
            DoctorSee ds = new DoctorSee();
            ds.DateTime = doctorSee.DateTime;
            ds.DoctorID = doctorSee.DoctorID;
            ds.Visited = doctorSee.Visited;
            ds.PatientID = doctorSee.PatientID;
            ds.ZamID = doctorSee.ZamID;
            DB.DoctorSee.Add(ds);
            Save(DB);
            return ds.ID;
        }

        public DoctorModel GetDoctor(int id)
        {
            DBContext DB = setDB();
            return new DoctorModel(DB.Doctor.Where(i => i.ID == id).FirstOrDefault());
        }

        public void UpdateDoctorSee(DoctorSeeModel doctorSee)
        {
            DBContext DB = setDB();
            DoctorSee ds = new DoctorSee();
            ds.ID = doctorSee.ID;
            ds.DateTime = doctorSee.DateTime;
            ds.DoctorID = doctorSee.DoctorID;
            ds.Visited = doctorSee.Visited;
            ds.PatientID = doctorSee.PatientID;
            ds.Closed = doctorSee.Closed;
            ds.ZamID = doctorSee.ZamID;
            DB.Entry(ds).State = System.Data.Entity.EntityState.Modified;
            Save(DB);
        }

        public ShiftModel GetShift(int id)
        {
            DBContext DB = setDB();
            Shifts shift = DB.Shifts.FirstOrDefault(i => i.ID == id);
            if (shift != null)
            {
                return new ShiftModel(shift);
            }
            else
            {
                return null;
            }
        }
        public List<ShiftModel> GetShifts()
        {
            DBContext DB = setDB();
            List<Shifts> shifts = DB.Shifts.ToList();
            return shifts.Select(i => new ShiftModel(i)).ToList();
        }

        public string GetDay(int id)
        {
            DBContext DB = setDB();
            return DB.Days.Where(i => i.ID == id).First().DayOfWeek;
        }

        public List<DaysModel> GetDays()
        {
            DBContext DB = setDB();
            return DB.Days.ToList().Select(i => new DaysModel(i)).ToList();
        }

        public void removeDoctorSee(int id)
        {
            DBContext DB = setDB();
            DoctorSee item = DB.DoctorSee.Find(id);
            if (item != null)
            {
                DB.DoctorSee.Remove(item);
                Save(DB);
            }
        }
        public void removeSchedule(int id)
        {
            DBContext DB = setDB();
            Schedule item = DB.Schedule.Find(id);
            if (item != null)
            {
                DB.Schedule.Remove(item);
                Save(DB);
            }   
        }

        public string GetDoctorShiftsForDay(int doctorId, int dayId, int changingId, DateTime start, DateTime end)
        {
            DBContext DB = setDB();
            Schedule sc = DB.Schedule.Where(i => i.DoctorID == doctorId && i.dayofWeek == dayId).FirstOrDefault();
            List<Schedule> schedules = DB.Schedule.Where(i => i.ZamID != null && i.ZamID.Value == doctorId && i.dayofWeek == dayId).ToList();
            schedules = schedules.Where(i => i.DoctorID != changingId && GetDoctor(i.DoctorID).ZamEnd != null && GetDoctor(i.DoctorID).ZamEnd.Value >= start && GetDoctor(i.DoctorID).ZamStart != null && GetDoctor(i.DoctorID).ZamStart.Value <= end).ToList();
            if (sc != null)
            {
                schedules.Add(sc);
            }
            string result = "";
            foreach(Schedule s in schedules)
            {
                result += " " + s.ShiftID.ToString();
            } 
            return result;
        }

        public List<ScheduleModel> GetDoctorSchedule(int doctorId)
        {
            DBContext DB = setDB();
            List<Schedule> sc = DB.Schedule.Where(i => i.DoctorID == doctorId).ToList();
            List<ScheduleModel> schedules = DB.Schedule.Where(i => i.ZamID != null && i.ZamID.Value == doctorId).ToList().Where(i => GetDoctor(i.DoctorID).ZamEnd != null && GetDoctor(i.DoctorID).ZamEnd.Value.Date >= DateTime.Now.Date).Select(i => new ScheduleModel(i)).ToList();
            schedules.AddRange(sc.Select(i => new ScheduleModel(i)).ToList());
            return schedules;
        }

        public List<ScheduleModel> GetDoctorMainSchedule(int doctorId)
        {
            DBContext DB = setDB();
            List<Schedule> sc = DB.Schedule.Where(i => i.DoctorID == doctorId).ToList();
            return sc.Select(i => new ScheduleModel(i)).ToList();
        }

        public void UpdateDoctor(DoctorModel d)
        {
            DBContext DB = setDB();
            Doctor doctor = new Doctor();
            doctor.ID = d.ID;
            doctor.FullName = d.FullName;
            doctor.PlaceOfSee = d.PlaceOfSee;
            doctor.SpecializationID = d.SpecializationID;
            doctor.CategoryID = d.CategoryID;
            doctor.Certificate = d.Certificate;
            doctor.ZamEnd = d.ZamEnd;
            doctor.ZamStart = d.ZamStart;
            doctor.Closed = d.Closed;
            doctor.ZamID = d.ZamID;
            DB.Entry(doctor).State = System.Data.Entity.EntityState.Modified;
            Save(DB);
        }

        public void UpdateSchedule(ScheduleModel s)
        {
            DBContext DB = setDB();
            Schedule schedule = new Schedule();
            schedule.dayofWeek = s.dayofWeek;
            schedule.DoctorID = s.DoctorID;
            schedule.ZamID = s.ZamID;
            schedule.ID = s.ID;
            schedule.Room = s.Room;
            schedule.ShiftID = s.ShiftID;
            DB.Entry(schedule).State = System.Data.Entity.EntityState.Modified;
            Save(DB);
        }

        public int GetDoctorSeesCount(int id, DateTime day)
        {
            DBContext DB = setDB();
            return DB.DoctorSee.Where(i => (i.ZamID == null && i.DoctorID == id || i.ZamID != null && i.ZamID == id) && i.Visited == null && i.Closed == false).ToList().Where(i => i.DateTime.Date <= day.Date).Count();
        }

        public List<DoctorSeeModel> GetDoctorSees(int id, DateTime day)
        {
            DBContext DB = setDB();
            List<DoctorSee> buffer = DB.DoctorSee.Where(i => (i.ZamID == null && i.DoctorID == id || i.ZamID != null && i.ZamID == id) && i.Visited == null && i.Closed == false).ToList().Where(i => i.DateTime.Date <= day.Date).ToList();
            return buffer.Select(i => new DoctorSeeModel(i)).ToList();
        }

        public UserModel GetUserByLogin(string login)
        {
            DBContext DB = setDB();
            Users user = DB.Users.Where(i => i.Login == login).FirstOrDefault();
            if (user != null)
            {
                return new UserModel(user);
            }
            else
            {
                return null;
            }
        }

        public List<TypeofProcModel> GetTypeProc()
        {
            DBContext DB = setDB();
            List<TypeofProc> buffer = DB.TypeofProc.ToList();
            return buffer.Select(i => new TypeofProcModel(i)).ToList();
        }

        public TypeofProcModel GetTypeProc(int id)
        {
            DBContext DB = setDB();
            TypeofProc buf = DB.TypeofProc.Where(i => i.ID == id).FirstOrDefault();
            return new TypeofProcModel(buf);
        }

        public int addPatientRecord(RecordPatientModel record)
        {
            DBContext DB = setDB();
            RecordInPatientFile rec = new RecordInPatientFile();
            rec.PatientID = record.PatientID;
            if (record.DoctorID != null)
            {
                rec.DoctorID = record.DoctorID;
            }
            else
            {
                rec.NurseID = record.NurseID;
            }
            rec.Diagnosis = record.Diagnosis;
            rec.Date = record.Date;
            rec.Result = record.Result;
            DB.RecordInPatientFile.Add(rec);
            Save(DB);
            return rec.ID;
        }

        public DoctorSeeModel GetDoctorSee(int id)
        {
            DBContext DB = setDB();
            DoctorSee buffer = DB.DoctorSee.Where(i => i.ID == id).FirstOrDefault();
            return new DoctorSeeModel(buffer);
        }

        public int AddDoProc(DoProcModel doProc)
        {
            DBContext DB = setDB();
            DoProc pr = new DoProc();
            pr.ProcID = doProc.ProcID;
            pr.RecordID = doProc.RecordID;
            pr.PatientID = doProc.PatientID;
            DB.DoProc.Add(pr);
            Save(DB);
            return pr.ID;
        }

        public List<DoProcModel> GetDoProc(int id)
        {
            DBContext DB = setDB();
            List<DoProc> buf = DB.DoProc.Where(i => i.RecordID == id).ToList();
            return buf.Select(i => new DoProcModel(i)).ToList();
        }

        public bool CheckBusyProcedure(int id)
        {
            DBContext DB = setDB();
            List<Procedure> buf = DB.Procedure.Where(i => i.DoProcID == id && (i.Visited == null || i.Visited.Value == true)).ToList();
            return buf.Count > 0;
        }

        public List<DoProcModel> GetDoProcForPatient(int id)
        {
            DBContext DB = setDB();
            List<DoProc> buf = DB.DoProc.Where(i => i.PatientID == id).ToList();
            return buf.Select(i => new DoProcModel(i)).ToList();
        }

        public DoProcModel GetDoProcByID(int id)
        {
            DBContext DB = setDB();
            return new DoProcModel(DB.DoProc.Where(i => i.ID == id).FirstOrDefault());
        }

        public List<RecordPatientModel> GetRecords(int id)
        {
            DBContext DB = setDB();
            List<RecordInPatientFile> buf = DB.RecordInPatientFile.Where(i => i.PatientID == id).ToList();
            return buf.Select(i => new RecordPatientModel(i)).ToList();
        }

        public List<ProcedureModel> GetProcedures(DateTime day)
        {
            DBContext DB = setDB();
            List<Procedure> buffer = DB.Procedure.Where(i => i.Visited == null).ToList().Where(i => i.Date.Date <= day.Date).ToList();
            return buffer.Select(i => new ProcedureModel(i)).ToList();
        }
        public List<ProcedureModel> GetProcedures()
        {
            DBContext DB = setDB();
            List<Procedure> buffer = DB.Procedure.Where(i => i.Visited == null).ToList();
            return buffer.Select(i => new ProcedureModel(i)).ToList();
        }
        public List<ProcedureModel> GetProceduresForPatient(int patientID)
        {
            DBContext DB = setDB();
            List<Procedure> buffer = DB.Procedure.Where(i => i.Visited == null).ToList().Where(i=> DB.DoProc.Where(j => j.ID == i.DoProcID).FirstOrDefault().PatientID == patientID).ToList();
            return buffer.Select(i => new ProcedureModel(i)).ToList();
        }

        public void UpdateProcedure(ProcedureModel procedure)
        {
            DBContext DB = setDB();
            Procedure proc = new Procedure();
            proc.ID = procedure.ID;
            proc.Date = procedure.Date;
            proc.DoProcID = procedure.ProcID;
            proc.Visited = procedure.Visited;
            proc.ScheduleID = procedure.ScheduleID;
            DB.Entry(proc).State = System.Data.Entity.EntityState.Modified;
            Save(DB);
        }

        public int addProcedure(ProcedureModel procedure)
        {
            DBContext DB = setDB();
            Procedure proc = new Procedure();
            proc.ID = procedure.ID;
            proc.Date = procedure.Date;
            proc.DoProcID = procedure.ProcID;
            proc.Visited = null;
            proc.ScheduleID = procedure.ScheduleID;
            DB.Procedure.Add(proc);
            Save(DB);
            return proc.ID;
        }

        public List<NurceModel> GetNurces()
        {
            DBContext DB = setDB();
            var buffer = DB.Nurce.ToList();
            return buffer.Select(i => new NurceModel(i)).ToList();
        }

        public NurceModel GetNurce(int ID)
        {
            DBContext DB = setDB();
            return new NurceModel(DB.Nurce.Where(i => i.ID == ID).FirstOrDefault());
        }

        public List<NurceModel> GetNurcesForProcedure(int ID)
        {
            DBContext DB = setDB();
            var buffer = DB.Nurce.ToList().Where(i => DB.MakingProcedure.Where(j => j.NurceID == i.ID && j.ProcedureID == ID).FirstOrDefault() != null).ToList();
            return buffer.Select(i => new NurceModel(i)).ToList();
        }

        public int GetPlaceOfSee(int id)
        {
            DBContext DB = setDB();
            return DB.Streets.Where(i => i.ID == id).FirstOrDefault().PlaceOfSee;
        }
        public List<StreetsModel> GetStreets()
        {
            DBContext DB = setDB();
            var buffer = DB.Streets.ToList();
            return buffer.Select(i => new StreetsModel(i)).ToList();
        }

        public int CreatePatient(PatientModel patient)
        {
            DBContext DB = setDB();
            Patient p = new Patient();
            p.Address = patient.Address;
            p.DateOfBirth = DateTime.Parse(patient.DateOfBirth);
            p.Document = patient.Document;
            p.FullName = patient.FullName;
            p.Male = patient.Male;
            p.PlaceOfWork = patient.PlaceOfWork;
            p.StreetID = patient.StreetID;
            p.Closed = true;
            DB.Patient.Add(p);
            Save(DB);
            return p.ID;
        }

        public void UpdatePatient(PatientModel patient)
        {
            DBContext DB = setDB();
            Patient p = new Patient();
            p.ID = patient.ID;
            p.Address = patient.Address;
            p.DateOfBirth = DateTime.Parse(patient.DateOfBirth);
            p.Document = patient.Document;
            p.Closed = patient.Closed;
            p.FullName = patient.FullName;
            p.Male = patient.Male;
            p.PlaceOfWork = patient.PlaceOfWork;
            p.StreetID = patient.StreetID;
            DB.Entry(p).State = System.Data.Entity.EntityState.Modified;
            Save(DB);
        }

        public List<int> GetMakingProceduresID(int id)
        {
            DBContext DB = setDB();
            var buffer = DB.MakingProcedure.Where(i => i.NurceID == id).ToList();
            return buffer.Select(i => i.ProcedureID).ToList();
        }

        public List<MakingProcedureModel> GetMakingProcedures(int id)
        {
            DBContext DB = setDB();
            var buffer = DB.MakingProcedure.Where(i => i.NurceID == id).ToList();
            return buffer.Select(i => new MakingProcedureModel(i)).ToList();
        }

        public int AddMakingProcedures(MakingProcedureModel proc)
        {
            DBContext DB = setDB();
            MakingProcedure m = new MakingProcedure();
            m.NurceID = proc.NurceID;
            m.ProcedureID = proc.ProcedureID;
            DB.MakingProcedure.Add(m);
            Save(DB);
            return m.ID;
        }
        public void UpdateMakingProcedures(MakingProcedureModel proc)
        {
            DBContext DB = setDB();
            MakingProcedure m = new MakingProcedure();
            m.ID = proc.ID;
            m.NurceID = proc.NurceID;
            m.ProcedureID = proc.ProcedureID;
            DB.Entry(m).State = System.Data.Entity.EntityState.Modified;
            Save(DB);
        }
        public void removeMakingProc(int id)
        {
            DBContext DB = setDB();
            var item = DB.MakingProcedure.Find(id);
            if (item != null)
                DB.MakingProcedure.Remove(item);
        }

        public int AddNurce(NurceModel nurce)
        {
            DBContext DB = setDB();
            Nurce n = new Nurce();
            n.Closed = false;
            n.FullName= nurce.FullName;
            n.WorkEx = nurce.WorkEx;
            DB.Nurce.Add(n);
            Save(DB);
            return n.ID;
        }
        public void UpdateNurce(NurceModel nurce)
        {
            DBContext DB = setDB();
            Nurce n = new Nurce();
            n.Closed = nurce.Closed;
            n.ID = nurce.ID;
            n.FullName = nurce.FullName;
            n.WorkEx = nurce.WorkEx;
            DB.Entry(n).State = System.Data.Entity.EntityState.Modified;
            Save(DB);
        }
        public List<ScheduleProcedureModel> GetScheduleProcedures()
        {
            DBContext DB = setDB();
            var buffer = DB.ScheduleProcedure.Where(i => !i.Closed).ToList();
            return buffer.Select(i => new ScheduleProcedureModel(i)).ToList();
        }

        public List<ScheduleProcedureModel> GetScheduleProceduresByType(int id)
        {
            DBContext DB = setDB();
            var buffer = DB.ScheduleProcedure.Where(i => !i.Closed && i.ProcedureID == id).ToList();
            return buffer.Select(i => new ScheduleProcedureModel(i)).ToList();
        }

        public int AddScheduleProcedure(ScheduleProcedureModel schedule)
        {
            DBContext DB = setDB();
            ScheduleProcedure sc = new ScheduleProcedure();
            sc.Count = schedule.Count;
            sc.DayID = schedule.DayID;
            sc.ProcedureID = schedule.ProcedureID;
            sc.Room = schedule.Room;
            sc.Closed = false;
            DB.ScheduleProcedure.Add(sc);
            Save(DB);
            return sc.ID;
        }
        public void UpdateScheduleProcedure(ScheduleProcedureModel schedule)
        {
            DBContext DB = setDB();
            ScheduleProcedure sc = new ScheduleProcedure();
            sc.ID = schedule.ID;
            sc.Count = schedule.Count;
            sc.DayID = schedule.DayID;
            sc.ProcedureID = schedule.ProcedureID;
            sc.Room = schedule.Room;
            sc.Closed = schedule.Closed;
            DB.Entry(sc).State = System.Data.Entity.EntityState.Modified;
            Save(DB);
        }

        public int GetBusyProceduresCount(int id, DateTime date)
        {
            DBContext DB = setDB();
            return DB.Procedure.Where(i => i.ScheduleID == id && (i.Visited == null || i.Visited == false)).ToList().Where(i => i.Date.Date == date.Date).Count();
        }

        public List<CategoryModel> GetCategories()
        {
            DBContext DB = setDB();
            var buf = DB.Categories.ToList();
            return buf.Select(i => new CategoryModel(i)).ToList();
        }

        public int CreateDoctor(DoctorModel doctor)
        {
            DBContext DB = setDB();
            Doctor d = new Doctor();
            d.CategoryID = doctor.CategoryID;
            d.Certificate = doctor.Certificate;
            d.FullName = doctor.FullName;
            d.PlaceOfSee = doctor.PlaceOfSee;
            d.SpecializationID = doctor.SpecializationID;
            d.Closed = false;
            DB.Doctor.Add(d);
            Save(DB);
            return d.ID;
        }

        public int AddSchedule(ScheduleModel schedule)
        {
            DBContext DB = setDB();
            Schedule s = new Schedule();
            s.dayofWeek = schedule.dayofWeek;
            s.DoctorID = schedule.DoctorID;
            s.Room = schedule.Room;
            s.ShiftID = schedule.ShiftID;
            DB.Schedule.Add(s);
            Save(DB);
            return s.ID;
        }
        public int Save(DBContext DB)
        {
            return DB.SaveChanges();
        }
    }
}
