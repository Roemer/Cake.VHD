using System;
using System.Collections.Generic;
using System.Text;

namespace Cake.VHD
{
    public class VirtualHardDisk
    {
        public string Path { get; internal set; }
        public uint DiskNumber { get; internal set; }
    }
}
