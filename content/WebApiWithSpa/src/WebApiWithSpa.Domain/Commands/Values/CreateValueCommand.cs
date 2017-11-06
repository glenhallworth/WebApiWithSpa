namespace WebApiWithSpa.Domain.Commands.Values
{
    public class CreateValueCommand : ICommand
    {
        public string Value { get; set; }
    }
}
