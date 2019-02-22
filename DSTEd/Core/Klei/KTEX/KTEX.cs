using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using Core.Klei.Squish;

namespace DSTEd.Core.Klei.KTEX {
    class KTEX {
        public class InvalidTEXFileException : Exception { public InvalidTEXFileException(string msg) : base(msg) { } };

        public enum Platform {
            PC = 12,
            XBOX360 = 11,
            PS3 = 10,
            Unknown = 0
        };

        public enum PixelFormat : uint {
            DXT1 = 0, DXT3 = 1, DXT5 = 2,
            ARGB = 4,
            Unknown = 7
        };

        public enum TextureType : uint {
            [Description("1D")]
            OneD = 1,
            [Description("2D")]
            TwoD = 2,
            [Description("3D")]
            ThreeD = 3,
            [Description("Cube Mapped")]
            Cubemap = 4
        };

        public readonly char[] KTEXHeader = new char[] { 'K', 'T', 'E', 'X' };

        public struct FileStruct {
            public struct HeaderStruct {
                public uint Platform;
                public uint PixelFormat;
                public uint TextureType;
                public uint NumMips;
                public uint Flags;
                public uint Remainder;
            }

            public HeaderStruct Header;
            public byte[] Raw;
        }

        public FileStruct File;

        public struct Mipmap {
            public ushort Width;
            public ushort Height;
            public ushort Pitch;
            public uint DataSize;
            public byte[] Data;
        };

        public KTEX() {
        }

        public KTEX(Stream stream) {
            using (var reader = new BinaryReader(stream)) {
                if (!reader.ReadChars(4).SequenceEqual(KTEXHeader)) {
                    throw new InvalidTEXFileException("The first 4 bytes do not match 'KTEX'.");
                }

                var header = reader.ReadUInt32();

                Console.WriteLine(Convert.ToString(header, 2));

                File.Header.Platform = header & 15;
                File.Header.PixelFormat = (header >> 4) & 31;
                File.Header.TextureType = (header >> 9) & 15;
                File.Header.NumMips = (header >> 13) & 31;
                File.Header.Flags = (header >> 18) & 3;
                File.Header.Remainder = (header >> 20) & 4095;

                // Just a little hack for pre cave updates, can remove later.
                OldRemainder = (header >> 14) & 262143;

                File.Raw = reader.ReadBytes((int) (reader.BaseStream.Length - reader.BaseStream.Position));
            }
        }

        public Boolean IsValid() {
            return true;
        }

        public Platform GetPlatform() {
            return ((Platform) this.File.Header.Platform);
        }

        public PixelFormat GetFormat() {
            return ((PixelFormat) this.File.Header.PixelFormat);
        }

        public string GetTexType() {
            return "NONE";// return EnumHelper<TextureType>.GetEnumDescription(((TextureType) this.File.Header.TextureType).ToString());
        }

        public int GetWidth() {
            return this.GetMainMipmap().Width;
        }

        public int GetHeight() {
            return this.GetMainMipmap().Height;
        }

        public Bitmap GenerateBitMap() {
            byte[] argbData;
            Mipmap mipmap = GetMainMipmap();

            switch ((PixelFormat) this.GetFormat()) {
                case PixelFormat.DXT1:
                    argbData = Squish.DecompressImage(mipmap.Data, mipmap.Width, mipmap.Height, SquishFlags.Dxt1);
                    break;
                case PixelFormat.DXT3:
                    argbData = Squish.DecompressImage(mipmap.Data, mipmap.Width, mipmap.Height, SquishFlags.Dxt3);
                    break;
                case PixelFormat.DXT5:
                    argbData = Squish.DecompressImage(mipmap.Data, mipmap.Width, mipmap.Height, SquishFlags.Dxt5);
                    break;
                case PixelFormat.ARGB:
                    argbData = mipmap.Data;
                    break;
                default:
                    throw new Exception("Unknown pixel format?");
            }

            var imgReader = new BinaryReader(new MemoryStream(argbData));

            Bitmap pt = new Bitmap((int) mipmap.Width, (int) mipmap.Height);

            for (int y = 0; y < mipmap.Height; y++) {
                for (int x = 0; x < mipmap.Width; x++) {
                    byte r = imgReader.ReadByte();
                    byte g = imgReader.ReadByte();
                    byte b = imgReader.ReadByte();
                    byte a = imgReader.ReadByte();
                    pt.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
                //if (OnProgressUpdate != null) {
                //OnProgressUpdate(y * 100 / mipmap.Height);
                //}
            }
            return pt;
        }

        /* A little hacky but it gets the job done. */
        private uint OldRemainder;
        public bool IsPreCaveUpdate() {
            return OldRemainder == 262143;
        }

        public Mipmap[] GetMipmaps() {
            Mipmap[] mipmapArray = new Mipmap[File.Header.NumMips];

            using (var reader = new BinaryReader(new MemoryStream(File.Raw))) {
                for (int i = 0; i < File.Header.NumMips; i++) {
                    mipmapArray[i] = new Mipmap();
                    mipmapArray[i].Width = reader.ReadUInt16();
                    mipmapArray[i].Height = reader.ReadUInt16();
                    mipmapArray[i].Pitch = reader.ReadUInt16();
                    mipmapArray[i].DataSize = reader.ReadUInt32();
                }

                for (int i = 0; i < File.Header.NumMips; i++) {
                    mipmapArray[i].Data = reader.ReadBytes((int) mipmapArray[i].DataSize);
                }
            }

            return mipmapArray;
        }

        public Mipmap[] GetMipmapsSummary() {
            Mipmap[] mipmapArray = new Mipmap[File.Header.NumMips];

            using (var reader = new BinaryReader(new MemoryStream(File.Raw))) {
                for (int i = 0; i < File.Header.NumMips; i++) {
                    mipmapArray[i] = new Mipmap();
                    mipmapArray[i].Width = reader.ReadUInt16();
                    mipmapArray[i].Height = reader.ReadUInt16();
                    mipmapArray[i].Pitch = reader.ReadUInt16();
                    mipmapArray[i].DataSize = reader.ReadUInt32();
                }
            }

            return mipmapArray;
        }

        public Mipmap GetMainMipmap() {
            Mipmap mipmap = new Mipmap();

            using (var reader = new BinaryReader(new MemoryStream(File.Raw))) {
                mipmap.Width = reader.ReadUInt16();
                mipmap.Height = reader.ReadUInt16();
                mipmap.Pitch = reader.ReadUInt16();
                mipmap.DataSize = reader.ReadUInt32();

                reader.BaseStream.Seek((File.Header.NumMips - 1) * 10, SeekOrigin.Current);

                mipmap.Data = reader.ReadBytes((int) mipmap.DataSize);
            }

            return mipmap;
        }

        public Mipmap GetMainMipmapSummary() {
            Mipmap mipmap = new Mipmap();

            using (var reader = new BinaryReader(new MemoryStream(File.Raw))) {
                mipmap.Width = reader.ReadUInt16();
                mipmap.Height = reader.ReadUInt16();
                mipmap.Pitch = reader.ReadUInt16();
                mipmap.DataSize = reader.ReadUInt32();
            }

            return mipmap;
        }

        public void SaveFile(Stream stream) {
            using (var writer = new BinaryWriter(stream)) {
                writer.Write(KTEXHeader);

                uint header = 0;

                header |= 4095;
                header <<= 2;
                header |= File.Header.Flags;
                header <<= 5;
                header |= File.Header.NumMips;
                header <<= 4;
                header |= File.Header.TextureType;
                header <<= 5;
                header |= File.Header.PixelFormat;
                header <<= 4;
                header |= File.Header.Platform;

                writer.Write(header);

                writer.Write(File.Raw);
            }
        }
    }
}
