using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Threading;
using IFS_Editor.Model;

namespace IFS_Editor.Serialization
{
    public static class FlameCollectionSerializer
    {

        public static List<Flame> LoadFile(string path, out string CollectionName)
        {
            using (StreamReader s = new StreamReader(path))
                return FlameSerializer.LoadStream(s.BaseStream, out CollectionName);
        }
        
        public static void SaveFile(string name, List<Flame> flames, string path)
        {
            string sf = "\r\n";
            foreach (Flame f in flames)
            {
                sf += FlameSerializer.SerializeFlame(f).ToString() + "\r\n";
            }
            using (XmlWriter w = XmlWriter.Create(path, SerializationUtils.xws))
            {

                w.WriteStartElement("flames");
                w.WriteAttributeString("name", name);
                w.WriteRaw(sf);//w.WriteString(sf);
                w.WriteEndElement();//flames
            }
        }

    }
}
