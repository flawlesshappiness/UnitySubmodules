using UnityEditor;

public class CreateEmptyScript
{
    [MenuItem("Assets/Create/C#/Empty", false, 80)]
    private static void CreateScript()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile("Assets/UnitySubmodules/Editor/Templates/EmptyScriptTemplate.cs.txt", "New MonoBehaviour.cs");
    }
}
