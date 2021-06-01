using System;
using System.Collections.Generic;
using Systems.Unlock;
using UnityEngine;

namespace Skin
{
    [CreateAssetMenu(fileName = "New skin", menuName = "Skins/Skin", order = 0)]
    public class SkinSo : UnlockableSo
    {
        [SerializeField] private SkinType skinType;
        [SerializeField] private bool selected;
        public bool Selected() => selected;
        public SkinType GetSkinType() => skinType;
        public void SetSelected(bool value) => selected = value;
    }

    public enum SkinType
    {
        Balls,
        Platforms,
        Trails,
        Particles
    }

    public static class SkinUtilities
    {
        public static IEnumerable<SkinType> SkinTypesArray() => (SkinType[]) Enum.GetValues(typeof(SkinType));
    }

    [Serializable]
    public class SkinSave : UpgradableSave
    {
        public bool Selected;
    }
}