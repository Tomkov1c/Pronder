using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pronder.Interfaces
{
    interface IBinaryScheme
    {
        static ushort SchemeVersion { get; }
        void Serialize(BinaryWriter writer, object data);
        object Deserialize(BinaryReader reader);
    }
}
