namespace HSA.RehaGame.DB
{
    using HSA.RehaGame.DB.Models;
    using System.Collections.Generic;

    public class DatabaseStatistic : Database, IDatabaseStatistic
    {
        private static DatabaseStatistic databaseStatistic;
        private static Patient _patient;

        protected DatabaseStatistic(Patient patient, string tablePrefix) : base(tablePrefix)
        {
            _patient = patient;
        }

        public static DatabaseStatistic Instance(Patient patient, string tablePrefix = "")
        {
            if (databaseStatistic == null)
                databaseStatistic = new DatabaseStatistic(patient, tablePrefix);
            else if (_patient != patient)
                _patient = patient;

            return databaseStatistic;
        }

        public object[] Exercises(bool distinct = true)
        {
            string dist = distinct ? "DISTINCT" : "";
            return ReadQuery(string.Format("SELECT {0} exercise_id FROM statisticdata WHERE patient_id = '{1}' AND data_type = '{2}' ORDER BY id;", dist, _patient.PrimaryKeyValue, (long)StatisticType.exercise));
        }

        public object[] ExercisesByDate(long date, bool distinct = true)
        {
            string dist = distinct ? "DISTINCT" : "";
            string query = string.Format("SELECT {0} exercise_id FROM statisticdata WHERE patient_id = '{1}' AND date = '{2}' AND data_type = '{3}' ORDER BY date;", dist, _patient.PrimaryKeyValue, date, (long)StatisticType.exercise);
            return ReadQuery(query);
        }

        public object[] Dates
        {
            get
            {
                return ReadQuery(string.Format("SELECT DISTINCT date FROM statisticdata WHERE patient_id = '{0}' ORDER BY date;", _patient.PrimaryKeyValue));
            }
        }

        public object[] GetAll()
        {
            string query = string.Format("SELECT id FROM statisticdata WHERE patient_id = '{0}' ORDER BY id;", _patient.PrimaryKeyValue);
            var data = ReadQuery(query);

            return data;
        }

        public object[] GetByExercise(Exercise exercise)
        {
            string query = string.Format("SELECT id FROM statisticdata WHERE patient_id = '{0}' AND exercise_id = '{1}';", _patient.PrimaryKeyValue, exercise.PrimaryKeyValue);
            return ReadQuery(query);
        }

        public object[] GetByDate(long date)
        {
            string query = string.Format("SELECT id FROM statisticdata WHERE patient_id = '{0}' AND date = '{1}' ORDER BY date;", _patient.PrimaryKeyValue, date);
            return ReadQuery(query);
        }
    }
}
