namespace HSA.RehaGame.DB
{
    using Mono.Data.Sqlite;

    public class TransactionResult
    {
        private SQLiteErrorCode errorCode;
        private object primaryKeyValue;

        public TransactionResult(SQLiteErrorCode errorCode, object primaryKeyValue)
        {
            this.errorCode = errorCode;
            this.primaryKeyValue = primaryKeyValue;
        }

        public SQLiteErrorCode ErrorCode
        {
            get
            {
                return this.errorCode;
            }
        }

        public object PrimaryKeyValue
        {
            get
            {
                return this.primaryKeyValue;
            }
        }
    }
}
