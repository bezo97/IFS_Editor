using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace IFS_Editor.Serialization
{
    public static class FlameSerializer
    {

        public static Flame LoadString(string flamexml)
        {
            string dummy;//itt nem szamit, mert nincs collection
            using (MemoryStream s = new MemoryStream(Encoding.UTF8.GetBytes(flamexml)))
                return LoadStream(s, out dummy)[0];//clipboard rol csak 1 flame-et tartalmazo xml-t olvasunk be
        }

        public static List<Flame> LoadStream(Stream s, out string CollectionName)
        {
            CultureInfo tmpCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            CollectionName = "Unnamed Flame Collection";

            Flame f = null;
            List<Flame> flamek = new List<Flame>();
            SerializationUtils.xaos = new List<string>();
            using (XmlReader r = XmlReader.Create(s))
            {
                while (r.Read())
                {
                    switch (r.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (r.Name)
                            {
                                case "flame":
                                    if (f != null)//tobb flame is van a fajlban
                                    {
                                        f = SerializationUtils.Osszerak(f);
                                        flamek.Add(f);
                                    }
                                    SerializationUtils.xaos.Clear();
                                    f = new Flame
                                    {
                                        name = r["name"] ?? "unnamed flame",
                                        version = r["version"] ?? "unknown",
                                        renderSettings = new RenderSettings
                                        {
                                            SizeX = int.Parse((r["size"] ?? "1920 1080").Split(' ')[0]),
                                            SizeY = int.Parse((r["size"] ?? "1920 1080").Split(' ')[1]),
                                            Oversample = int.Parse(r["oversample"] ?? "2"),
                                            Filter = double.Parse(r["filter"] ?? "0.4"),
                                            Quality = int.Parse(r["quality"] ?? "4000")
                                        },
                                        imagingSettings = new ImagingSettings
                                        {
                                            Back_colorR = double.Parse((r["background"] ?? "0 0 0").Split(' ')[0]),
                                            Back_colorG = double.Parse((r["background"] ?? "0 0 0").Split(' ')[1]),
                                            Back_colorB = double.Parse((r["background"] ?? "0 0 0").Split(' ')[2]),
                                            Brightness = double.Parse(r["brightness"] ?? "1"),
                                            Gamma = double.Parse(r["gamma"] ?? "1"),
                                            G_threshold = double.Parse(r["gamma_threshold"] ?? "0")
                                        },
                                        cameraSettings = new CameraSettings
                                        {
                                            CenterX = double.Parse((r["center"] ?? "0 0").Split(' ')[0]),
                                            CenterY = double.Parse((r["center"] ?? "0 0").Split(' ')[1]),
                                            Scale = double.Parse(r["scale"] ?? "25"),
                                            Angle = double.Parse(r["angle"] ?? "0"),
                                            Rotate = double.Parse(r["rotate"] ?? "0"),
                                            Zoom = double.Parse(r["zoom"] ?? "0"),
                                            Cam_pitch = double.Parse(r["cam_pitch"] ?? "0"),
                                            Cam_yaw = double.Parse(r["cam_yaw"] ?? "0"),
                                            Cam_persp = double.Parse(r["cam_perspective"] ?? "0"),
                                            Cam_zpos = double.Parse(r["cam_zpos"] ?? "0"),
                                            Cam_dof = double.Parse(r["cam_dof"] ?? "0")
                                        }
                                    };
                                    break;
                                case "xform":
                                    XForm xf = XFormSerializer.DeserXForm(r);
                                    f.AddXForm(xf);
                                    break;
                                case "finalxform":
                                    XForm fxf = XFormSerializer.DeserXForm(r);
                                    f.finalxf = fxf;
                                    break;
                                case "palette":
                                    f.palette = r.ReadElementContentAsString();
                                    break;
                                case "flames":
                                    CollectionName = r["name"]?? "Unnamed Flame Collection";
                                    break;
                                default:
                                    Console.WriteLine("EZMIEZ: " + r.Name);
                                    break;
                            }
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            //Console.WriteLine(r.Value);
                            break;
                        case XmlNodeType.EndElement:
                            //
                            break;
                            /*case XmlNodeType.Whitespace:
                                break;*/
                    }
                }
            }

            f = SerializationUtils.Osszerak(f);//utolso flamet is osszerak
            flamek.Add(f);

            Thread.CurrentThread.CurrentCulture = tmpCulture;

            return flamek;
        }

        public static StringBuilder SerializeFlame(Flame f)
        {
            CultureInfo tmpCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            StringBuilder sb = new StringBuilder();
            using (XmlWriter w = XmlWriter.Create(sb, SerializationUtils.xws))
            {

                w.WriteStartElement("flame");
                w.WriteAttributeString("name", f.name);
                w.WriteAttributeString("version", "Node Editor");//pl a mi verzionk lehetne
                w.WriteAttributeString("size", f.renderSettings.SizeString);
                w.WriteAttributeString("center", f.cameraSettings.CenterString/*.Replace(',','.')*/);
                w.WriteAttributeString("scale", f.cameraSettings.Scale.ToString());
                w.WriteAttributeString("angle", f.cameraSettings.Angle.ToString());
                w.WriteAttributeString("rotate", f.cameraSettings.Rotate.ToString());
                w.WriteAttributeString("zoom", f.cameraSettings.Zoom.ToString());
                w.WriteAttributeString("cam_pitch", f.cameraSettings.Cam_pitch.ToString());
                w.WriteAttributeString("cam_yaw", f.cameraSettings.Cam_yaw.ToString());
                w.WriteAttributeString("cam_perspective", f.cameraSettings.Cam_persp.ToString());
                w.WriteAttributeString("cam_zpos", f.cameraSettings.Cam_zpos.ToString());
                w.WriteAttributeString("cam_dof", f.cameraSettings.Cam_dof.ToString());
                w.WriteAttributeString("oversample", f.renderSettings.Oversample.ToString());
                w.WriteAttributeString("filter", f.renderSettings.Filter.ToString());
                w.WriteAttributeString("quality", f.renderSettings.Quality.ToString());
                w.WriteAttributeString("background", f.imagingSettings.Back_ColorString/*.Replace(',','.')*/);
                w.WriteAttributeString("brightness", f.imagingSettings.Brightness.ToString());
                w.WriteAttributeString("gamma", f.imagingSettings.Gamma.ToString());
                w.WriteAttributeString("gamma_threshold", f.imagingSettings.G_threshold.ToString());
                //ezeket nem hasznaljuk, mert hibas a mukodesuk. legyen default value, az jo
                //estimator_radius="9"
                //estimator_minimum ="0"
                //estimator_curve ="0.4"
                //enable_de ="0"
                //plugins =""
                w.WriteAttributeString("new_linear", "1");//mindig
                //curves ="0 0 1 0 0 1 1 1 1 1 1 1 0 0 1 0 0 1 1 1 1 1 1 1 0 0 1 0 0 1 1 1 1 1 1 1 0 0 1 0 0 1 1 1 1 1 1 1"

                //xforms
                List<XForm> xfs = f.GetXForms();
                foreach (XForm xf in xfs)
                {
                    XFormSerializer.SerXForm(w, xf, xfs);
                }
                if(f.finalxf!=null)
                    XFormSerializer.SerXForm(w, f.finalxf, null/* => final*/);
                w.WriteStartElement("palette");
                w.WriteAttributeString("count", "256");
                w.WriteAttributeString("format", "RGB");
                w.WriteString(f.palette);
                //w.WriteElementString("palette", f.palette);
                w.WriteEndElement();//palette
                w.WriteEndElement();//flame
                w.Flush();
            }

            Thread.CurrentThread.CurrentCulture = tmpCulture;

            return sb;
        }

    }
}
