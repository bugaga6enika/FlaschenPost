namespace FlaschenPost.Drinks
{
    public interface ITypeConverter<out T>
    {
        public T Convert(ReadOnlySpan<char> stringRepresentation);
    }
}

