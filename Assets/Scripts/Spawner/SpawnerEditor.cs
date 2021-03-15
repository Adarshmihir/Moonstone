#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        //draw spawner settings hidden in default inspector
        if (!(target is Spawner script)) return;
        EditorGUILayout.LabelField("Spawner Settings", EditorStyles.boldLabel);
        script.spawnerRadius = EditorGUILayout.FloatField("Spawner Radius", script.spawnerRadius);
        script.maxArraySize = EditorGUILayout.IntField("Max Number Spawned", script.maxArraySize);
        script.bDelay = GUILayout.Toggle(script.bDelay, "Use Delay");
        if (!script.bDelay) return;
        script.spawnTime = EditorGUILayout.FloatField("Spawn Time", script.spawnTime);
        script.spawnDelay = EditorGUILayout.FloatField("Spawn Delay", script.spawnDelay);
    }
}
#endif