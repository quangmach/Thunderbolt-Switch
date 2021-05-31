﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DockerForm
{
    public enum Constructor
    {
        Intel = 0,
        Nvidia = 1,
        AMD = 2
    }

    public enum Type
    {
        Internal = 0,
        Discrete = 1
    }

    public class VideoController
    {
        public string DeviceID;
        public string PNPDeviceID;
        public string Name;
        public string Description;
        public uint ErrorCode;
        public Constructor Constructor;
        public Type Type;

        internal void Initialize()
        {
            if (Name.ToLower().Contains("nvidia"))
                Constructor = Constructor.Nvidia;
            else if (Name.ToLower().Contains("amd"))
                Constructor = Constructor.AMD;
            else if (Name.ToLower().Contains("intel"))
                Constructor = Constructor.Intel;

            Type = DeviceID == "VideoController1" ? Type.Internal : Type.Discrete;
        }

        public bool IsDisabled()
        {
            return ErrorCode == 22;
        }

        public bool IsEnabled()
        {
            return ErrorCode == 0;
        }

        public bool EnableDevice(string devconPath)
        {
            using (var EnableProcess = Process.Start(new ProcessStartInfo(devconPath, $" /enable \"{this.Name}\"") { CreateNoWindow = true, RedirectStandardOutput = true, UseShellExecute = false, Verb = "runas" }))
                return true;
        }

        public bool DisableDevice(string devconPath)
        {
            using (var EnableProcess = Process.Start(new ProcessStartInfo(devconPath, $" /disable \"{this.Name}\"") { CreateNoWindow = true, RedirectStandardOutput = true, UseShellExecute = false, Verb = "runas" }))
            {
                ErrorCode = 22;
                return true;
            }
        }
    }
}
