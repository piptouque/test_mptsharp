using System;

using System.Runtime.InteropServices;

namespace OpenMpt
{
    class Native
    {
        
        #region Core OpenMPT
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_get_core_version")]
        public static extern UInt32 GetCoreVersion();

        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_get_library_version")]
        public static extern UInt32 GetLibraryVersion();

        #endregion


        #region Module class 
        
        // Apparently, the closest equivalent to size_t in c# is System.UIntPtr..
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_create_from_memory2")]
        public static extern IntPtr ModuleCreateFromMemory(
            IntPtr fileData, UIntPtr filesize, IntPtr logFunc, IntPtr loguser, 
            IntPtr errFunc, IntPtr errUser, IntPtr error, IntPtr errorMessage, IntPtr ctls);

        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_destroy")]
        public static extern void ModuleDestroy(IntPtr mod);
        
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_error_get_last_message")]
        public static extern IntPtr ErrorGetLastMessage(IntPtr mod);
        
        
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_set_position_order_row")]
        public static extern Double ModuleSetPositionOrderRow(IntPtr mod, Int32 order, Int32 row);
        
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_get_current_order")]
        public static extern Int32 ModuleGetCurrentOrder(IntPtr mod);
        
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_get_current_row")]
        public static extern Int32 ModuleGetCurrentRow(IntPtr mod);
        
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_get_current_pattern")]
        public static extern Int32 ModuleGetCurrentPattern(IntPtr mod);
        
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_set_repeat_count")]
        public static extern Int32 ModuleSetRepeatCount(IntPtr mod, Int32 repeatCount);
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_get_repeat_count")]
        public static extern Int32 ModuleGetRepeatCount(IntPtr mod);
        
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_get_current_playing_channels")]
        public static extern Int32 ModuleGetCurrentPlayingChannels(IntPtr mod);
        
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_get_current_estimated_bpm")]
        public static extern Double ModuleGetCurrentEstimatedBpm(IntPtr mod);
        
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_get_current_speed")]
        public static extern Int32 ModuleGetCurrentSpeed(IntPtr mod);
        
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_get_current_tempo")]
        public static extern Int32 ModuleGetCurrentTempo(IntPtr mod);

        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_get_metadata")]
        public static extern IntPtr ModuleGetMetadata(IntPtr mod, [MarshalAs(UnmanagedType.AnsiBStr)] IntPtr key);

        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_get_selected_subsong")]
        public static extern Int32 ModuleGetSelectedSubsong(IntPtr mod);
        
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_select_subsong")]
        public static extern Int32 ModuleSelectSubsong(IntPtr mod, Int32 subsong);
        
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_read_float_mono")]
        public static extern Int32 ModuleReadFloatMono(IntPtr mod, Int32 sampleRate, UIntPtr count, IntPtr mono);
        
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_get_num_channels")]
        public static extern Int32 ModuleGetNumChannels(IntPtr mod);
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_get_num_instruments")]
        public static extern Int32 ModuleGetNumInstruments(IntPtr mod);

        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_get_num_patterns")]
        public static extern Int32 ModuleGetNumPatterns(IntPtr mod);

        #endregion
        
        #region ModuleExt class
        // Apparently, the closest equivalent to size_t in c# is System.UIntPtr..
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_ext_create_from_memory")]
        public static extern IntPtr ModuleExtCreateFromMemory(
            IntPtr fileData, UIntPtr filesize, IntPtr logFunc, IntPtr loguser, 
            IntPtr errFunc, IntPtr errUser, IntPtr error, IntPtr errorMessage, IntPtr ctls);

        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_ext_destroy")]
        public static extern void ModuleExtDestroy(IntPtr modExt);
        
        
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_ext_get_module")]
        public static extern IntPtr ModuleExtGetModule(IntPtr modExt);
        
        [DllImport(Import.c_openMptLib, EntryPoint = "openmpt_module_ext_get_interface")]
        public static extern IntPtr ModuleExtGetInterface(
            IntPtr modExt, 
            [MarshalAs(UnmanagedType.AnsiBStr)] IntPtr interfaceId,
            IntPtr interfacePtr,
            UIntPtr interfaceSize
            );
        
        #endregion
        
        #region Delegates
        
        [UnmanagedFunctionPointer(System.Runtime.InteropServices
            .CallingConvention.Cdecl)]
        public delegate Int32 StreamCallback([MarshalAs(UnmanagedType.AnsiBStr)] IntPtr message, IntPtr user);
        
        [UnmanagedFunctionPointer(System.Runtime.InteropServices
            .CallingConvention.Cdecl)]
        public delegate Int32 LogFunc([MarshalAs(UnmanagedType.AnsiBStr)] IntPtr message, IntPtr user);
        
        [UnmanagedFunctionPointer(System.Runtime.InteropServices
            .CallingConvention.Cdecl)]
        public delegate Int32 ErrorFunc(Int32 error, IntPtr user);
        
        #endregion
        
        #region Native Interface delegates
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 ModuleExtInterfaceInteractiveSetCurrentSpeed(IntPtr moduleExt, Int32 speed);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 ModuleExtInterfaceInteractiveSetCurrentTempo(IntPtr moduleExt, Int32 tempo);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 ModuleExtInterfaceInteractiveSetTempoFactor(IntPtr moduleExt, Double factor);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Double ModuleExtInterfaceInteractiveGetTempoFactor(IntPtr moduleExt);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 ModuleExtInterfaceInteractiveSetPitchFactor(IntPtr moduleExt, Double factor);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Double ModuleExtInterfaceInteractiveGetPitchFactor(IntPtr moduleExt);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 ModuleExtInterfaceInteractiveSetGlobalVolume(IntPtr moduleExt, Double volume);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Double ModuleExtInterfaceInteractiveGetGlobalVolume(IntPtr moduleExt);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 ModuleExtInterfaceInteractiveSetChannelVolume(
            IntPtr moduleExt, Int32 channel, Double volume);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Double ModuleExtInterfaceInteractiveGetChannelVolume(IntPtr moduleExt, Int32 channel);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 ModuleExtInterfaceInteractiveSetChannelMuteStatus(
            IntPtr moduleExt, Int32 channel, Int32 mute);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 ModuleExtInterfaceInteractiveGetChannelMuteStatus(IntPtr moduleExt, Int32 channel);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 ModuleExtInterfaceInteractiveSetInstrumentMuteStatus(
            IntPtr moduleExt, Int32 instrument, Int32 mute);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 ModuleExtInterfaceInteractiveGetInstrumentMuteStatus(IntPtr moduleExt, Int32 instrument);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 ModuleExtInterfaceInteractivePlayNote(
            IntPtr moduleExt, Int32 instrument, Int32 note, Double volume, Double panning);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 ModuleExtInterfaceInteractiveStopNote(IntPtr moduleExt, Int32 channel);
        
        #endregion
        
        #region Native Interface structure

        [StructLayout(LayoutKind.Sequential)]
        public struct ModuleExtInterfaceInteractive
        {
            public readonly ModuleExtInterfaceInteractiveSetCurrentSpeed SetCurrentSpeed;
            public readonly ModuleExtInterfaceInteractiveSetCurrentTempo SetCurrentTempo;
            public readonly ModuleExtInterfaceInteractiveSetTempoFactor  SetTempoFactor;
            public readonly ModuleExtInterfaceInteractiveSetPitchFactor  SetPitchFactor;
            public readonly ModuleExtInterfaceInteractiveSetGlobalVolume SetGlobalVolume;
            public readonly ModuleExtInterfaceInteractiveSetChannelVolume SetChannelVolume;
            public readonly ModuleExtInterfaceInteractiveSetChannelMuteStatus SetChannelMuteStatus;
            public readonly ModuleExtInterfaceInteractiveSetInstrumentMuteStatus SetInstrumentMuteStatus;
            public readonly ModuleExtInterfaceInteractivePlayNote        PlayNote;
            public readonly ModuleExtInterfaceInteractiveStopNote        StopNote;
            

            public readonly ModuleExtInterfaceInteractiveGetGlobalVolume  GetGlobalVolume;
            public readonly ModuleExtInterfaceInteractiveGetChannelVolume GetChannelVolume;
            public readonly ModuleExtInterfaceInteractiveGetPitchFactor   GetPitchFactor;
            public readonly ModuleExtInterfaceInteractiveGetTempoFactor   GetTempoFactor;
            public readonly ModuleExtInterfaceInteractiveGetChannelMuteStatus GetChannelMuteStatus;
            public readonly ModuleExtInterfaceInteractiveGetInstrumentMuteStatus GetInstrumentMuteStatus;

        }

        #endregion
    }

    static class Import
    {
        public const string c_openMptLib = "libopenmpt";

        public const string c_win32Dir = "Win32";
        public const string c_linuxDir = "Linux";
        public const string c_OsxDir   = "OSX";

        public const string c_32BitDir = "x86";
        public const string c_64BitDir = "x86-64";
    }

}