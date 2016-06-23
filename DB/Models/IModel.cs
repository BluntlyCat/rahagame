namespace HSA.RehaGame.DB.Models
{
    using Mono.Data.Sqlite;

    public interface IModel
    {
        TransactionResult Save();

        TransactionResult AddManyToManyRelations();

        TransactionResult Delete();

        void SetData();

        void SetRelations();
    }
}
