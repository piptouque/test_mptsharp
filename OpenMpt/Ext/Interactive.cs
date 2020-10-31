using System;
using System.Runtime.InteropServices;

namespace Audio.OpenMpt.Ext
{
    public class Interactive
    {

        #region Public Methods

        public bool SetCurrentSpeed(int speed)
        {
            int success = m_nativeInteractive.SetCurrentSpeed(m_internalModuleExt, speed);
            return success != 0;
        }

        public bool SetCurrentTempo(int tempo)
        {
            int success = m_nativeInteractive.SetCurrentTempo(m_internalModuleExt, tempo);
            return success != 0;
        }

        public bool SetTempoFactor(double factor)
        {
            int success = m_nativeInteractive.SetTempoFactor(m_internalModuleExt, factor);
            return success != 0;
        }

        public bool SetPitchFactor(double factor)
        {
            int success = m_nativeInteractive.SetPitchFactor(m_internalModuleExt, factor);
            return success != 0;
        }

        public bool SetGlobalVolume(double volume)
        {
            int success = m_nativeInteractive.SetGlobalVolume(m_internalModuleExt, volume);
            return success != 0;
        }

        public bool SetChannelVolume(int channel, double volume)
        {
            int success = m_nativeInteractive.SetChannelVolume(m_internalModuleExt, channel, volume);
            return success != 0;
        }

        public bool SetChannelMuteStatus(int channel, bool mute)
        {
            int success = m_nativeInteractive.SetChannelMuteStatus(m_internalModuleExt, channel, mute ? 1 : 0);
            return success != 0;
        }
        
        public bool SetInstrumentMuteStatus(int instrument, bool mute)
        {
            int success = m_nativeInteractive.SetInstrumentMuteStatus(m_internalModuleExt, instrument, mute ? 1 : 0);
            return success != 0;
        }

        public bool PlayNote(int instrument, int note, double volume, double panning)
        {
            int success = m_nativeInteractive.PlayNote(m_internalModuleExt, instrument, note, volume, panning);
            return success != 0;
        }
        
        public bool StopNote(int channel)
        {
            int success = m_nativeInteractive.StopNote(m_internalModuleExt, channel);
            return success != 0;
        }

        public double GetGlobalVolume()
        {
            return m_nativeInteractive.GetGlobalVolume(m_internalModuleExt);
        }

        public double GetChannelVolume(int channel)
        {
            return m_nativeInteractive.GetChannelVolume(m_internalModuleExt, channel);
        }

        public double GetPitchFactor()
        {
            return m_nativeInteractive.GetPitchFactor(m_internalModuleExt);
        }

        public double GetTempoFactor()
        {
            return m_nativeInteractive.GetTempoFactor(m_internalModuleExt);
        }

        public bool GetChannelMuteStatus(int channel)
        {
            return m_nativeInteractive.GetChannelMuteStatus(m_internalModuleExt, channel) != 0;
        }

        public bool GetInstrumentMuteStatus(int instrument)
        {
            return m_nativeInteractive.GetInstrumentMuteStatus(m_internalModuleExt, instrument) != 0;
        }
        #endregion
        
        #region Life-cycle
        /// <summary>
        /// Register an interactive interface from unmanaged pointer.
        /// Should only be called by ModuleExt.
        /// </summary>
        /// <param name="internalInteractive"></param>
        public Interactive(IntPtr internalModuleExt)
        {
            m_internalModuleExt = internalModuleExt;
            
           int interfaceSize = Marshal.SizeOf<Native.ModuleExtInterfaceInteractive>();
           
           m_internalInterface = Marshal.AllocHGlobal(interfaceSize);
           IntPtr keyInteractive = Marshal.StringToHGlobalAnsi(c_keyInteractive);

           Native.ModuleExtGetInterface(
               internalModuleExt, 
               keyInteractive,
               m_internalInterface, 
               (UIntPtr)interfaceSize
               );
           Marshal.FreeHGlobal(keyInteractive);
           
           m_nativeInteractive = new Native.ModuleExtInterfaceInteractive();

           m_nativeInteractive = Marshal.PtrToStructure<Native.ModuleExtInterfaceInteractive>(m_internalInterface);
        }

        ~Interactive()
        {
           Marshal.FreeHGlobal(m_internalInterface);
        }

        #endregion

        #region Private data

        const string c_keyInteractive = "interactive";
        
        readonly IntPtr m_internalModuleExt;
        
        readonly IntPtr m_internalInterface;
        Native.ModuleExtInterfaceInteractive m_nativeInteractive;

        #endregion
    }
}