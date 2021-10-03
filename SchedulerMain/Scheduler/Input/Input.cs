namespace Domain
{
    public class Input : IInput
    {
        public DateTime InitialDate => DateTime.Now;

        public string DescriptionInput => "Current Date";
    }
}