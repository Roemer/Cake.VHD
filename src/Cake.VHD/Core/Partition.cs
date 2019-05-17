using Microsoft.Management.Infrastructure;

namespace Cake.VHD.Core
{
    public class Partition
    {
        public Partition(CimInstance cimObject)
        {
            CimObject = cimObject;
            DiskNumber = (uint)cimObject.CimInstanceProperties["DiskNumber"].Value;
            PartitionNumber = (uint)cimObject.CimInstanceProperties["PartitionNumber"].Value;
        }

        public CimInstance CimObject { get; private set; }
        public uint DiskNumber { get; private set; }
        public uint PartitionNumber { get; private set; }
    }
}
