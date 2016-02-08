namespace HSA.RehaGame.DB.Models
{
    using DB;

    public class PatientJoint : Model
    {
        private int id;

        private bool active = true;

        private long x_axis_min_value;
        private long x_axis_max_value;

        private long y_axis_min_value;
        private long y_axis_max_value;

        private long z_axis_min_value;
        private long z_axis_max_value;

        private string kinectJoint_id;

        public PatientJoint(int id, IDatabase database) : base(database)
        {
            this.id = id;
        }

        public PatientJoint(IDatabase database) : base(database)
        {
        }

        [TableColumn]
        [PrimaryKey]
        public int ID
        {
            get
            {
                return id;
            }

            private set
            {
                id = value;
            }
        }

        [TableColumn]
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        [TableColumn]
        public long X_Axis_Min_Value
        {
            get
            {
                return this.x_axis_min_value;
            }

            set
            {
                this.x_axis_min_value = value;
            }
        }

        [TableColumn]
        public long X_Axis_Max_Value
        {
            get
            {
                return this.x_axis_max_value;
            }

            set
            {
                this.x_axis_max_value = value;
            }
        }

        [TableColumn]
        public long Y_Axis_Min_Value
        {
            get
            {
                return this.y_axis_min_value;
            }

            set
            {
                this.y_axis_min_value = value;
            }
        }

        [TableColumn]
        public long Y_Axis_Max_Value
        {
            get
            {
                return this.y_axis_max_value;
            }

            set
            {
                this.y_axis_max_value = value;
            }
        }

        [TableColumn]
        public long Z_Axis_Min_Value
        {
            get
            {
                return this.z_axis_min_value;
            }

            set
            {
                this.z_axis_min_value = value;
            }
        }

        [TableColumn]
        public long Z_Axis_Max_Value
        {
            get
            {
                return this.z_axis_max_value;
            }

            set
            {
                this.z_axis_max_value = value;
            }
        }
    }
}
