using UnityEditor;

public class CreateSingletonScript
{
    [MenuItem("Assets/Create/C# Script (Singleton)", false, 80)]
    private static void CreateScript()
    {
        var path_dest = AssetDatabase.GetAssetPath(Selection.activeInstanceID) + "/New MonoBehaviour.cs";
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile("Assets/UnitySubmodules/Editor/Templates/SingletonScriptTemplate.cs.txt", path_dest);
    }
}
