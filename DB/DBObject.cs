namespace HSA.RehaGame.DB
{
    public abstract class DBObject : IDBObject
    {
        public abstract object Insert();

        public abstract IDBObject Select();

        public abstract bool Update();

        public abstract void Delete();
    }
}
