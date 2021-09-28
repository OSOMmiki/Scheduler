namespace SchedulerMain
{
    public class InputScheduler : IInputScheduler
    {
        public DateTime InitialDate => DateTime.Now;

        public string DescriptionInput => "Current Date";
    }
}