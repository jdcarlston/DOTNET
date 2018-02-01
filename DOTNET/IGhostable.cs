using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DOTNET
{
    public enum LoadStatus { GHOST, LOADING, LOADED };

    public interface IGhostable
    {
        bool IsGhost { get; }
        bool IsLoaded { get; }

        void MarkGhost();
        void MarkLoading();
        void MarkLoaded();
    }
}
