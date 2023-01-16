using UnityEditor;
using UnityEngine;

namespace CircuitPuzzle
{
    [CustomEditor(typeof(PuzzleCreator))]
    public class PuzzleCreatorInspector : Editor
    {
        private int contentSpacing = 10;
        private int groupSpacing = 3;

        private int headerFontSize = 20;

        private int labelFontSize = 30;
        private int labelSpacing = 3;

        public override void OnInspectorGUI()
        {
            #region INITIALIZE
            // Get selected PuzzleCreator script.
            // Returns if casting fails.
            PuzzleCreator creator = target as PuzzleCreator;
            if (creator == null)
            {
                return;
            }
            #endregion

            #region STYLES
            // Create styles for labels.
            // Header Style.
            GUIStyle headerStyle = new GUIStyle();
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.alignment = TextAnchor.MiddleCenter;
            headerStyle.fontSize = headerFontSize;
            headerStyle.normal.textColor = Color.white;
            // Label Style.
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontStyle = FontStyle.Normal;
            labelStyle.alignment = TextAnchor.MiddleCenter - labelSpacing;
            labelStyle.fontSize = labelFontSize;
            labelStyle.normal.textColor = Color.white;
            // Field Style.
            GUIStyle fieldStyle = new GUIStyle();
            fieldStyle.fontStyle = FontStyle.Normal;
            fieldStyle.alignment = TextAnchor.MiddleCenter + labelSpacing;
            fieldStyle.fontSize = labelFontSize;
            fieldStyle.normal.textColor = Color.white;
            #endregion

            #region BOARD SIZE SETTINGS
            // Board size settings header.
            EditorGUILayout.LabelField("Board Size", headerStyle);

            // Spacing.
            GUILayout.Space(contentSpacing);

            // Columns section.
            // Field.
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Columns", labelStyle);
                creator.Columns = EditorGUILayout.IntField(creator.Columns, fieldStyle);
            EditorGUILayout.EndHorizontal();

            // Spacing.
            GUILayout.Space(groupSpacing);

            // Buttons.
            EditorGUILayout.BeginHorizontal();
                // Left button.
                if (GUILayout.Button("Left"))
                {
                    Debug.Log("Left");
                }
                // Right Button.
                if (GUILayout.Button("Right"))
                {
                    Debug.Log("Right");
                }
            EditorGUILayout.EndHorizontal();

            //Spacing.
            GUILayout.Space(contentSpacing);

            // Rows section.
            // Field.
            creator.Rows = EditorGUILayout.IntField("Rows", creator.Rows, labelStyle);
            // Buttons.
            EditorGUILayout.BeginHorizontal();
                // Left button.
                if (GUILayout.Button("Left"))
                {
                    Debug.Log("Left");
                }
                // Right Button.
                if (GUILayout.Button("Right"))
                {
                    Debug.Log("Right");
                }
            EditorGUILayout.EndHorizontal();

            // Spacing.
            GUILayout.Space(contentSpacing);

            // Apply and cancel section.
            EditorGUILayout.BeginVertical();
            // Apply button.
                if (GUILayout.Button("Apply"))
                {
                    Debug.Log("Apply");
                }
                // Cancel button.
                if (GUILayout.Button("Cancel"))
                {
                    Debug.Log("Cancel");
                }
            EditorGUILayout.EndVertical();
            #endregion
        }
    }
}