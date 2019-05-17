using Cake.VHD.Definitions;
using Microsoft.Management.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;

namespace Cake.VHD.Core
{
    public static class PowerShellWrapper
    {
        /// <summary>
        /// Basic method to execute PowerShell commandlets.
        /// </summary>
        /// <param name="commandlet">The name of the commandlet to execute.</param>
        /// <param name="parameters">A dictionary with parameters for the commandlet.</param>
        /// <returns>The resulting objects from the execution.</returns>
        public static Collection<PSObject> ExecutePowerShell(string commandlet, IDictionary parameters)
        {
            var ps = PowerShell.Create();
            ps.AddCommand(commandlet);
            if (parameters.Count > 0)
            {
                ps.AddParameters(parameters);
            }
            var result = ps.Invoke();
            if (ps.HadErrors)
            {
                throw ps.Streams.Error.ElementAt(0).Exception;
            }
            return result;
        }

        public static ulong Call_GetPartitionSupportedSize(Partition partition)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("DiskNumber", partition.DiskNumber);
            parameters.Add("PartitionNumber", partition.PartitionNumber);
            var result = ExecutePowerShell("Get-PartitionSupportedSize", parameters);
            return (ulong)result[0].Properties["SizeMax"].Value;
        }

        public static void Call_ResizePartition(Partition partition, ulong partitionSize)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("DiskNumber", partition.DiskNumber);
            parameters.Add("PartitionNumber", partition.PartitionNumber);
            parameters.Add("Size", partitionSize);
            var result = ExecutePowerShell("Resize-Partition", parameters);
        }

        public static void Call_ResizeVHD(string vhdPath, ulong diskSize)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Path", vhdPath);
            parameters.Add("SizeBytes", diskSize);
            var result = ExecutePowerShell("Resize-VHD", parameters);
        }

        public static void Call_OptimizeVHD(string vhdPath, VhdCompactMode compactMode = VhdCompactMode.Quick)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Path", vhdPath);
            parameters.Add("Mode", compactMode.ToString());
            var result = ExecutePowerShell("Optimize-VHD", parameters);
        }

        public static VirtualHardDisk Call_GetVHD(string vhdPath)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Path", vhdPath);
            var result = ExecutePowerShell("Get-VHD", parameters);
            var vhd = (VirtualHardDisk)result[0].BaseObject;
            return vhd;
        }

        public static void Call_InitializeDisk(uint diskNumber)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Number", diskNumber);
            var result = ExecutePowerShell("Initialize-Disk", parameters);
        }

        public static Partition Call_NewPartition(uint diskNumber, bool useMaxSize = true)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("DiskNumber", diskNumber);
            parameters.Add("UseMaximumSize", useMaxSize);
            var result = ExecutePowerShell("New-Partition", parameters);
            var origObject = (CimInstance)result[0].BaseObject;
            var partition = new Partition(origObject);
            return partition;
        }

        public static Partition Call_GetPartition(uint diskNumber)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("DiskNumber", diskNumber);
            var result = ExecutePowerShell("Get-Partition", parameters);
            // Use the second one as the first is the "Reserved" one
            var origObject = (CimInstance)result[1].BaseObject;
            var partition = new Partition(origObject);
            return partition;
        }

        public static void Call_FormatVolume(Partition partition, FileSystemType fileSystem, bool confirm = false, bool force = true)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Partition", partition.CimObject);
            parameters.Add("FileSystem", fileSystem.ToString());
            parameters.Add("Confirm", confirm);
            parameters.Add("Force", force);
            var result = ExecutePowerShell("Format-Volume", parameters);
        }

        public static void Call_AddPartitionAccessPath(Partition partition, string accessPath)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("DiskNumber", partition.DiskNumber);
            parameters.Add("PartitionNumber", partition.PartitionNumber);
            parameters.Add("AccessPath", accessPath);
            var result = ExecutePowerShell("Add-PartitionAccessPath", parameters);
        }

        public static void Call_RemovePartitionAccessPath(Partition partition, string accessPath)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("DiskNumber", partition.DiskNumber);
            parameters.Add("PartitionNumber", partition.PartitionNumber);
            parameters.Add("AccessPath", accessPath);
            var result = ExecutePowerShell("Remove-PartitionAccessPath", parameters);
        }

        public static void Call_MountVHD(string vhdPath, bool readOnly = false)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Path", vhdPath);
            parameters.Add("ReadOnly", readOnly);
            var result = ExecutePowerShell("Mount-VHD", parameters);
        }

        public static void Call_DismountVHD(string vhdPath)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Path", vhdPath);
            var result = ExecutePowerShell("Dismount-VHD", parameters);
        }

        // Microsoft.HyperV.PowerShell.Objects.dll
        //public static Microsoft.Vhd.PowerShell.VirtualHardDisk Call_NewVHD(string vhdPath, ulong diskSize, uint blockSize = 0, VhdType vhdType = VhdType.Dynamic, string parentPath = null)
        public static VirtualHardDisk Call_NewVHD(string vhdPath, ulong diskSize, uint blockSize = 0, VhdType vhdType = VhdType.Dynamic, string parentPath = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Path", vhdPath);
            parameters.Add("SizeBytes", diskSize);
            if (blockSize > 0)
            {
                parameters.Add("BlockSizeBytes", blockSize);
            }
            switch (vhdType)
            {
                case VhdType.Differencing:
                    parameters.Add("Differencing", true);
                    parameters.Add("ParentPath", parentPath);
                    break;
                case VhdType.Fixed:
                    parameters.Add("Fixed", true);
                    break;
                case VhdType.Dynamic:
                default:
                    parameters.Add("Dynamic", true);
                    break;
            }
            var result = ExecutePowerShell("New-VHD", parameters);
            var vhd = (VirtualHardDisk)result[0].BaseObject;
            return vhd;
        }
    }
}
