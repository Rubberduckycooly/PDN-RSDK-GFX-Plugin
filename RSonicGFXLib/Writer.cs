﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RSDKvRS
{
    public class Writer : BinaryWriter
    {
        public Writer(Stream stream) : base(stream)
        {
        }

        public Writer(string file) : base(File.Open(file, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
        {
        }

        public bool IsEof
        {
            get { return BaseStream.Position >= BaseStream.Length; }
        }

        public void Seek(long position, SeekOrigin org)
        {
            BaseStream.Seek(position, org);
        }


        public long Pos
        {
            get { return BaseStream.Position; }
        }

        public long Size
        {
            get { return BaseStream.Length; }
        }

        public string GetFilename()
        {
            var fileStream = BaseStream as FileStream;
            return fileStream.Name;
        }

        public void WriteUInt32BE(uint val)
        {
            val = ((val >> 24) & 0xff) | ((val << 8) & 0xff0000) | ((val >> 8) & 0xff00) | ((val << 24) & 0xff000000);
            base.Write(val);
        }

        public void WriteRSDKString(string val)
        {
            base.Write((byte)val.Length);
            base.Write(new UTF8Encoding().GetBytes(val));
        }

        public void WriteRSDKUnicodeString(string val)
        {
            base.Write((ushort)val.Length);
            base.Write(new UnicodeEncoding().GetBytes(val));
        }
    }
}
