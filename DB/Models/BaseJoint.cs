namespace HSA.RehaGame.DB.Models
{
    public abstract class BaseJoint : Model
    {
        protected bool xyAxis;
        protected bool yzAxis;
        protected bool zxAxis;

        protected long xyMinValue;
        protected long xyMaxValue;

        protected long yzMinValue;
        protected long yzMaxValue;

        protected long zxMinValue;
        protected long zxMaxValue;

        [TableColumn("xyAxis")]
        public bool XYAxis
        {
            get
            {
                return xyAxis;
            }
            set
            {
                xyAxis = value;
            }
        }

        [TableColumn("yzAxis")]
        public bool YZAxis
        {
            get
            {
                return yzAxis;
            }
            set
            {
                yzAxis = value;
            }
        }

        [TableColumn("zxAxis")]
        public bool ZXAxis
        {
            get
            {
                return zxAxis;
            }
            set
            {
                zxAxis = value;
            }
        }

        [TableColumn]
        public long XYMinValue
        {
            get
            {
                return this.xyMinValue;
            }

            set
            {
                this.xyMinValue = value;
            }
        }

        [TableColumn]
        public long XYMaxValue
        {
            get
            {
                return this.xyMaxValue;
            }

            set
            {
                this.xyMaxValue = value;
            }
        }

        [TableColumn]
        public long YZMinValue
        {
            get
            {
                return this.yzMinValue;
            }

            set
            {
                this.yzMinValue = value;
            }
        }

        [TableColumn]
        public long YZMaxValue
        {
            get
            {
                return this.yzMaxValue;
            }

            set
            {
                this.yzMaxValue = value;
            }
        }

        [TableColumn]
        public long ZXMinValue
        {
            get
            {
                return this.zxMinValue;
            }

            set
            {
                this.zxMinValue = value;
            }
        }

        [TableColumn]
        public long ZXMaxValue
        {
            get
            {
                return this.zxMaxValue;
            }

            set
            {
                this.zxMaxValue = value;
            }
        }
    }
}
