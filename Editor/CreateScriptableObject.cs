using UnityEditor;

public class CreateScriptableObject
{
    [MenuItem("Assets/Create/C#/ScriptableObject", false, 80)]
    private static void CreateScript()
    {
        var path_dest = AssetDatabase.GetAssetPath(Selection.activeInstanceID) + "/New ScriptableObject.cs";
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile("Assets/UnitySubmodules/Editor/Templates/ScriptableObjectTemplate.cs.txt", path_dest);
    }
}
