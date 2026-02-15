using Pronder.Handlers;
using Pronder.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Pronder.FileSchemes
{
    class SchemeV1 : IBinaryScheme
    {
        public static ushort SchemeVersion = 1;

        private const int FieldName = 0;
        private const int FieldCount = 1;

        public void Serialize(BinaryWriter writer, object data)
        {
            var d = (SchemeV1Data)data;
            long tableStart = writer.BaseStream.Position;
            long[] offsets = new long[FieldCount];
            for (int i = 0; i < FieldCount; i++)
                writer.Write((long)0);

            offsets[FieldName] = writer.BaseStream.Position;
            writer.Write(d.Name);

            long end = writer.BaseStream.Position;
            writer.BaseStream.Seek(tableStart, SeekOrigin.Begin);
            foreach (var offset in offsets)
                writer.Write(offset);
            writer.BaseStream.Seek(end, SeekOrigin.Begin);
        }

        public object Deserialize(BinaryReader reader)
        {
            long[] offsets = ReadOffsetTable(reader);

            reader.BaseStream.Seek(offsets[FieldName], SeekOrigin.Begin);
            var name = reader.ReadString();

            return new SchemeV1Data { Name = name };
        }

        private long[] ReadOffsetTable(BinaryReader reader)
        {
            var offsets = new long[FieldCount];
            for (int i = 0; i < FieldCount; i++)
                offsets[i] = reader.ReadInt64();
            return offsets;
        }

        private void SkipFileHeader(BinaryReader reader)
        {
            reader.BaseStream.Seek(BinaryFileHandler.Magic.Length + 2, SeekOrigin.Begin);
        }
    }

    public class SchemeV1Data
    {
        public string Name { get; set; }
    }
}