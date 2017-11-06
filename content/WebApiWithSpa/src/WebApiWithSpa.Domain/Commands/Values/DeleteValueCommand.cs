namespace WebApiWithSpa.Domain.Commands.Values
{
    public class DeleteValueCommand : ICommand
    {
        public int Id { get; set; }
    }
}
