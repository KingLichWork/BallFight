#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class FindAndDestroyMissingScripts : EditorWindow
{
    [MenuItem("Auto/Remove Missing Scripts Recursively")]
    private static void FindAndRemoveMissingInSelected()
    {
        var deepSelection = EditorUtility.CollectDeepHierarchy(Selection.gameObjects);
        var compCount = 0;
        var goCount = 0;

        foreach (var o in deepSelection)
        {
            if (o is not GameObject go)
                continue;

            var count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);

            if (count <= 0)
                continue;

            Undo.RegisterCompleteObjectUndo(go, "Remove missing scripts");
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
            compCount += count;
            goCount++;
        }

        Debug.Log($"Found and removed {compCount} missing scripts from {goCount} GameObjects");
    }
}
#endif