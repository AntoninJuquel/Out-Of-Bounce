using Systems.Unlock;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New skin", menuName = "Skin", order = 0)]
    public class SkinSo : UnlockableSo
    {
        [SerializeField] private bool selected;
        public bool Selected() => selected;
    }
}