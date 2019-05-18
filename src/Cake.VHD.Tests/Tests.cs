using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.VHD.Definitions;
using NSubstitute;
using NUnit.Framework;
using System.IO;

namespace Cake.VHD.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test()
        {
            var env = FakeEnvironment.CreateWindowsEnvironment();
            var context = Substitute.For<ICakeContext>();

            var vhdPath = new FilePath(@"c:\temp\test.vhdx");
            var size = 10ul * 1024 * 1024 * 1024;

            VhdAliases.VhdCreate(context, vhdPath, size, 0, VhdType.Dynamic);
            VhdAliases.VhdMount(context, vhdPath, false);
            var vhd = VhdAliases.VhdGet(context, vhdPath);
            VhdAliases.VhdInitialize(context, vhd);
            var part = VhdAliases.VhdCreatePartition(context, vhd);
            VhdAliases.VhdFormat(context, part, FileSystemType.NTFS);
            //Call_AddPartitionAccessPath(part, mountPath);

            VhdAliases.VhdDismount(context, vhdPath);

            File.Delete(vhdPath.FullPath);

            Assert.Pass();
        }
    }
}
