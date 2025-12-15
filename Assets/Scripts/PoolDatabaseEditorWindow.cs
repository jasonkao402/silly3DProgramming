using UnityEditor;
using UnityEngine;

public class PoolDatabaseEditorWindow : EditorWindow
{
    private PoolConfigDatabase database;
    private Vector2 scroll;

    [MenuItem("Tools/Pooling/Pool Database Editor")]
    static void OpenWindow()
    {
        GetWindow<PoolDatabaseEditorWindow>("Pool Database Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Pool Config Database", EditorStyles.boldLabel);

        // Drag & drop database
        database = (PoolConfigDatabase)EditorGUILayout.ObjectField("Database", database, typeof(PoolConfigDatabase), false);

        if (database == null)
        {
            EditorGUILayout.HelpBox("Assign a PoolConfigDatabase asset.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space();
        scroll = EditorGUILayout.BeginScrollView(scroll);

        var serializedObj = new SerializedObject(database);
        var entriesProp = serializedObj.FindProperty("entries");

        EditorGUILayout.PropertyField(entriesProp, true);

        serializedObj.ApplyModifiedProperties();

        EditorGUILayout.EndScrollView();
    }
}
