using System;

namespace Save
{
    public interface ISave
    {
        public event Action<string, object> OnSave;
        public object DefaultSave { get; }
        public string Name { get; }
        public void Load(object loadedObject);
        public void Save();
    }
}