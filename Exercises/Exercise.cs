namespace HSA.RehaGame.Exercises
{
    using DB;
    using FulFillables;
    using UI.VisualExercise;
    using UnityEngine;
    using UnityEngine.UI;
    using User;
    using Windows.Kinect;

    public class Exercise : DBObject
    {
        private int id;
        private string unityObjectName;
        private string name;
        private Texture thumbnail;
        private Difficulties difficulty;
        private MovieTexture video;
        private string description;
        private AudioClip auditiveDescription;
        private ExecutionLanguage rel;
        private AudioClip auditiveInformation;
        private string information;

        private bool exerciseRuns;
        private bool exerciseDone;

        private Patient patient;
        private ExerciseExecutionManager executionManager;

        public Exercise(string unityObjectName, Patient patient)
        {
            this.unityObjectName = unityObjectName;
            this.patient = patient;
        }

        public int ID
        {
            get
            {
                return id;
            }
        }

        public string UnityObjectName
        {
            get
            {
                return unityObjectName;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public Texture Thumbnail
        {
            get
            {
                return thumbnail;
            }
        }

        public Difficulties Difficulty
        {
            get
            {
                return difficulty;
            }
        }

        public MovieTexture Video
        {
            get
            {
                return video;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
        }

        public AudioClip AuditiveDescription
        {
            get
            {
                return auditiveDescription;
            }
        }

        public ExecutionLanguage REL
        {
            get
            {
                return rel;
            }
        }

        public AudioClip AuditiveInformation
        {
            get
            {
                return auditiveInformation;
            }
        }

        public string Information
        {
            get
            {
                return information;
            }
        }

        public Patient Patient
        {
            get
            {
                return patient;
            }
        }

        public bool IsActive
        {
            get
            {
                return !exerciseDone && exerciseRuns;
            }
        }

        public ExerciseExecutionManager ExecutionManager
        {
            get
            {
                return executionManager;
            }
        }

        public bool DoExercise(Body body)
        {
            if (exerciseRuns)
            {
                exerciseDone = executionManager.IsFulfilled(body);

                if (exerciseDone)
                    exerciseRuns = false;
                else
                {
                    executionManager.VisualInformation(body);
                    Drawing.ShowInformation(executionManager.Information());
                }
            }

            return exerciseDone || !exerciseRuns;
        }

        public void StartDoingExercise()
        {
            exerciseRuns = true;
        }

        public void StopDoingExercise()
        {
            exerciseRuns = false;
        }

        public override void Delete() {}

        public override object Insert() { return null; }

        public override IDBObject Select()
        {
            var table = DBManager.Query("editor_exercise", string.Format("SELECT * FROM editor_exercise WHERE unityObjectName = '{0}'", this.UnityObjectName));

            this.id = table.GetInt("id");
            this.name = table.GetValueFromLanguage("name");
            this.thumbnail = table.GetResource<Texture>("thumbnail", "jpg", false);
            this.difficulty = (Difficulties)table.GetInt("difficulty");
            this.video = table.GetResource<MovieTexture>("video", "mp4");
            this.description = table.GetValueFromLanguage("description");
            this.auditiveDescription = table.GetResource<AudioClip>("auditiveDescription", "mp3");
            this.rel = new ExecutionLanguage(table.GetValue("rel"));
            this.auditiveInformation = table.GetResource<AudioClip>("auditiveInformation", "mp3");
            this.information = table.GetValueFromLanguage("information");

            var stressedJoints = DBManager.GetStressedJoints(this.id);

            foreach(var joint in stressedJoints.Rows)
            {
                patient.GetJoint(joint.GetValue("name")).Stressed = true;
            }

            this.executionManager = new ExerciseExecutionManager(this.rel.GetSteps(this));

            return this;
        }

        public override bool Update() { return true; }
    }
}
