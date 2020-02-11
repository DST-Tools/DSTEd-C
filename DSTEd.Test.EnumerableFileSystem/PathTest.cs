using DSTEd.Core.IO.EnumerableFileSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSTEd.Test.IO.EnumerableFileSystem
{
	[TestClass]
	public class PathTest
	{
		static Path p1_p2_file_hid_ext = new Path("/parent1/parent2/.filename.ext");

		[TestMethod]
		public void Extension()
		{
			string hid_ext = ".foo.bar";
			string hid = ".foo";
			string ext = "a.b";
			string long_ext = "a.b.cd.ef";
			Assert.AreEqual(".ext", p1_p2_file_hid_ext,"2*parent hid filename ext");
			Assert.AreEqual(string.Empty, new Path(hid).Extension, "hid");
			Assert.AreEqual(".bar", new Path(hid_ext).Extension, "hid ext");
			Assert.AreEqual(".b", new Path(ext).Extension, "ext");
			Assert.AreEqual(".ef", new Path(long_ext).Extension, "long ext");
		}

		[TestMethod]
		public void FileName()
		{
			Assert.AreEqual("filename", p1_p2_file_hid_ext.FileName,"filename");
		}
	}
}
