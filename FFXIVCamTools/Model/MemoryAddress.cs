using System;

namespace FFXIVCamTools.Model
{
    class MemoryAddress
    {
        public IntPtr ZoomCurrent { get; set; }
        public IntPtr ZoomMax { get; set; }
        public IntPtr FovCurrent { get; set; }
        public IntPtr FovMax { get; set; }

        public MemoryAddress() { }
    }
}
