namespace ApiDeFilasDeAtendimento.Exceptions
{
    public class BadRequestException : BusinessException
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
