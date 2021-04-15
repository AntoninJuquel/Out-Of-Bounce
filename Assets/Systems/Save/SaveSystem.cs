using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

namespace Systems.Save
{
    public static class SaveManager
    {
        static string saveDirectory => PlayerPrefs.GetString("SavePath", string.Concat(Application.persistentDataPath, "/"));
        public static void SetupSavePath(string path = "")
        {
            if (path != "")
                PlayerPrefs.SetString("SavePath", path);
        }
        public static void SaveByBF(string relativePath, object toSave)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = string.Concat(saveDirectory, relativePath);
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, toSave);
            stream.Close();
            Debug.Log("Saved binary file " + path);
        }

        public static object LoadByBF(string relativePath, object defaultSave)
        {
            string path = string.Concat(saveDirectory, relativePath);
            object data = defaultSave;
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                data = formatter.Deserialize(stream);
                stream.Close();
                Debug.Log("Loaded binary file " + path);
            }
            else SaveByBF(relativePath, defaultSave);

            return data;
        }

        public static void SaveByXML(string relativePath, object toSave)
        {
            XmlSerializer serializer = new XmlSerializer(toSave.GetType());
            string path = string.Concat(saveDirectory, relativePath);
            FileStream stream = new FileStream(path, FileMode.Create);
            serializer.Serialize(stream, toSave);
            stream.Close();
            Debug.Log("Saved xml file " + path);
        }

        public static object LoadByXML(string relativePath, object defaultSave)
        {
            string path = string.Concat(saveDirectory, relativePath);
            object data = defaultSave;
            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(defaultSave.GetType());
                FileStream stream = new FileStream(path, FileMode.Open);
                data = serializer.Deserialize(stream);
                stream.Close();
                Debug.Log("Loaded xml file " + path);
            }
            else SaveByXML(relativePath, defaultSave);

            return data;
        }

        public static void SaveByDataContractSerializer(string relativePath, object toSave)
        {
            DataContractSerializer serializer = new DataContractSerializer(toSave.GetType());
            string path = string.Concat(saveDirectory, relativePath);
            FileStream stream = new FileStream(path, FileMode.Create);
            serializer.WriteObject(stream,toSave);
            stream.Close();
            Debug.Log("Saved xml file DCS " + path);
        }

        public static object LoadByDataContractSerializer(string relativePath, object defaultSave)
        {
            string path = string.Concat(saveDirectory, relativePath);
            object data = defaultSave;
            if (File.Exists(path))
            {
                DataContractSerializer serializer = new DataContractSerializer(defaultSave.GetType());
                FileStream stream = new FileStream(path, FileMode.Open);
                data = serializer.ReadObject(stream);
                stream.Close();
                Debug.Log("Loaded xml file DCS " + path);
            }
            else SaveByXML(relativePath, defaultSave);

            return data;
        }

        public static void Delete(string relativePath)
        {
            try
            {
                File.Delete(string.Concat(saveDirectory, relativePath));
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}