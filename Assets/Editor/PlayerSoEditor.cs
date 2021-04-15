using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(PlayerSo))]
    public class PlayerSoEditor : UnityEditor.Editor
    {
        private bool _foldout;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var player = (PlayerSo) target;
            var achievements = player.GetAchievements().GetDictionary();
            if (GUILayout.Button("Show"))
                _foldout = !_foldout;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            if (!_foldout) return;
            foreach (var kvp in achievements)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(kvp.Key.ToString(), GUILayout.Width(100));

                EditorGUILayout.BeginVertical();
                DrawProperty("Best", kvp.Value.GetBest());
                DrawProperty("Last", kvp.Value.GetLast());
                DrawProperty("Total", kvp.Value.GetTotal());
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }

        private void DrawProperty(string label, float value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(100));
            EditorGUILayout.LabelField(value.ToString(CultureInfo.InvariantCulture));
            EditorGUILayout.EndHorizontal();
        }
    }
}