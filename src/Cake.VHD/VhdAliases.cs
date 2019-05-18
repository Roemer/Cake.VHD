using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.VHD.Core;
using Cake.VHD.Definitions;

namespace Cake.VHD
{
    [CakeAliasCategory("Vhd")]
    public static class VhdAliases
    {
        [CakeMethodAlias]
        public static VirtualHardDisk VhdCreate(this ICakeContext context, FilePath vhdPath, ulong size, uint blockSize, VhdType vhdType)
        {
            var vhdFullPath = vhdPath.MakeAbsolute(context.Environment).FullPath;
            return PowerShellWrapper.Call_NewVHD(vhdFullPath, size, blockSize, vhdType);
        }

        [CakeMethodAlias]
        public static VirtualHardDisk VhdGet(this ICakeContext context, FilePath vhdPath)
        {
            var vhdFullPath = vhdPath.MakeAbsolute(context.Environment).FullPath;
            return PowerShellWrapper.Call_GetVHD(vhdFullPath);
        }

        [CakeMethodAlias]
        public static void VhdMount(this ICakeContext context, FilePath vhdPath, bool readOnly)
        {
            var vhdFullPath = vhdPath.MakeAbsolute(context.Environment).FullPath;
            PowerShellWrapper.Call_MountVHD(vhdFullPath, readOnly);
        }

        [CakeMethodAlias]
        public static void VhdDismount(this ICakeContext context, FilePath vhdPath)
        {
            var vhdFullPath = vhdPath.MakeAbsolute(context.Environment).FullPath;
            PowerShellWrapper.Call_DismountVHD(vhdFullPath);
        }

        [CakeMethodAlias]
        public static void VhdInitialize(this ICakeContext context, VirtualHardDisk vhd)
        {
            PowerShellWrapper.Call_InitializeDisk(vhd.DiskNumber);
        }

        [CakeMethodAlias]
        public static Partition VhdCreatePartition(this ICakeContext context, VirtualHardDisk vhd)
        {
            return PowerShellWrapper.Call_NewPartition(vhd.DiskNumber);
        }

        [CakeMethodAlias]
        public static Partition VhdGetPartition(this ICakeContext context, VirtualHardDisk vhd)
        {
            return PowerShellWrapper.Call_NewPartition(vhd.DiskNumber);
        }

        [CakeMethodAlias]
        public static void VhdFormat(this ICakeContext context, Partition partition, FileSystemType fileSystem)
        {
            PowerShellWrapper.Call_FormatVolume(partition, fileSystem);
        }

        [CakeMethodAlias]
        public static void VhdResize(this ICakeContext context, FilePath vhdPath, ulong size)
        {
            var vhdFullPath = vhdPath.MakeAbsolute(context.Environment).FullPath;
            PowerShellWrapper.Call_ResizeVHD(vhdFullPath, size);
        }

        [CakeMethodAlias]
        public static void VhdResizePartition(this ICakeContext context, Partition partition)
        {
            var maxSize = PowerShellWrapper.Call_GetPartitionSupportedSize(partition);
            PowerShellWrapper.Call_ResizePartition(partition, maxSize);
        }

        [CakeMethodAlias]
        public static void VhdOptimize(this ICakeContext context, FilePath vhdPath, VhdCompactMode compactMode)
        {
            var vhdFullPath = vhdPath.MakeAbsolute(context.Environment).FullPath;
            PowerShellWrapper.Call_OptimizeVHD(vhdFullPath, compactMode);
        }

        [CakeMethodAlias]
        public static void VhdAddAccessPath(this ICakeContext context, Partition partition, FilePath accessPath)
        {
            var accessFullPath = accessPath.MakeAbsolute(context.Environment).FullPath;
            PowerShellWrapper.Call_AddPartitionAccessPath(partition, accessFullPath);
        }

        [CakeMethodAlias]
        public static void VhdRemoveAccessPath(this ICakeContext context, Partition partition, FilePath accessPath)
        {
            var accessFullPath = accessPath.MakeAbsolute(context.Environment).FullPath;
            PowerShellWrapper.Call_RemovePartitionAccessPath(partition, accessFullPath);
        }

        //var disk = Call_NewVHD(vhdPath, size, blockSize, VhdType.Dynamic);
        //Call_MountVHD(vhdPath);
        //var vhd = Call_GetVHD(vhdPath);
        //Call_InitializeDisk(vhd.DiskNumber.Value);
        //var part = Call_NewPartition(vhd.DiskNumber.Value);
        //Call_FormatVolume(part, FileSystemType.NTFS);
        //Call_AddPartitionAccessPath(part, mountPath);

        //Call_ResizeVHD(vhdPath, size* 2);
        //var maxSize = Call_GetPartitionSupportedSize(part);
        //Call_ResizePartition(part, maxSize);

        //vhd = Call_GetVHD(vhdPath);
        //var partition = Call_GetPartition(vhd.DiskNumber.Value);
        //Call_RemovePartitionAccessPath(partition, mountPath);
        //Call_DismountVHD(vhdPath);
    }
}
