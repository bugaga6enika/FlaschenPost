using System;
namespace FlaschenPost.Drinks
{
    public interface IMapper<in TIn, out TOut>
    {
        public TOut Map(TIn source);
    }
}

