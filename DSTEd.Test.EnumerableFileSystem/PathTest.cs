using Path = DSTEd.Core.IO.EnumerableFileSystem.Path;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DSTEd.Test.IO.EnumerableFileSystem
{
	[TestClass]
	public class PathTest
	{
		static Path p1_p2_file_hid_ext = new Path("/parent1/parent2/.filename.ext");
		const string hid_ext = ".foo.bar";
		const string hid = ".foo";
		const string ext = "a.b";
		const string long_ext = "a.b.cd.ef";
		const string p1_file = "/parent/file";
		const string p1_empty = "/parent/";
		[TestMethod]
		public void Extension()
		{
			//hid = unix hidden file
			Assert.AreEqual(".ext", p1_p2_file_hid_ext.Extension,"2*parent hid filename ext");
			Assert.AreEqual(".foo", new Path(hid).Extension, "hid");
			Assert.AreEqual(".bar", new Path(hid_ext).Extension, "hid ext");
			Assert.AreEqual(".b", new Path(ext).Extension, "ext");
			Assert.AreEqual(".ef", new Path(long_ext).Extension, "long ext");
		}

		[TestMethod]
		public void FileName()
		{
			Assert.AreEqual(".foo.bar", new Path(hid_ext).FileName, "hid ext");
			Assert.AreEqual(".filename.ext", p1_p2_file_hid_ext.FileName, "2*parent hidname ext");
			Assert.AreEqual(long_ext, new Path(long_ext).FileName, "long ext");
		}

		[TestMethod]
		public void Parent()
		{
			Assert.AreEqual("\\parent", new Path(p1_file).Parent, "p1 file");
			Assert.AreEqual("\\parent", new Path(p1_empty).Parent, "p1 empty");
			Assert.AreEqual("\\parent1\\parent2", p1_p2_file_hid_ext.Parent, "2parent hidfile ext");
			Assert.AreEqual(string.Empty, new Path(hid).Parent, "no parent");
		}
	}
}
