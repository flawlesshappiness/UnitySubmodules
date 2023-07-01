using UnityEditor;

public class CreateFakeEnum
{
    [MenuItem("Assets/Create/C#/Fake Enum", false, 80)]
    private static void CreateScript()
    {
        var path_dest = AssetDatabase.GetAssetPath(Selection.activeInstanceID) + "/New FakeEnum.cs";
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile("Assets/UnitySubmodules/Editor/Templates/FakeEnumTemplate.cs.txt", path_dest);
    }
}