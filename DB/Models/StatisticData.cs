namespace HSA.RehaGame.DB.Models
{
    using System;
    using System.Collections.Generic;
    public class StatisticData : Model
    {
        private long id;

        private string name;

        private long dataType;

        private long date;
        private long startTime;
        private long endTime;

        private string message;

        private long state;

        private PatientJoint affectedJoint;
        private Exercise exercise;
        private Patient patient;

        public StatisticData(long id)
        {
            this.id = id;
        }

        public StatisticData(long date, Patient patient, long id)
        {
            var now = DateTime.Now;

            this.id = id;
            this.date = date;
            this.startTime = now.TimeOfDay.Ticks;
            this.endTime = now.TimeOfDay.Ticks;
        }

        public StatisticData(long date, string name, StatisticType dataType, StatisticStates state, string message, PatientJoint affectedJoint, Exercise exercise, Patient patient)
        {
            var now = DateTime.Now;

            this.name = name;
            this.dataType = (long)dataType;
            this.state = (long)state;

            this.date = date;
            this.startTime = now.TimeOfDay.Ticks;
            this.endTime = now.TimeOfDay.Ticks;

            this.message = message;

            this.affectedJoint = affectedJoint;
            this.exercise = exercise;
            this.patient = patient;
        }

        [PrimaryKey]
        public long Id
        {
            get
            {
                return id;
            }

            set
            {
                this.id = value;
            }
        }

        [TableColumn]
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        [TableColumn("data_type")]
        private long LongDataType
        {
            get
            {
                return this.dataType;
            }

            set
            {
                this.dataType = value;
            }
        }

        public StatisticType DataType
        {
            get
            {
                return (StatisticType)this.dataType;
            }

            set
            {
                this.dataType = (long)value;
            }
        }

        [TableColumn("state")]
        private long LongState
        {
            get
            {
                return this.state;
            }

            set
            {
                this.state = value;
            }
        }

        public StatisticStates State
        {
            get
            {
                return (StatisticStates)this.state;
            }

            set
            {
                this.state = (long)value;
            }
        }

        [TableColumn("date")]
        private long LongDate
        {
            get
            {
                return date;
            }

            set
            {
                this.date = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return new DateTime(this.LongDate);
            }

            set
            {
                this.date = value.Ticks;
            }
        }

        [TableColumn("start_time")]
        private long LongStartTime
        {
            get
            {
                return startTime;
            }

            set
            {
                this.startTime = value;
            }
        }

        public DateTime StartTime
        {
            get
            {
                return new DateTime(this.LongStartTime);
            }

            set
            {
                this.startTime = value.Ticks;
            }
        }

        [TableColumn("end_time")]
        private long LongEndTime
        {
            get
            {
                return endTime;
            }

            set
            {
                this.endTime = value;
            }
        }

        public DateTime EndTime
        {
            get
            {
                return new DateTime(this.LongEndTime);
            }

            set
            {
                this.endTime = value.Ticks;
            }
        }

        [TableColumn]
        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                this.message = value;
            }
        }
        
        [ForeignKey("patientjoint", "affected_joint_id", false)]
        public PatientJoint AffectedJoint
        {
            get
            {
                return affectedJoint;
            }

            set
            {
                this.affectedJoint = value;
            }
        }

        [ForeignKey("exercise", "exercise_id")]
        public Exercise Exercise
        {
            get
            {
                return exercise;
            }

            set
            {
                this.exercise = value;
            }
        }

        [ForeignKey("patient", "patient_id")]
        public Patient Patient
        {
            get
            {
                return patient;
            }

            set
            {
                this.patient = value;
            }
        }

        public TimeSpan TimeDiff
        {
            get
            {
                return EndTime - StartTime;
            }
        }

        public static StatisticData GetModel(Patient patient, object primaryKey)
        {
            var data = GetModel<StatisticData>(primaryKey);
            data.database = DatabaseStatistic.Instance(patient);

            return data;
        }

        public static IList<StatisticData> GetAll(Patient patient)
        {
            IList<StatisticData> data = new List<StatisticData>();
            var primaryKeys = DatabaseStatistic.Instance(patient).GetAll();

            foreach (var primaryKey in primaryKeys)
                data.Add(GetModel(patient, primaryKey));

            return data;
        }

        public static IList<object> Exercises(Patient patient, bool distinct = true)
        {
            return DatabaseStatistic.Instance(patient).Exercises(distinct);
        }

        public static IList<object> ExercisesByDate(Patient patient, long date, bool distinct = true)
        {
            return DatabaseStatistic.Instance(patient).ExercisesByDate(date, distinct);
        }

        public static IList<long> Dates(Patient patient)
        {
            var dates = new List<long>();
            var ticks = DatabaseStatistic.Instance(patient).Dates;

            foreach(long tick in ticks)
            {
                if (dates.Contains(tick) == false)
                    dates.Add(tick);
            }

            return dates;
        }

        public static IList<StatisticData> GetByExercise(Patient patient, Exercise exercise)
        {
            IList<StatisticData> data = new List<StatisticData>();
            var primaryKeys = DatabaseStatistic.Instance(patient).GetByExercise(exercise);

            foreach (var primaryKey in primaryKeys)
                data.Add(GetModel(patient, primaryKey));

            return data;
        }

        public static IList<StatisticData> GetByDate(Patient patient, long date)
        {
            IList<StatisticData> data = new List<StatisticData>();
            var primaryKeys = DatabaseStatistic.Instance(patient).GetByDate(date);

            foreach (var primaryKey in primaryKeys)
                data.Add(GetModel(patient, primaryKey));

            return data;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} at {2}s ({3})", dataType, state, endTime - startTime);
        }
    }
}
