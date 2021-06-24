using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonSerialisableScriptableObjectEditor<T> : Editor where T : JsonSerialisableScriptableObject<T>
{
    public override void OnInspectorGUI()
    {
        var scriptableObject = (T) target;
        using ( new EditorGUILayout.HorizontalScope() )
        {
            if ( GUILayout.Button( "Load File" ) )
            {
                var filename = EditorUtility.OpenFilePanel( "Load Json File", Application.dataPath, "json" );
                if ( !string.IsNullOrEmpty( filename ) && File.Exists( filename ) )
                {
                    scriptableObject.LoadJsonFromFile( filename );
                }
            }

            if ( GUILayout.Button( "Save File" ) )
            {
                var filename = EditorUtility.SaveFilePanel( "Save Json File", Application.dataPath,
                    scriptableObject.name + ".json", "json" );
                if ( !string.IsNullOrEmpty( filename ) )
                {
                    scriptableObject.DumpJsonToFile( filename );
                }
            }
        }

        base.OnInspectorGUI();
    }
}