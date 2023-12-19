using AIR.Fluxity;

namespace Examples.DataCommand
{
    public class ChangeCountCommand : ICommand
    {
        public int Delta { get; set; }
    }
}