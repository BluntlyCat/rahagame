namespace HSA.RehaGame.Exercises.FulFillables
{
    using DB.Models;
    using Manager;
    public class Step : BaseStep
    {
        public Step(string name, StatisticType statisticType, PatientJoint affectedJoint, BaseStep prevStep, int repetitions, WriteStatisticManager statisticManager) : base(name, statisticType, affectedJoint, prevStep, repetitions, statisticManager)
        {
            this.type = Types.step;
        }

        public void SetFirstInformable(Informable firstToDoAble)
        {
            this.firstInformable = this.currentInformable = firstToDoAble;
        }
    }
}
