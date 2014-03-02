using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace QuadLogic.Framework.Utilities
{
    public class ObjectSerializer
    {
        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inObject">The input object.</param>
        /// <returns></returns>
        public static string SerializeObject<T>(T inObject)
        {
            // create a MemoryStream here, we are just working exclusively in memory
            using (Stream stream = new System.IO.MemoryStream())
            {
                // The XmlTextWriter takes a stream and encoding as one of its constructors
                using (XmlTextWriter xtWriter = new System.Xml.XmlTextWriter(stream, Encoding.UTF8))
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        serializer.Serialize(xtWriter, inObject);
                        xtWriter.Flush();
                        // go back to the beginning of the Stream to read its contents
                        stream.Seek(0, System.IO.SeekOrigin.Begin);
                        // read back the contents of the stream and supply the encoding
                        using (StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8))
                        {
                            string result = reader.ReadToEnd();
                            return result;
                        }
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                    finally
                    {
                        if (stream != null) stream.Close();
                        if (xtWriter != null) xtWriter.Close();
                    }
                }
            }
        }
        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inObject">The input object.</param>
        /// <returns></returns>
        public static string SerializeObject<T>(T inObject, Encoding encodeType)
        {
            if (encodeType == null)
                encodeType = Encoding.UTF8;
            // create a MemoryStream here, we are just working exclusively in memory
            using (Stream stream = new System.IO.MemoryStream())
            {
                // The XmlTextWriter takes a stream and encoding as one of its constructors
                using (XmlTextWriter xtWriter = new System.Xml.XmlTextWriter(stream, encodeType))
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        serializer.Serialize(xtWriter, inObject);
                        xtWriter.Flush();
                        // go back to the beginning of the Stream to read its contents
                        stream.Seek(0, System.IO.SeekOrigin.Begin);
                        // read back the contents of the stream and supply the encoding
                        using (StreamReader reader = new System.IO.StreamReader(stream, encodeType))
                        {
                            string result = reader.ReadToEnd();
                            return result;
                        }
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                    finally
                    {
                        if (stream != null) stream.Close();
                        if (xtWriter != null) xtWriter.Close();
                    }
                }
            }
        }

        public static T DeserialzeObject<T>(string xmlFilePath)
        {
            XmlDocument _Doc = new XmlDocument();
            _Doc.Load(xmlFilePath);

            XmlSerializer _XMLSer = new XmlSerializer(typeof(T));
            return (T)_XMLSer.Deserialize(new StringReader(_Doc.OuterXml));
        }

        public static T DeserialzeObject<T>(Stream stream)
        {
            XmlDocument _Doc = new XmlDocument();
            _Doc.Load(stream);

            XmlSerializer _XMLSer = new XmlSerializer(typeof(T));
            return (T)_XMLSer.Deserialize(new StringReader(_Doc.OuterXml));
        }

        private static string UTF8ByteArrayToString(Byte[] byteChars)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String outString = encoding.GetString(byteChars);

            return (outString);
        }

        private static Byte[] StringToUTF8ByteArray(String xmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(xmlString);

            return byteArray;
        }
    }
}
