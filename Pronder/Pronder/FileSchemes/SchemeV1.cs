using Pronder.Handlers;
using Pronder.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pronder.FileSchemes
{
    class SchemeV1 : IBinaryScheme
    {
        public static ushort SchemeVersion = 1;

        private const int FieldName = 0;
        private const int FieldScore = 1;
        private const int FieldTodoList = 2;
        private const int FieldCount = 3;

        public void Serialize(BinaryWriter writer, object data)
        {
            var d = (SchemeV1Data)data;

            long tableStart = writer.BaseStream.Position;
            long[] offsets = new long[FieldCount];

            for (int i = 0; i < FieldCount; i++)
                writer.Write((long)0);

            offsets[FieldName] = writer.BaseStream.Position;
            writer.Write(d.Name);

            offsets[FieldScore] = writer.BaseStream.Position;
            writer.Write(d.Score);

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

            reader.BaseStream.Seek(offsets[FieldScore], SeekOrigin.Begin);
            var score = reader.ReadInt32();

            reader.BaseStream.Seek(offsets[FieldTodoList], SeekOrigin.Begin);
            var todos = ReadTodoList(reader);

            return new SchemeV1Data { Name = name, Score = score };
        }

        public List<string> ReadTodoList(string filePath)
        {
            using var fs = File.OpenRead(filePath);
            using var reader = new BinaryReader(fs);

            SkipFileHeader(reader);
            long[] offsets = ReadOffsetTable(reader);

            reader.BaseStream.Seek(offsets[FieldTodoList], SeekOrigin.Begin);

            return ReadTodoList(reader);
        }


        private long[] ReadOffsetTable(BinaryReader reader)
        {
            var offsets = new long[FieldCount];

            for (int i = 0; i < FieldCount; i++)
                offsets[i] = reader.ReadInt64();

            return offsets;
        }

        private List<string> ReadTodoList(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            var list = new List<string>(count);

            for (int i = 0; i < count; i++)
                list.Add(reader.ReadString());

            return list;
        }

        private void SkipFileHeader(BinaryReader reader)
        {
            reader.BaseStream.Seek(BinaryFileHandler.Magic.Length + 2, SeekOrigin.Begin);
        }
    }

    public class SchemeV1Data
    {
        public string Name { get; set; }
        public int Score { get; set; }
    }
}
