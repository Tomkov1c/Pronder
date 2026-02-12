using Pronder.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Pronder.Handlers
{
    class BinaryFileHandler
    {
        public static readonly byte[] Magic = { 0x70, 0x72, 0x6F, 0x6E, 0x64 };

        public static void Write(string path, IBinaryScheme scheme, object data)
        {
            var versionField = scheme.GetType().GetField("Version", BindingFlags.Public | BindingFlags.Static);
            if (versionField == null)
                throw new InvalidOperationException($"{scheme.GetType().Name} is missing a static Version field.");

            ushort version = (ushort)(versionField.GetValue(null) ?? 
                   throw new InvalidOperationException($"{scheme.GetType().Name} has a null Version field."));

            using var fs = File.OpenWrite(path);
            using var writer = new BinaryWriter(fs);
            writer.Write(Magic);
            writer.Write(version);
            scheme.Serialize(writer, data);
        }

        public static object Read(string path)
        {
            using var fs = File.OpenRead(path);
            using var reader = new BinaryReader(fs);

            var magic = reader.ReadBytes(Magic.Length);
            if (!magic.SequenceEqual(Magic))
                throw new InvalidDataException("Not a valid file: bad magic bytes.");

            ushort schemeId = reader.ReadUInt16();
            var scheme = FindSchemeForVersion(schemeId);

            return scheme.Deserialize(reader);
        }

        public static IBinaryScheme FindSchemeForVersion(ushort fileVersion)
        {
            var schemeType = typeof(IBinaryScheme);

            var match = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => schemeType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .FirstOrDefault(t =>
                {
                    var versionField = t.GetField("Version", BindingFlags.Public | BindingFlags.Static);
                    if (versionField == null) return false;

                    return (ushort)(versionField.GetValue(null) ?? 0) == fileVersion;
                });

            if (match == null)
                throw new InvalidOperationException($"No scheme found for version {fileVersion}.");

            return (IBinaryScheme)(Activator.CreateInstance(match) ?? 
                   throw new InvalidOperationException($"Failed to create instance of {match.Name}."));
        }
    }
}
