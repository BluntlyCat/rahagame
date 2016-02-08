namespace HSA.RehaGame.DB.Models
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class TableColumn : Attribute
    {
        private string nameInTable;
        public readonly bool NotNull;

        public TableColumn(string nameInTable = null, bool notNull = true)
        {
            this.nameInTable = nameInTable;
            this.NotNull = notNull;
        }

        public string NameInTable
        {
            get
            {
                return nameInTable;
            }

            protected set
            {
                this.nameInTable = value;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey : Attribute
    {
        public readonly string NameInTable;

        public PrimaryKey(string nameInTable = null)
        {
            this.NameInTable = nameInTable;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKey : TableColumn
    {
        public readonly string RelationTable;

        public ForeignKey(string relationTable, string nameInTable = null, bool notNull = true) : base(nameInTable, notNull)
        {
            this.RelationTable = relationTable;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ManyToManyRelation : ForeignKey
    {
        public readonly string FromID;
        public readonly string ToID;

        public ManyToManyRelation(string fromId, string relationTable, string toId, bool notNull = true) : base(relationTable, null, notNull)
        {
            this.FromID = fromId;
            this.ToID = toId;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class TranslationColumn : TableColumn
    {
        public TranslationColumn(string nameInTable = null, bool notNull = true) : base (nameInTable, notNull) {}

        public void SetColumn(string columnName, string language)
        {
            if(this.NameInTable == null)
                this.NameInTable = string.Format("{0}_{1}", columnName.ToLower(), language);
            else
                this.NameInTable = string.Format("{0}_{1}", NameInTable, language);
        }
    }
}