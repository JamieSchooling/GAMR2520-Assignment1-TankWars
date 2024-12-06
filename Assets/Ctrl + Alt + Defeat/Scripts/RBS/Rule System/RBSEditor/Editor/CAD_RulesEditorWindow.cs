using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Collections.Generic;

public class CAD_RulesEditorWindow : EditorWindow
{
    private CAD_Rules m_Rules;
    private SerializedObject m_SerializedRules;
    private ReorderableList m_RuleList;
    private List<string> m_KnowledgeBaseMembers; 
    private List<bool> m_Foldouts = new List<bool>();
    private Vector2 m_ScrollPosition;

    private void OnEnable()
    {
        m_KnowledgeBaseMembers = CAD_KnowledgeBaseUtils.GetBooleanMembers();
    }

    public void SetAsset(CAD_Rules rules)
    {
        m_Rules = rules;
        if (m_Rules != null)
        {
            m_SerializedRules = new SerializedObject(m_Rules);
            InitializeRuleList();
        }
        else
        {
            m_SerializedRules = null;
            m_RuleList = null;
        }
        Repaint();
    }

    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        var newDecisionRules = (CAD_Rules)EditorGUILayout.ObjectField("Rules", m_Rules, typeof(CAD_Rules), false);
        if (EditorGUI.EndChangeCheck())
        {
            SetAsset(newDecisionRules);
        }

        if (m_Rules == null)
        {
            EditorGUILayout.HelpBox("Please assign a Rules object.", MessageType.Info);
            return;
        }

        if (m_RuleList != null)
        {
            m_SerializedRules.Update();
            // Begin Scroll View
            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);
            // Draw the ReorderableList
            m_RuleList.DoLayoutList();
            // End Scroll View
            EditorGUILayout.EndScrollView();
            m_SerializedRules.ApplyModifiedProperties();
        }
    }

    private void InitializeRuleList()
    {
        SerializedProperty rulesProp = m_SerializedRules.FindProperty("m_Rules");

        // Initialize foldouts based on the number of rules
        m_Foldouts = new List<bool>();
        for (int i = 0; i < rulesProp.arraySize; i++)
        {
            m_Foldouts.Add(false); // Initially collapsed
        }

        m_RuleList = new ReorderableList(m_SerializedRules, rulesProp, true, true, true, true);

        // Draw header
        m_RuleList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Rules (Highest priority at top)", EditorStyles.boldLabel);
        };

        // Draw each element
        m_RuleList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            SerializedProperty element = rulesProp.GetArrayElementAtIndex(index);

            SerializedProperty ruleNameProp = element.FindPropertyRelative("m_Name");
            SerializedProperty conditionsProp = element.FindPropertyRelative("m_Conditions");
            SerializedProperty actionProp = element.FindPropertyRelative("m_Action");

            // Ensure foldouts list is in sync
            if (m_Foldouts.Count <= index)
                m_Foldouts.Add(false);

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 2f;

            Rect nameRect = new Rect(rect.x + 20, rect.y + spacing, 300, lineHeight);
            EditorGUI.PropertyField(nameRect, ruleNameProp, new GUIContent(""));

            // Draw the foldout
            Rect foldoutRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
            m_Foldouts[index] = EditorGUI.Foldout(foldoutRect, m_Foldouts[index], "", true);

            if (m_Foldouts[index])
            {
                EditorGUI.indentLevel++;

                float y = rect.y + EditorGUIUtility.singleLineHeight + 10;

                y += DrawComplexConditionsInline(new Rect(rect.x, y, rect.width, EditorGUIUtility.singleLineHeight), conditionsProp, ref rect);

                EditorGUILayout.Space();

                // Draw Action
                Rect actionRect = new Rect(rect.x, y, 400, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(
                    actionRect,
                    actionProp,
                    new GUIContent("Action", "Define the action to execute when conditions are met.")
                );

                EditorGUI.indentLevel--;
            }
        };

        // Calculate element height based on foldout state
        m_RuleList.elementHeightCallback = (int index) =>
        {
            SerializedProperty element = m_SerializedRules.FindProperty("m_Rules").GetArrayElementAtIndex(index);
            SerializedProperty conditionsProp = element.FindPropertyRelative("m_Conditions");

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 2f;

            float height = lineHeight + spacing; // Foldout

            if (index < m_Foldouts.Count && m_Foldouts[index])
            {
                Rect rect = new();
                // Calculate height for conditions
                height += DrawComplexConditionsInline(new Rect(0, 0, 0, 0), conditionsProp, ref rect) + spacing;

                // Add height for Action field
                height += lineHeight + spacing + 10;
            }

            return height;
        };

        // Draw background with divider lines and alternating colors
        m_RuleList.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
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

        m_RuleList.onAddCallback = (ReorderableList list) =>
        {
            int newIndex = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = newIndex;
            SerializedProperty newRule = list.serializedProperty.GetArrayElementAtIndex(newIndex);

            // Initialize new rule's name
            newRule.FindPropertyRelative("Name").stringValue = "New Rule";
            // Initialize other properties as needed

            // Add a new foldout state
            m_Foldouts.Add(false);
        };

        m_RuleList.onRemoveCallback = (ReorderableList list) =>
        {
            if (list.index >= 0 && list.index < m_Foldouts.Count)
            {
                m_Foldouts.RemoveAt(list.index);
            }
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
        };

        m_RuleList.onReorderCallback = (ReorderableList list) =>
        {
            // Reorder the foldouts list to match the new order of the rules
            List<bool> newFoldouts = new List<bool>();
            SerializedProperty rulesProp = list.serializedProperty;
            for (int i = 0; i < rulesProp.arraySize; i++)
            {
                newFoldouts.Add(m_Foldouts[i]);
            }
            m_Foldouts = newFoldouts;
        };


    }

    private float DrawComplexConditionsInline(Rect rect, SerializedProperty conditionsProp, ref Rect parentRect)
    {
        SerializedProperty conditionsList = conditionsProp.FindPropertyRelative("m_Conditions");
        SerializedProperty operatorsList = conditionsProp.FindPropertyRelative("m_Operators");

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
        SerializedProperty firstConditionNameProp = firstCondition.FindPropertyRelative("m_Name");
        SerializedProperty firstConditionNegateProp = firstCondition.FindPropertyRelative("m_Negate");

        // Toggle first
        Rect firstToggleRect = new Rect(rect.x, y, toggleWidth, lineHeight);
        firstConditionNegateProp.boolValue = EditorGUI.ToggleLeft(firstToggleRect, "Not", firstConditionNegateProp.boolValue);

        // Dropdown next
        int firstSelectedIndex = Mathf.Max(0, m_KnowledgeBaseMembers.IndexOf(firstConditionNameProp.stringValue));
        Rect firstConditionRect = new Rect(rect.x + toggleWidth + spacing, y, conditionWidth, lineHeight);
        int newFirstIndex = EditorGUI.Popup(firstConditionRect, firstSelectedIndex, m_KnowledgeBaseMembers.ToArray());
        if (newFirstIndex != firstSelectedIndex)
        {
            firstConditionNameProp.stringValue = m_KnowledgeBaseMembers[newFirstIndex];
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
            SerializedProperty conditionNameProp = condition.FindPropertyRelative("m_Name");
            SerializedProperty conditionNegateProp = condition.FindPropertyRelative("m_Negate");

            // Toggle
            Rect toggleRect = new Rect(x, y, toggleWidth, lineHeight);
            conditionNegateProp.boolValue = EditorGUI.ToggleLeft(toggleRect, "Not", conditionNegateProp.boolValue);

            // Popup
            Rect conditionRect = new Rect(x + toggleWidth + spacing, y, conditionWidth, lineHeight);
            int selectedIndex = Mathf.Max(0, m_KnowledgeBaseMembers.IndexOf(conditionNameProp.stringValue));
            int newIndex = EditorGUI.Popup(conditionRect, selectedIndex, m_KnowledgeBaseMembers.ToArray());
            if (newIndex != selectedIndex)
            {
                conditionNameProp.stringValue = m_KnowledgeBaseMembers[newIndex];
            }

            x += toggleWidth + spacing + conditionWidth + spacing;
        }

        y += lineHeight + spacing;

        // Add/Remove Buttons
        float buttonY = y;
        Rect addButtonRect = new Rect(rect.x + 10, buttonY, 30, lineHeight);
        if (GUI.Button(addButtonRect, "+"))
        {
            conditionsList.arraySize++;
            operatorsList.arraySize = conditionsList.arraySize - 1;
        }

        if (conditionsList.arraySize > 1)
        {
            Rect removeButtonRect = new Rect(rect.x + 40, buttonY, 30, lineHeight);
            if (GUI.Button(removeButtonRect, "-"))
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
