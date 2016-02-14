namespace HSA.RehaGame.DB.Models
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class TableColumn : Attribute
    {
        private string nameInTable;
        public readonly bool NotNull;
        public readonly bool unique;

        public TableColumn(string nameInTable = null, bool notNull = true, bool unique = false)
        {
            this.nameInTable = nameInTable;
            this.NotNull = notNull;
            this.unique = unique;
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
    public class PrimaryKey : TableColumn
    {
        

        public PrimaryKey(string nameInTable = null) : base(nameInTable, true)
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
    public class ManyToManyRelation : TableColumn
    {
        public readonly string SourceID;
        public readonly string SourceTable;
        public readonly string JoinSourceId;
        public readonly string JoinTable;
        public readonly string JoinTargetId;
        public readonly string TargetTable;
        public readonly string TargetID;

        public ManyToManyRelation(string sourceId, string sourceTable, string joinSourceId, string joinTable, string joinTargetId, string targetTable, string targetId) : base(notNull: false)
        {
            this.SourceID = sourceId;
            this.SourceTable = sourceTable;
            this.JoinSourceId = joinSourceId;
            this.JoinTable = joinTable;
            this.JoinTargetId = joinTargetId;
            this.TargetTable = targetTable;
            this.TargetID = targetId;
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

    [AttributeUsage(AttributeTargets.Property)]
    public class ResourceColumn : TableColumn
    {
        public readonly Type Type;

        public ResourceColumn(Type type = null, string nameInTable = null, bool notNull = true) : base(nameInTable, notNull)
        {
            this.Type = type;
        }
    }
}