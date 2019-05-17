using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.VHD.Definitions;

namespace Cake.VHD
{
    [CakeAliasCategory("Vhd")]
    public static class VhdAliases
    {
        [CakeMethodAlias]
        public static void VhdCreate(this ICakeContext context, FilePath vhdPath, ulong size, uint blockSize, VhdType vhdType)
        {
            //JMeter(context, testFile, null);
        }

        //VhdNew
        //VhdGet
        //VhdMount
        //VhdInitialize
        //VhdNewPartition
        //VhdFormat
        //VhdAccessPath
        //VhdResize
        //VhdPartitionResize
        //VhdDismount


        //var disk = Call_NewVHD(vhdPath, size, blockSize, VhdType.Dynamic);
        //Call_MountVHD(vhdPath);
        //var vhd = Call_GetVHD(vhdPath);
        //Call_InitializeDisk(vhd.DiskNumber.Value);
        //var part = Call_NewPartition(vhd.DiskNumber.Value);
        //Call_AddPartitionAccessPath(part, mountPath);
        //Call_FormatVolume(part, FileSystemType.NTFS);

        //Call_ResizeVHD(vhdPath, size* 2);
        //var maxSize = Call_GetPartitionSupportedSize(part);
        //Call_ResizePartition(part, maxSize);

        //vhd = Call_GetVHD(vhdPath);
        //var partition = Call_GetPartition(vhd.DiskNumber.Value);
        //Call_RemovePartitionAccessPath(partition, mountPath);
        //Call_DismountVHD(vhdPath);
    }
}
