using System;
using System.IO;
using System.Runtime.InteropServices;

namespace OpenMpt
{
    public class ModuleExt
    {
        
        #region Public methods

        public Module GetModule()
        {
            return m_module;
        }

        public Ext.Interactive GetInteractive()
        {
            return m_interactive;
        }

        #endregion
        
        #region Life-cycle

        public ModuleExt(string path) : this(File.ReadAllBytes(path))
        {
        }

        public ModuleExt(byte[] data)
        {
            IntPtr dataPtr = Marshal.AllocHGlobal(data.Length * sizeof(byte));
            Marshal.Copy(data, 0, dataPtr, data.Length);
            IntPtr internalModuleExt = 
                Native.ModuleExtCreateFromMemory(
                    dataPtr,
                    (UIntPtr)data.LongLength,
                    IntPtr.Zero, 
                    IntPtr.Zero, 
                    IntPtr.Zero, 
                    IntPtr.Zero, 
                    IntPtr.Zero, 
                    IntPtr.Zero, 
                    IntPtr.Zero
                );
            Marshal.FreeHGlobal(dataPtr);
            if (internalModuleExt == IntPtr.Zero)
            {
                throw new ArgumentException(
                    "File could not be loaded."
                );
            }
            m_internalModuleExt = internalModuleExt;
           
            // Register module (composition).
            IntPtr internalModule = Native.ModuleExtGetModule(m_internalModuleExt);
            m_module = new Module(internalModule);
           
            // Register interactive (also composition)
            m_interactive = new Ext.Interactive(m_internalModuleExt);
            
        }

        ~ModuleExt()
        {
            // here there might be a problem
            // because the internal module will already be unloaded by base class.
            // will have to check.
            Native.ModuleExtDestroy(m_internalModuleExt);
        }

        #endregion

        #region Private data

        readonly IntPtr m_internalModuleExt;

        readonly Module m_module;

        readonly Ext.Interactive m_interactive;


        #endregion
    }
}