using System;
using System.Collections.Generic;

namespace EasyGamePlay
{
    public interface IRequest
    {
        void Init(Dictionary<Type, IRepository> dictionary);
        
    }

    public interface IRequest<T>: IRequest
    {
        T Request();
    }
}
