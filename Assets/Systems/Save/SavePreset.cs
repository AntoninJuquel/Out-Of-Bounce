using System;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Save
{
    [CreateAssetMenu(fileName = "New Save Preset", menuName = "Save", order = 0)]
    public class SavePreset : SerializedScriptableObject
    {
        [SerializeField] private string directory = "";

        [SerializeField] private SaveType saveType = SaveType.Json;
        [OdinSerialize] private ISave[] saveObjects = { };

        private void OnEnable()
        {
            foreach (var saveObject in saveObjects)
            {
                saveObject.OnSave += Save;
                var loaded = Load(saveObject.Name, saveObject.DefaultSave);
                saveObject.Load(loaded);
            }
        }

        private void OnDisable()
        {
            foreach (var saveObject in saveObjects)
            {
                saveObject.OnSave -= Save;
            }
        }

        private void OnDestroy()
        {
            foreach (var saveObject in saveObjects)
            {
                saveObject.OnSave -= Save;
            }
        }

        [Button]
        public void Save(string fileName, object toSave)
        {
            SaveManager.Save(Path.Join(directory.AsSpan(), fileName.AsSpan()), toSave, saveType);
        }

        [Button]
        public object Load(string fileName, object defaultSave)
        {
            return SaveManager.Load(Path.Join(directory.AsSpan(), fileName.AsSpan()), defaultSave, saveType);
        }
    }
}