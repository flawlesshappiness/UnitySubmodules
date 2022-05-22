using UnityEditor;

public class CreateEmptyScript
{
    [MenuItem("Assets/Create/C# Script (Empty)", false, 80)]
    private static void CreateScript()
    {
        var path_dest = AssetDatabase.GetAssetPath(Selection.activeInstanceID) + "/New MonoBehaviour.cs";
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile("Assets/UnitySubmodules/Editor/Templates/EmptyScriptTemplate.cs.txt", path_dest);
    }
}
