using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace DSTEd.Core.Contents.Editors
{
    class Zip:IDisposable
    {
        private ZipArchive zipfile = null;
        public Zip(string Filepath)//construct one with file
        {
            zipfile = new ZipArchive(new System.IO.FileStream(Filepath ,System.IO.FileMode.Open));
        }

        public void Dispose()
        {
            ((IDisposable)zipfile).Dispose();
        }

        public List<string> GetFileList()//returns a list includes Full name
        {
            var ret = new List<string>();
            foreach(var a in zipfile.Entries)
            {
                ret.Add(a.FullName);
            }
            return ret;
        }

        public System.IO.Stream ReadFileFromZip(string FullName)//read file by Fullname from GetFileList()
        {
            return zipfile.GetEntry(FullName).Open();
        }
        public void WriteFileIntoZip(System.IO.Stream Istream, string FullName)
        {
            var entry = zipfile.CreateEntry(FullName);
            using (var writeoperator = new System.IO.StreamWriter(entry.Open()))
            {
                writeoperator.Write(Istream);
            }
        }
    }
}
