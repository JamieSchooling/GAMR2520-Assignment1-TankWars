using UnityEditor;
using UnityEngine;

/// <summary>
/// Automatically opens a custom editor window for rule editing when a CAD_Rules asset is double-clicked.
/// </summary>
public class CAD_RulesAssetOpener : AssetModificationProcessor
{
    /// <summary>
    /// Callback triggered when an asset is opened in the Unity editor.
    /// If the opened asset is of type <see cref="CAD_Rules"/>, it opens a custom rules editor window.
    /// </summary>
    /// <param name="instanceID">The instance ID of the opened asset.</param>
    /// <param name="line">The line number clicked in the asset (unused).</param>
    /// <returns>
    /// Returns true if the asset was handled (opened in the custom editor), otherwise false to allow default Unity behavior.
    /// </returns>
    [UnityEditor.Callbacks.OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        // Get the selected asset by its instance ID.
        Object obj = EditorUtility.InstanceIDToObject(instanceID);

        // Check if the selected asset is of type CAD_Rules.
        if (obj is CAD_Rules decisionRules)
        {
            // Open the custom rules editor window.
            CAD_RulesEditorWindow window = EditorWindow.GetWindow<CAD_RulesEditorWindow>();
            window.titleContent = new GUIContent("Rules Editor");
            window.Show();

            // Assign the opened CAD_Rules asset to the editor window.
            window.SetAsset(decisionRules);

            // Indicate that the asset was handled by this custom logic.
            return true;
        }

        // Return false to allow Unity's default behavior for non-CAD_Rules assets.
        return false;
    }
}
