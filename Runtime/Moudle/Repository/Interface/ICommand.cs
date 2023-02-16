using System;
using System.Collections.Generic;

namespace EasyGamePlay
{
    public interface ICommand
    {
        void Init(Dictionary<Type, IRepository> dictionary);
        void Command();
    }
}
