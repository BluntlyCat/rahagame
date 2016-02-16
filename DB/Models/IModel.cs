namespace HSA.RehaGame.DB.Models
{
    using Mono.Data.Sqlite;

    public interface IModel
    {
        SQLiteErrorCode Save();

        SQLiteErrorCode AddManyToManyRelations();

        SQLiteErrorCode Delete();

        void SetData();
    }
}
