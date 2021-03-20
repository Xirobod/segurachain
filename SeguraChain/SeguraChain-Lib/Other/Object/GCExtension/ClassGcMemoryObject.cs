using System;
using System.Runtime.InteropServices;

namespace SeguraChain_Lib.Other.Object.GCExtension
{
    public class ClassGcMemoryObject : IDisposable
    {
        /// <summary>
        /// Indicate if the handle has been cleaned.
        /// </summary>
        private bool _free;
        private GCHandle _handle;

        #region Disposing.

        public void Dispose()
        {
            ReleaseUnmanagedResources();
        }

        ~ClassGcMemoryObject()
        {
            ReleaseUnmanagedResources();
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="target"></param>
        public ClassGcMemoryObject(object target)
        {
            _handle = GCHandle.Alloc(target, GCHandleType.WeakTrackResurrection);
        }


        /// <summary>
        /// Ensure to retrieve the data allocated.
        /// </summary>
        public object Target
        {
            get
            {
                if (!_free)
                {
                    if (_handle.IsAllocated)
                    {
                        return _handle.Target;
                    }
                    _free = true;
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    ReleaseUnmanagedResources();
                }
                else
                {
                    _handle.Target = value;
                }
            }
        }

        /// <summary>
        /// Release unmanaged resources.
        /// </summary>
        private void ReleaseUnmanagedResources()
        {
            if (!_free)
            {
                if (_handle.IsAllocated)
                {
                    try
                    {
                        _handle.Target = null;
                        _handle.Free();
                        _free = true;
                       
                    }
                    catch
                    {
                        // Ignored.
                    }
                }
            }
        }

    }
}
