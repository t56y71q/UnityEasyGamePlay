using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGamePlay
{
    public interface ISystemData:ISystem
    {
        void LoadData(string data);
        string CreateData();
    }
}
