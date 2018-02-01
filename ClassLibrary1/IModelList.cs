using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LIB
{
    public interface IModelList : IGhostable
    {
        void Load();
        List<int> GetIds();
    }
}
