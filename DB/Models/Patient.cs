namespace HSA.RehaGame.DB.Models
{
    using DB;

    public class Patient : Model
    {
        private string name;

        private long age;
        private long sex;

        public Patient(string name, IDatabase database) : base(database)
        {
            this.name = name;
        }

        public Patient(string name, long age, Sex sex, IDatabase database) : base(database)
        {
            this.name = name;
            this.age = age;
            this.sex = (long)sex;
        }

        [TableColumn]
        [PrimaryKey]
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

        [TableColumn("sex")]
        private long LongSex
        {
            get
            {
                return this.sex;
            }

            set
            {
                this.sex = value;
            }
        }

        public Sex Sex
        {
            get
            {
                return (Sex)this.sex;
            }

            set
            {
                this.sex = (long)value;
            }
        }

        [TableColumn]
        public long Age
        {
            get
            {
                return this.age;
            }

            set
            {
                this.age = value;
            }
        }
    }
}
