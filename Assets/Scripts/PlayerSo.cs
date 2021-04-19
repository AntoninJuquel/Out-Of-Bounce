using UnityEngine;
using UnityEditor;
using System.Globalization;
using Systems.Achievement;
using Systems.Unlock;


[CreateAssetMenu(fileName = "New player data", menuName = "Player", order = 0)]
public class PlayerSo : ScriptableObject
{
    [SerializeField] private Vault vault = new Vault();
    [SerializeField] private Achievements achievements = new Achievements();
    public Achievements GetAchievements() => achievements;
}
#if UNITY_EDITOR
[CustomEditor(typeof(PlayerSo))]
public class PlayerSoEditor : Editor
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
#endif