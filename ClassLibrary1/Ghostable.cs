using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LIB
{
    /// <summary>
    /// Base class for all "ghostable objects". Provides state management for lazy loading.
    /// </summary>
    [Serializable]
    public class Ghostable : IGhostable, IDisposable
    {
        private enum LoadStatus { GHOST, LOADING, LOADED };

        private LoadStatus loadstatus = LoadStatus.GHOST;

        public bool IsGhost
        {
            get { return loadstatus.Equals(LoadStatus.GHOST); }
        }

        public bool IsLoaded
        {
            get { return loadstatus.Equals(LoadStatus.LOADED); }
        }

        public void MarkGhost()
        {
            loadstatus = LoadStatus.GHOST;
        }

        public void MarkLoading()
        {
            loadstatus = LoadStatus.LOADING;
        }

        public void MarkLoaded()
        {
            loadstatus = LoadStatus.LOADED;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                loadstatus = LoadStatus.GHOST;
            }
        }
    }
}
