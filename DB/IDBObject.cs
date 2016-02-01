namespace HSA.RehaGame.DB
{
    public interface IDBObject
    {
        object Insert();

        IDBObject Select();

        bool Update();

        void Delete();
    }
}
