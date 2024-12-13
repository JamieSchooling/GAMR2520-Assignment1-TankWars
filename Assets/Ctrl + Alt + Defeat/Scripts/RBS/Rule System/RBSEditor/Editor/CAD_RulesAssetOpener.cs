using UnityEditor;
using UnityEngine;

public class CAD_DecisionRulesAssetOpener : AssetModificationProcessor
{
    // Called when an asset is double-clicked
    /// <summary>
    /// creates an editor asset that can be used to make a set of rules
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    [UnityEditor.Callbacks.OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        // Get the selected asset   
        Object obj = EditorUtility.InstanceIDToObject(instanceID);

        // Check if the selected asset is of type CAD_Rules
        if (obj is CAD_Rules decisionRules)
        {
            // Open the custom editor window
            CAD_RulesEditorWindow window = EditorWindow.GetWindow<CAD_RulesEditorWindow>();
            window.titleContent = new GUIContent("Rules Editor");
            window.Show();

            // Assign the asset to the editor window
            window.SetAsset(decisionRules);

            // Return true to indicate that the asset was handled
            return true;
        }

        // Return false to allow Unity's default behavior for other assets
        return false;
    }
}
