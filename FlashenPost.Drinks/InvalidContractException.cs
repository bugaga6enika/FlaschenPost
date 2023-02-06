namespace FlaschenPost.Drinks
{
    public class InvalidContractException : Exception
    {
        public InvalidContractException()
        {
        }

        public InvalidContractException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
