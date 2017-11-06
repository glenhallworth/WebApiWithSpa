namespace WebApiWithSpa.Domain.Commands.Values
{
    public class UpdateValueCommand : ICommand
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
