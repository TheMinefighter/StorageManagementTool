using System;
using System.IO;
using System.Threading.Tasks;
using StorageManagementCore.Backend;
using StorageManagementCore.Operation;
using System.Net.Http;
using System.Runtime.InteropServices;
//!So block 3 first, then 2 then 1
namespace Test {
	internal class Program {
		public static unsafe void Main(string[] args) {
			Offsets();
			ulong a = 0x42F2_1D43_F38B_F404, b = 0x74121AC00135F25C;
			Guid O = new Guid(0xF38BF404, 0x1D43, 0x42F2, 0x93, 0x05, 0x67, 0xDE, 0x0B, 0x28, 0xFC, 0x23);
			ulong y = *((ulong*) &O);
			ulong z = *((ulong*)( (&O)+7) );
			ulong* ap = &a;
			ulong* bp = &b;
			Guid F = new Guid(*((uint*) ap), *((ushort*) (ap + 4)), *((ushort*) (ap + 6)), *((byte*) (bp)), *((byte*) (bp + 1)),
				*((byte*) (bp + 2)), *((byte*) (bp + 3)), *((byte*) (bp + 4)), *((byte*) (bp + 5)), *((byte*) (bp+6)), *((byte*) (bp+7)));
			Console.WriteLine(O == F);
//			Updater u = new Updater();
//				await u.GetReleasesData();

			//PagefileManagement.GetFutureFreeSpace(new PagefileSysConfiguration(){SystemManaged = true});
		}

		private static void Offsets() {
			Marshal.OffsetOf(typeof(Guid), "_a");
			Marshal.OffsetOf(typeof(Guid), "_b");
			Marshal.OffsetOf(typeof(Guid), "_c");
			Marshal.OffsetOf(typeof(Guid), "_d");
			Marshal.OffsetOf(typeof(Guid), "_e");
			Marshal.OffsetOf(typeof(Guid), "_f");
			Marshal.OffsetOf(typeof(Guid), "_g");
			Marshal.OffsetOf(typeof(Guid), "_h");
			Marshal.OffsetOf(typeof(Guid), "_i");
			Marshal.OffsetOf(typeof(Guid), "_j");
			Marshal.OffsetOf(typeof(Guid), "_k");
		}
	}
}