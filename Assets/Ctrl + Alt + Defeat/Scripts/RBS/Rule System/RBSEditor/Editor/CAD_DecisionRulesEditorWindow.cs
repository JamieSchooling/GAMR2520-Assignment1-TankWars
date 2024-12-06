using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Collections.Generic;

public class CAD_DecisionRulesEditorWindow : EditorWindow
{
    private CAD_Rules decisionRules;
    private SerializedObject serializedDecisionRules;
    private ReorderableList ruleList;
    private List<string> knowledgeBaseMembers; 
    private List<bool> foldouts = new List<bool>();
    private Vector2 scrollPos; // Scroll position variable

    [MenuItem("Window/Decision Rules Editor")]
    public static CAD_DecisionRulesEditorWindow ShowWindow()
    {
        var window = GetWindow<CAD_DecisionRulesEditorWindow>("Decision Rules Editor");
        window.Show();
        return window;
    }

    private void OnEnable()
    {
        knowledgeBaseMembers = CAD_KnowledgeBaseUtils.GetBooleanMembers();
    }

    public void SetAsset(CAD_Rules rules)
    {
        decisionRules = rules;
        if (decisionRules != null)
        {
            serializedDecisionRules = new SerializedObject(decisionRules);
            InitializeRuleList();
        }
        else
        {
            serializedDecisionRules = null;
            ruleList = null;
        }
        Repaint();
    }

    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        var newDecisionRules = (CAD_Rules)EditorGUILayout.ObjectField("Decision Rules", decisionRules, typeof(CAD_Rules), false);
        if (EditorGUI.EndChangeCheck())
        {
            SetAsset(newDecisionRules);
        }

        if (decisionRules == null)
        {
            EditorGUILayout.HelpBox("Please assign a Decision Rules object.", MessageType.Info);
            return;
        }

        if (ruleList != null)
        {
            serializedDecisionRules.Update();
            // Begin Scroll View
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            // Draw the ReorderableList
            ruleList.DoLayoutList();
            // End Scroll View
            EditorGUILayout.EndScrollView();
            serializedDecisionRules.ApplyModifiedProperties();
        }
    }

    private void InitializeRuleList()
    {
        SerializedProperty rulesProp = serializedDecisionRules.FindProperty("rules");

        // Initialize foldouts based on the number of rules
        foldouts = new List<bool>();
        for (int i = 0; i < rulesProp.arraySize; i++)
        {
            foldouts.Add(false); // Initially collapsed
        }

        ruleList = new ReorderableList(serializedDecisionRules, rulesProp, true, true, true, true);

        // Draw header
        ruleList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Decision Rules (Highest priority at top)", EditorStyles.boldLabel);
        };

        // Draw each element
        ruleList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            SerializedProperty element = rulesProp.GetArrayElementAtIndex(index);

            SerializedProperty ruleNameProp = element.FindPropertyRelative("Name");
            SerializedProperty conditionsProp = element.FindPropertyRelative("conditions");
            SerializedProperty actionProp = element.FindPropertyRelative("action");

            // Ensure foldouts list is in sync
            if (foldouts.Count <= index)
                foldouts.Add(false);

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 2f;

            Rect nameRect = new Rect(rect.x + 20, rect.y + spacing, rect.width, lineHeight);
            EditorGUI.PropertyField(nameRect, ruleNameProp, new GUIContent(""));

            // Draw the foldout
            Rect foldoutRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
            foldouts[index] = EditorGUI.Foldout(foldoutRect, foldouts[index], "", true);

            if (foldouts[index])
            {
                EditorGUI.indentLevel++;

                float y = rect.y + EditorGUIUtility.singleLineHeight + 2;

                y += DrawComplexConditionsInline(new Rect(rect.x, y, rect.width, EditorGUIUtility.singleLineHeight), conditionsProp, ref rect);

                EditorGUILayout.Space();

                // Draw Action
                Rect actionRect = new Rect(rect.x, y, rect.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(
                    actionRect,
                    actionProp,
                    new GUIContent("Action", "Define the action to execute when conditions are met.")
                );

                EditorGUI.indentLevel--;
            }
        };

        // Calculate element height based on foldout state
        ruleList.elementHeightCallback = (int index) =>
        {
            SerializedProperty element = serializedDecisionRules.FindProperty("rules").GetArrayElementAtIndex(index);
            SerializedProperty conditionsProp = element.FindPropertyRelative("conditions");

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 2f;

            float height = lineHeight + spacing; // Foldout

            if (index < foldouts.Count && foldouts[index])
            {
                Rect rect = new();
                // Calculate height for conditions
                height += DrawComplexConditionsInline(new Rect(0, 0, 0, 0), conditionsProp, ref rect) + spacing;

                // Add height for Action field
                height += lineHeight + spacing;
            }

            return height;
        };

        // Draw background with divider lines and alternating colors
        ruleList.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (Event.current.type == EventType.Repaint)
            {
                // Alternate background color
                Color backgroundColor = index % 2 == 0 ? new Color(1.0f, 1.0f, 1.0f, 0.1f) : new Color(1.0f, 1.0f, 1.0f, 0.3f);
                EditorGUI.DrawRect(rect, backgroundColor);

                // Divider line
                Color dividerColor = EditorGUIUtility.isProSkin ? new Color(1, 1, 1, 0.1f) : new Color(0, 0, 0, 0.2f);
                EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - 1, rect.width, 1), dividerColor);
            }
        };

        ruleList.onAddCallback = (ReorderableList list) =>
        {
            int newIndex = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = newIndex;
            SerializedProperty newRule = list.serializedProperty.GetArrayElementAtIndex(newIndex);

            // Initialize new rule's name
            newRule.FindPropertyRelative("Name").stringValue = "New Rule";
            // Initialize other properties as needed

            // Add a new foldout state
            foldouts.Add(false);
        };

        ruleList.onRemoveCallback = (ReorderableList list) =>
        {
            if (list.index >= 0 && list.index < foldouts.Count)
            {
                foldouts.RemoveAt(list.index);
            }
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
        };

        ruleList.onReorderCallback = (ReorderableList list) =>
        {
            // Reorder the foldouts list to match the new order of the rules
            List<bool> newFoldouts = new List<bool>();
            SerializedProperty rulesProp = list.serializedProperty;
            for (int i = 0; i < rulesProp.arraySize; i++)
            {
                newFoldouts.Add(foldouts[i]);
            }
            foldouts = newFoldouts;
        };


    }

    private float DrawComplexConditionsInline(Rect rect, SerializedProperty conditionsProp, ref Rect parentRect)
    {
        SerializedProperty conditionsList = conditionsProp.FindPropertyRelative("conditions");
        SerializedProperty operatorsList = conditionsProp.FindPropertyRelative("operators");

        // Ensure at least one condition
        if (conditionsList.arraySize == 0)
            conditionsList.arraySize = 1;

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = 5f;
        float y = rect.y;
        float totalHeight = 0f;

        float toggleWidth = 55f;
        float operatorWidth = 60f;

        int conditionCount = conditionsList.arraySize;
        float totalConditionArea = (rect.width - (conditionCount - 1) * (operatorWidth + spacing));
        float conditionWidth = (totalConditionArea / conditionCount) - toggleWidth - spacing;

        // Draw the first condition with a negate toggle
        SerializedProperty firstCondition = conditionsList.GetArrayElementAtIndex(0);
        SerializedProperty firstConditionNameProp = firstCondition.FindPropertyRelative("Name");
        SerializedProperty firstConditionNegateProp = firstCondition.FindPropertyRelative("negate");

        // Toggle first
        Rect firstToggleRect = new Rect(rect.x, y, toggleWidth, lineHeight);
        firstConditionNegateProp.boolValue = EditorGUI.ToggleLeft(firstToggleRect, "Not", firstConditionNegateProp.boolValue);

        // Dropdown next
        int firstSelectedIndex = Mathf.Max(0, knowledgeBaseMembers.IndexOf(firstConditionNameProp.stringValue));
        Rect firstConditionRect = new Rect(rect.x + toggleWidth + spacing, y, conditionWidth, lineHeight);
        int newFirstIndex = EditorGUI.Popup(firstConditionRect, firstSelectedIndex, knowledgeBaseMembers.ToArray());
        if (newFirstIndex != firstSelectedIndex)
        {
            firstConditionNameProp.stringValue = knowledgeBaseMembers[newFirstIndex];
        }

        float x = rect.x + toggleWidth + spacing + conditionWidth + spacing; // Move past the first condition block

        // Subsequent conditions
        for (int i = 1; i < conditionsList.arraySize; i++)
        {
            if (operatorsList.arraySize < i)
                operatorsList.arraySize = i;

            // Operator
            EditorGUI.PropertyField(new Rect(x, y, operatorWidth, lineHeight), operatorsList.GetArrayElementAtIndex(i - 1), GUIContent.none);
            x += operatorWidth + spacing;

            // Condition
            SerializedProperty condition = conditionsList.GetArrayElementAtIndex(i);
            SerializedProperty conditionNameProp = condition.FindPropertyRelative("Name");
            SerializedProperty conditionNegateProp = condition.FindPropertyRelative("negate");

            // Toggle
            Rect toggleRect = new Rect(x, y, toggleWidth, lineHeight);
            conditionNegateProp.boolValue = EditorGUI.ToggleLeft(toggleRect, "Not", conditionNegateProp.boolValue);

            // Popup
            Rect conditionRect = new Rect(x + toggleWidth + spacing, y, conditionWidth, lineHeight);
            int selectedIndex = Mathf.Max(0, knowledgeBaseMembers.IndexOf(conditionNameProp.stringValue));
            int newIndex = EditorGUI.Popup(conditionRect, selectedIndex, knowledgeBaseMembers.ToArray());
            if (newIndex != selectedIndex)
            {
                conditionNameProp.stringValue = knowledgeBaseMembers[newIndex];
            }

            x += toggleWidth + spacing + conditionWidth + spacing;
        }

        y += lineHeight + spacing;

        // Add/Remove Buttons
        float buttonY = y;
        Rect addButtonRect = new Rect(rect.x, buttonY, 100, lineHeight);
        if (GUI.Button(addButtonRect, "Add Condition"))
        {
            conditionsList.arraySize++;
            operatorsList.arraySize = conditionsList.arraySize - 1;
        }

        if (conditionsList.arraySize > 1)
        {
            Rect removeButtonRect = new Rect(rect.x + 110, buttonY, 150, lineHeight);
            if (GUI.Button(removeButtonRect, "Remove Last Condition"))
            {
                conditionsList.arraySize--;
                operatorsList.arraySize = conditionsList.arraySize - 1;
            }
        }

        y += lineHeight + spacing;
        totalHeight = y - rect.y;

        return totalHeight;
    }
}
