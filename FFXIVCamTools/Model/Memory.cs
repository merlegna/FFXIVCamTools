using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FFXIVCamTools.Model
{
    class Memory
    {
        public Memory()
        {

        }

        public static MemoryAddress GetMemoryAddress(Process process, Settings settings)
        {
            IntPtr baseAddress;
            MemoryAddress memoryAddress = new MemoryAddress();

            try
            {
                baseAddress = new IntPtr(BitConverter.ToInt64(
                    Read(process.Handle, IntPtr.Add(process.MainModule.BaseAddress, settings.StructureAddress), 8), 0));

                memoryAddress.ZoomCurrent = IntPtr.Add(baseAddress, settings.ZoomCurrent);
                memoryAddress.ZoomMax = IntPtr.Add(baseAddress, settings.ZoomMax);
                memoryAddress.FovCurrent = IntPtr.Add(baseAddress, settings.FovCurrent);
                memoryAddress.FovMax = IntPtr.Add(baseAddress, settings.FovMax);
            }
            catch
            {
                throw new Exception("Failed to get memoryaddress");
            }

            return memoryAddress;
        }

        public static void Write(IntPtr hProcess, IntPtr address, byte[] buffer)
        {
            if (0 >= WriteProcessMemory(hProcess, address, buffer, buffer.Length, out var write))
                throw new Exception("Unable to write process memory");
        }

        public static byte[] Read(IntPtr hProcess, IntPtr address, int size)
        {
            byte[] buffer = new byte[size];

            if (!ReadProcessMemory(hProcess, address, buffer, size, out var read))
                throw new Exception("Unable to read process memory");

            return buffer;
        }

        [DllImport("kernel32")]
        private static extern int WriteProcessMemory(
           IntPtr hProcess, IntPtr IpBaseAddress,
           byte[] IpBuffer, int nSize, out int IpNumberOfBytesWritten);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool ReadProcessMemory(
            IntPtr hProcess, IntPtr IpBaseAddress,
            byte[] IpBuffer, int nSize, out int IpNumberOfBytesRead);
    }
}
