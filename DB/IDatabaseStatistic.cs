namespace HSA.RehaGame.DB
{
    using Models;
    using System;
    public interface IDatabaseStatistic
    {
        object[] GetAll();

        object[] GetByExercise(Exercise exercise);

        object[] GetByDate(long date);
    }
}
