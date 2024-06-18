using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace com.zibra.liquid.Solver
{
    public static class ZibraLiquidBridge
    {

#if UNITY_EDITOR

// Editor library selection
#if UNITY_EDITOR_WIN
        public const String PluginLibraryName = "ZibraFluidNative_Win";
#elif UNITY_EDITOR_OSX
        public const String PluginLibraryName = "ZibraFluidNative_Mac";
#else
#error Unsupported platform
#endif

#else

// Player library selection
#if UNITY_IOS || UNITY_TVOS
        public const String PluginLibraryName = "__Internal";
#elif UNITY_WSA
        public const String PluginLibraryName = "ZibraFluidNative_WSA";
#elif UNITY_STANDALONE_OSX
        public const String PluginLibraryName = "ZibraFluidNative_Mac";
#elif UNITY_STANDALONE_WIN
        public const String PluginLibraryName = "ZibraFluidNative_Win";
#elif UNITY_ANDROID
        public const String PluginLibraryName = "ZibraFluidNative_Android";
#else
#error Unsupported platform
#endif

#endif

        [DllImport(PluginLibraryName)]
        public static extern IntPtr GetRenderEventWithDataFunc();

        [DllImport(PluginLibraryName)]
        public static extern IntPtr GPUReadbackGetData(Int32 InstanceID, UInt32 size);

        [DllImport(PluginLibraryName)]
        public static extern Int32 GetCurrentAffineBufferIndex(Int32 InstanceID);

        [DllImport(PluginLibraryName)]
        public static extern Int32 GarbageCollect();

        [DllImport(PluginLibraryName)]
        public static extern bool IsPaidVersion();

        [DllImport(PluginLibraryName)]
        public static extern IntPtr GetVersionString();

        public static readonly string version = Marshal.PtrToStringAnsi(GetVersionString());

        public enum EventID : int
        {
            None = 0,
            StepPhysics = 1,
            Draw = 2,
            UpdateLiquidParameters = 3,
            UpdateManipulatorParameters = 4,
            ClearSDFAndID = 5,
            CreateFluidInstance = 6,
            RegisterParticlesBuffers = 7,
            SetCameraParameters = 8,
            SetRenderParameters = 9,
            RegisterManipulators = 10,
            RegisterSolverBuffers = 11,
            RegisterRenderResources = 12,
            ReleaseResources = 13,
            InitializeGpuReadback = 14,
            UpdateReadback = 15,
            SetCameraParams = 16,
            UpdateMeshRenderGlobalParameters = 17,
            InitializeGraphicsPipeline = 18,
        }

        public struct EventData
        {
            public int InstanceID;
            public IntPtr ExtraData;
        };

        public enum LogLevel
        {
            Verbose = 0,
            Info = 1,
            Performance = 2,
            Warning = 3,
            Error = 4,
        }

        public enum TextureFormat
        {
            None,
            R8G8B8A8_SNorm,
            R16G16B16A16_SFloat,
            R32G32B32A32_SFloat,
            R16_SFloat,
            R32_SFloat,
        }

        public static TextureFormat ToBridgeTextureFormat(GraphicsFormat format)
        {
            switch (format)
            {
            case GraphicsFormat.R8G8B8A8_UNorm:
                return TextureFormat.R8G8B8A8_SNorm;
            case GraphicsFormat.R16G16B16A16_SFloat:
                return TextureFormat.R16G16B16A16_SFloat;
            case GraphicsFormat.R32G32B32A32_SFloat:
                return TextureFormat.R32G32B32A32_SFloat;
            case GraphicsFormat.R16_SFloat:
                return TextureFormat.R16_SFloat;
            case GraphicsFormat.R32_SFloat:
                return TextureFormat.R32_SFloat;
            default:
                return TextureFormat.None;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DebugMessage
        {
            public IntPtr Text;
            public LogLevel Level;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct LoggerSettings
        {
            public IntPtr PFNCallback;
            public LogLevel LogLevel;
        };

        public static int EventAndInstanceID(EventID eventID, int InstanceID)
        {
            return (int)eventID | (InstanceID << 8);
        }

        public static void SubmitInstanceEvent(CommandBuffer cmd, int instanceID, EventID eventID,
                                               IntPtr data = default)
        {
            EventData eventData;
            eventData.InstanceID = instanceID;
            eventData.ExtraData = data;

            IntPtr eventDataNative = Marshal.AllocHGlobal(Marshal.SizeOf(eventData));
            Marshal.StructureToPtr(eventData, eventDataNative, true);

            cmd.IssuePluginEventAndData(GetRenderEventWithDataFunc(), (int)eventID, eventDataNative);
        }

        public static bool NeedGarbageCollect()
        {
            switch (UnityEngine.SystemInfo.graphicsDeviceType)
            {
            case GraphicsDeviceType.Vulkan:
            case GraphicsDeviceType.Direct3D12:
            case GraphicsDeviceType.XboxOneD3D12:
#if UNITY_2020_3_OR_NEWER
            case GraphicsDeviceType.GameCoreXboxOne:
            case GraphicsDeviceType.GameCoreXboxSeries:
#endif
                return true;
            default:
                return false;
            }
        }
    }
}
