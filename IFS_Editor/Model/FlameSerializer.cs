using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IFS_Editor.Model
{
    public class FlameSerializer
    {
        public static Flame Load(string path)
        {
            Flame f = new Flame();
            List<string> chaos = new List<string>();
            using (XmlReader r = XmlReader.Create(path))
            {
                while (r.Read())
                {
                    switch (r.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch(r.Name)
                            {
                                case "flame":
                                    break;
                                case "xform":
                                    XForm xf = new XForm
                                    {
                                        name = r["name"] ?? "",//kesobb replace variation nevekkel
                                        color = Double.Parse(r["color"].Replace('.', ',')),
                                        opacity = Double.Parse(r["opacity"].Replace('.', ',')),
                                        baseWeight = Double.Parse(r["weight"].Replace('.', ',')),
                                        PreCoefs = GenCoefs(r["coefs"]),
                                        PostCoefs = GenCoefs(r["post"]),
                                        symmetry = Double.Parse((r["symmetry"]?? "0").Replace('.', ','))
                                    };
                                    chaos.Add(r["chaos"]);//null: mindenkivel osszekot

                                    //TODO: variable es variation read: r[i]

                                    f.AddXForm(xf);
                                    break;
                                case "palette":
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            Console.WriteLine(r.Value);
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            Console.Write("</" + r.Name);
                            Console.WriteLine(">");
                            break;
                        default://debug
                            Console.WriteLine(r.NodeType);
                            break;
                    }
                }
            }

            //TODO: itt osszekotesek chaos alapjan, ures name replace variation nevekkel

            return f;
        }

        public static void Save(Flame f, string path)
        {

        }

        private static List<double> GenCoefs(string vals)
        {
            if (vals == null)
                vals = "1 0 0 1 0 0";//default
            List<string> l = vals.Replace('.',',').Split(' ').ToList();
            return l.Select(x => double.Parse(x)).ToList();
        }
    }
}
