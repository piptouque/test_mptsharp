using System;
using System.IO;
using System.Runtime.InteropServices;

namespace OpenMpt
{
    public class Module
    {
        #region Render Parameters
        public enum RenderParam
        {
            eRenderMasterGainMillibel = 1,
            eRenderStereoSeperationPercent = 2,
            eRenderInterpolationFilterLength = 3,
            eRenderVolumeRampingStrength = 4
        }

        public int GetRenderParam(RenderParam param)
        {
            return Native.ModuleGetRenderParam(m_internalModule, (int) param);
        }

        public bool SetRenderParam(RenderParam param, int value)
        {
            int success = Native.ModuleSetRenderParam(m_internalModule, (int) param, value);
            return success != 0;
        }
        #endregion
        
        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="row"></param>
        /// <returns>New position in seconds.</returns>
        public double SetPositionOrderRow(int order, int row)
        {
            return Native.ModuleSetPositionOrderRow(m_internalModule, order, row);
        }

        public int GetCurrentOrder()
        {
            return Native.ModuleGetCurrentOrder(m_internalModule);
        }

        public int GetCurrentRow()
        {
            return Native.ModuleGetCurrentRow(m_internalModule);
        }

        public int GetCurrentPattern()
        {
            return Native.ModuleGetCurrentPattern(m_internalModule);
        }

        public int GetRepeatCount()
        {
            return Native.ModuleGetRepeatCount(m_internalModule);
        }

        /// <summary>
        /// Sets repeat count for the music.
        /// From libopenmptdoc: https://lib.openmpt.org/doc/group__libopenmpt__c.html
        /// Repeat Count
        ///   -1: repeat forever
        ///   0: play once, repeat zero times (the default)
        ///   n>0: play once and repeat n times after that
        /// </summary>
        /// <param name="count"></param>
        /// <returns>true on success, false on failure.</returns>
        public bool SetRepeatCount(int count)
        {
            int success = Native.ModuleSetRepeatCount(m_internalModule, count);
            return success != 0;
        }

        public int GetCurrentNumberPlayingChannels()
        {
            return Native.ModuleGetCurrentPlayingChannels(m_internalModule);
        }

        public double GetCurrentEstimatedBpm()
        {
            return Native.ModuleGetCurrentEstimatedBpm(m_internalModule);
        }

        public int GetCurrentSpeed()
        {
            return Native.ModuleGetCurrentSpeed(m_internalModule);
        }

        public int GetCurrentTempo()
        {
            return Native.ModuleGetCurrentTempo(m_internalModule);
        }

        public string GetMetadata(string key)
        {
            IntPtr ptr = Marshal.StringToHGlobalAnsi(key);
            IntPtr dataPtr = Native.ModuleGetMetadata(m_internalModule, ptr);
            string data = Marshal.PtrToStringAnsi(dataPtr);
            Marshal.FreeHGlobal(ptr);
            return data;
        }

        public int GetSelectedSubsong()
        {
            return Native.ModuleGetSelectedSubsong(m_internalModule);
        }

        public bool SelectSubsong(int subsong)
        {
            int success = Native.ModuleSelectSubsong(m_internalModule, subsong);
            return success != 0;
        }

        public int GetNumChannels()
        {
            return Native.ModuleGetNumChannels(m_internalModule);
        }

        public int GetNumInstruments()
        {
            return Native.ModuleGetNumInstruments(m_internalModule);
        }

        public int GetNumPatterns()
        {
            return Native.ModuleGetNumPatterns(m_internalModule);
        }

        public string GetInstrumentName(int index)
        {
            IntPtr dataPtr = Native.ModuleGetInstrumentName(m_internalModule, index);
            string data = Marshal.PtrToStringAnsi(dataPtr);
            return data;
        }

        /// <summary>
        /// Reads numberFrames samples from module into data.
        /// </summary>
        /// <param name="sampleRate"></param>
        /// <param name="count">number of samples?</param>
        /// <param name="mono">output data</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public long Read(int sampleRate, long count, float[] mono)
        {
            // User should make sure data.Length size is greater than count.
            if (count > mono.LongLength)
            {
                throw new ArgumentException(
                       $"data of size {mono.LongLength} is not large enough for output of size {count}"
                    );
            }
            if (count > m_internalOutputBufferSize)
            {
                ResizeInternalBuffer((int)count);
            }
            long actualNumberFrames = 
                Native.ModuleReadFloatMono(
                m_internalModule, 
                sampleRate, 
                (UIntPtr)count,
                m_internalOutputBuffer
                );
            Marshal.Copy(m_internalOutputBuffer, mono, 0, (int)actualNumberFrames);

            return actualNumberFrames;
        }

        public long ReadInterleavedQuad(int sampleRate, long count, float[] interleavedQuad)
        {
            // todo: shameful copy-paste
            if (count > interleavedQuad.LongLength)
            {
                throw new ArgumentException(
                       $"data of size {interleavedQuad.LongLength} is not large enough for output of size {count}"
                    );
            }
            if (count > m_internalOutputBufferSize)
            {
                ResizeInternalBuffer((int)count);
            }
            long actualNumberFrames = 
                Native.ModuleReadInterleavedFloatQuad(
                m_internalModule, 
                sampleRate, 
                (UIntPtr)count,
                m_internalOutputBuffer
                );
            Marshal.Copy(m_internalOutputBuffer, interleavedQuad, 0, (int)actualNumberFrames);

            return actualNumberFrames;
        }

        public long ReadInterleavedStereo(int sampleRate, long count, float[] interleavedStereo)
        {
            // todo: shameful copy-paste
            if (count > interleavedStereo.LongLength)
            {
                throw new ArgumentException(
                       $"data of size {interleavedStereo.LongLength} is not large enough for output of size {count}"
                    );
            }
            if (count > m_internalOutputBufferSize)
            {
                ResizeInternalBuffer((int)count);
            }
            long actualNumberFrames = 
                Native.ModuleReadInterleavedFloatStereo(
                m_internalModule, 
                sampleRate, 
                (UIntPtr)count,
                m_internalOutputBuffer
                );
            Marshal.Copy(m_internalOutputBuffer, interleavedStereo, 0, (int)actualNumberFrames);

            return actualNumberFrames;
        }
        
        #endregion
        
        #region C-tor, D-tor
        /// <summary>
        /// Loads module at path.
        /// </summary>
        /// <param name="path">absolute path to module file.</param>
        /// <exception cref="ArgumentException">On Failure to load file at path.</exception>
        public Module(string path) : this(File.ReadAllBytes(path))
        {
        }

        public Module(byte[] data)
        {
            IntPtr dataPtr = Marshal.AllocHGlobal(data.Length * sizeof(byte));
            Marshal.Copy(data, 0, dataPtr, data.Length);
            IntPtr internalModule = 
                Native.ModuleCreateFromMemory(
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
            if (internalModule == IntPtr.Zero)
            {
                throw new ArgumentException(
                    "File could not be loaded."
                );
            }

            m_internalModule = internalModule;
            ResizeInternalBuffer(0);
            
            // Independent from Module Extension
            m_isFromExt = false;
        }

        /// <summary>
        /// Registre already-loaded module. Should only be used by ModuleExt.
        /// </summary>
        internal Module(IntPtr internalModule)
        {
            m_internalModule = internalModule;
            // data already loaded in OpenMpt,
            // Just have to point to it.
            ResizeInternalBuffer(0);
            m_isFromExt = true;
        }


        ~Module()
        {
            FreeInternalBuffer();
            // If it's not dependant on a ModuleExt, we should free the memory.
            if (!m_isFromExt)
            {
                Native.ModuleDestroy(m_internalModule);
            }
        }

        void ResizeInternalBuffer(int size)
        {
            
            FreeInternalBuffer();
            // Initialise buffer
            // Will realloc when necessary.
            m_internalOutputBuffer = Marshal.AllocHGlobal(size * sizeof(double));
            m_internalOutputBufferSize = size;
        }

        void FreeInternalBuffer()
        {
            
            if (m_internalOutputBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(m_internalOutputBuffer);
            }
        }
        
        #endregion
        
        #region Error Handling

        public void ErrorClear()
        {
            Native.ErrorClear(m_internalModule);
        }
        public int ErrorGetLast()
        {
            return Native.ErrorGetLast(m_internalModule);
        }
        public String ErrorGetLastMessage()
        {
            IntPtr ptr = Native.ErrorGetLastMessage(m_internalModule);
            return Marshal.PtrToStringAnsi(ptr);
        }

        #endregion

        #region Internal Data 

        readonly IntPtr m_internalModule;
        /// <summary>
        /// Whether the Module handle was obtained from a ModuleExt.
        /// If was, the ModuleExt will free the internal module for us.
        /// </summary>
        readonly bool m_isFromExt;
        
        IntPtr m_internalOutputBuffer;
        long m_internalOutputBufferSize;


        #endregion

        #region Public Metadata keys

        /// <summary>
        /// see: https://lib.openmpt.org/doc/group__libopenmpt__c.html#gac171f8fb2c7a0b998855956069159068
        /// </summary>
        public const string c_keyAuthor  = "artist";
        public const string c_keyTitle   = "title";
        public const string c_keyMessage = "message_raw";

        #endregion

    }
}