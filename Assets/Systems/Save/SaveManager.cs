using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

namespace Save
{
    public static class SaveManager
    {
        public static string SaveDirectory
        {
            get => PlayerPrefs.GetString("SavePath", Application.persistentDataPath);
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Debug.LogError("Save path cannot be null or empty");
                    return;
                }

                if (File.Exists(value))
                {
                    Debug.LogError("Save path cannot be a file");
                    return;
                }

                if (!Directory.Exists(value))
                {
                    try
                    {
                        Directory.CreateDirectory(value);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Failed to create directory {value}: {ex.Message}");
                        return;
                    }
                }

                PlayerPrefs.SetString("SavePath", value);
                Debug.Log($"Save path set to {value}");
            }
        }

        private static string ResolvePath(string relativePath)
        {
            var fullPath = Path.Join(SaveDirectory.AsSpan(), relativePath.AsSpan());
            var directory = Path.GetDirectoryName(fullPath);
            if (directory == null || Directory.Exists(directory)) return fullPath;
            try
            {
                Directory.CreateDirectory(directory);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to create directory {directory}: {ex.Message}");
            }

            return fullPath;
        }

        public static void SaveByBf(string relativePath, object toSave)
        {
            var formatter = new BinaryFormatter();
            var fullPath = ResolvePath(relativePath);
            var stream = new FileStream(fullPath, FileMode.Create);
            formatter.Serialize(stream, toSave);
            stream.Close();
            Debug.Log($"Saved binary file {fullPath}");
        }

        public static object LoadByBf(string relativePath, object defaultSave)
        {
            var fullPath = ResolvePath(relativePath);
            var data = defaultSave;
            if (File.Exists(fullPath))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(fullPath, FileMode.Open);
                data = formatter.Deserialize(stream);
                stream.Close();
                Debug.Log($"Loaded binary file {fullPath}");
            }
            else
            {
                SaveByBf(relativePath, defaultSave);
            }

            return data;
        }

        public static void SaveByXML(string relativePath, object toSave)
        {
            var serializer = new XmlSerializer(toSave.GetType());
            var fullPath = ResolvePath(relativePath);
            var stream = new FileStream(fullPath, FileMode.Create);
            serializer.Serialize(stream, toSave);
            stream.Close();
            Debug.Log($"Saved xml file {fullPath}");
        }

        public static object LoadByXML(string relativePath, object defaultSave)
        {
            var fullPath = ResolvePath(relativePath);
            var data = defaultSave;
            if (File.Exists(fullPath))
            {
                var serializer = new XmlSerializer(defaultSave.GetType());
                var stream = new FileStream(fullPath, FileMode.Open);
                data = serializer.Deserialize(stream);
                stream.Close();
                Debug.Log($"Loaded xml file {fullPath}");
            }
            else
            {
                SaveByXML(relativePath, defaultSave);
            }

            return data;
        }

        public static void SaveByDataContractSerializer(string relativePath, object toSave)
        {
            var serializer = new DataContractSerializer(toSave.GetType());
            var fullPath = ResolvePath(relativePath);
            var stream = new FileStream(fullPath, FileMode.Create);
            serializer.WriteObject(stream, toSave);
            stream.Close();
            Debug.Log($"Saved xml file DCS {fullPath}");
        }

        public static object LoadByDataContractSerializer(string relativePath, object defaultSave)
        {
            var fullPath = ResolvePath(relativePath);
            var data = defaultSave;
            if (File.Exists(fullPath))
            {
                var serializer = new DataContractSerializer(defaultSave.GetType());
                var stream = new FileStream(fullPath, FileMode.Open);
                data = serializer.ReadObject(stream);
                stream.Close();
                Debug.Log($"Loaded xml file DCS {fullPath}");
            }
            else
            {
                SaveByDataContractSerializer(relativePath, defaultSave);
            }

            return data;
        }

        public static void SaveByJson(string relativePath, object toSave)
        {
            var fullPath = ResolvePath(relativePath);
            var json = JsonUtility.ToJson(toSave);
            File.WriteAllText(fullPath, json);
            Debug.Log($"Saved json file {fullPath}");
        }

        public static object LoadByJson(string relativePath, object defaultSave)
        {
            var fullPath = ResolvePath(relativePath);
            var data = defaultSave;
            if (File.Exists(fullPath))
            {
                var json = File.ReadAllText(fullPath);
                data = JsonUtility.FromJson(json, defaultSave.GetType());
                Debug.Log($"Loaded json file {fullPath}");
            }
            else
            {
                SaveByJson(relativePath, defaultSave);
            }

            return data;
        }

        public static void Save(string relativePath, object defaultSave, SaveType saveType = SaveType.Json)
        {
            switch (saveType)
            {
                case SaveType.BinaryFormatter:
                    SaveByBf(relativePath, defaultSave);
                    break;
                case SaveType.XML:
                    SaveByXML(relativePath, defaultSave);
                    break;
                case SaveType.DataContractSerializer:
                    SaveByDataContractSerializer(relativePath, defaultSave);
                    break;
                case SaveType.Json:
                    SaveByJson(relativePath, defaultSave);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(saveType), saveType, null);
            }
        }

        public static object Load(string relativePath, object defaultSave, SaveType saveType = SaveType.Json)
        {
            return saveType switch
            {
                SaveType.BinaryFormatter => LoadByBf(relativePath, defaultSave),
                SaveType.XML => LoadByXML(relativePath, defaultSave),
                SaveType.DataContractSerializer => LoadByDataContractSerializer(relativePath, defaultSave),
                SaveType.Json => LoadByJson(relativePath, defaultSave),
                _ => throw new ArgumentOutOfRangeException(nameof(saveType), saveType, null)
            };
        }

        public static void Delete(string relativePath)
        {
            if (string.IsNullOrEmpty(Path.GetExtension(relativePath)))
            {
                Debug.LogError($"Invalid file path: {relativePath}");
                return;
            }

            var fullPath = Path.Join(SaveDirectory.AsSpan(), relativePath.AsSpan());
            if (!File.Exists(fullPath))
            {
                Debug.LogError($"File not found: {relativePath}");
                return;
            }

            try
            {
                File.Delete(fullPath);
                Debug.Log($"Deleted file: {relativePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to delete file {relativePath}: {ex.Message}");
            }
        }
    }
}