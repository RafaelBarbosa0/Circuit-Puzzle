using UnityEditor;
using UnityEngine;

namespace CircuitPuzzle
{
    [CustomEditor(typeof(PuzzleCreator))]
    public class PuzzleCreatorInspector : Editor
    {
        #region FIELDS
        // Spacing.
        private int contentSpacing = 20;
        private int groupSpacing = 5;

        // Styles.
        private GUIStyle headerStyle;
        private GUIStyle labelStyle;
        private GUIStyle fieldStyle;
        private GUIStyle buttonStyle;

        // Textures.
        private Texture2D leftArrowTexture;
        private Texture2D rightArrowTexture;
        #endregion

        #region GUI
        public override void OnInspectorGUI()
        {
            #region SETUP
            // Get selected PuzzleCreator script.
            // Returns if casting fails.
            PuzzleCreator creator = target as PuzzleCreator;
            if (creator == null)
            {
                return;
            }

            // Set styles for UI elements.
            headerStyle = creator.References.HeaderStyle;
            labelStyle = creator.References.LabelStyle;
            fieldStyle = creator.References.FieldStyle;
            buttonStyle= creator.References.ButtonStyle;

            // Set textures for UI elements.
            leftArrowTexture = creator.References.LeftArrow;
            rightArrowTexture = creator.References.RightArrow;
            #endregion

            #region LAYOUT
            // Board size settings header.
            EditorGUILayout.LabelField("Board Size", headerStyle);

            // Spacing //
            GUILayout.Space(contentSpacing);

            // COLUMNS SECTION.
            // Field.
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Columns", labelStyle);
                creator.Columns = EditorGUILayout.IntField(creator.Columns, fieldStyle);
            EditorGUILayout.EndHorizontal();

            // Spacing //
            GUILayout.Space(groupSpacing);

            // Arrow Buttons.
            EditorGUILayout.BeginHorizontal();
                // Left Arrow.
                if (GUILayout.Button(leftArrowTexture))
                {
                    Debug.Log("Left");
                }

                // Right Arrow.
                if (GUILayout.Button(rightArrowTexture))
                {
                    Debug.Log("Right");
                }
            EditorGUILayout.EndHorizontal();

            //Spacing //
            GUILayout.Space(contentSpacing);

            // ROWS SECTION.
            // Field.
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Rows", labelStyle);
                creator.Columns = EditorGUILayout.IntField(creator.Columns, fieldStyle);
            EditorGUILayout.EndHorizontal();

            // Spacing //
            GUILayout.Space(groupSpacing);

            // Arrow Buttons.
            EditorGUILayout.BeginHorizontal();
                // Left Arrow.
                if (GUILayout.Button(leftArrowTexture))
                {
                    Debug.Log("Left");
                }

                // Right Arrow.
                if (GUILayout.Button(rightArrowTexture))
                {
                    Debug.Log("Right");
                }
            EditorGUILayout.EndHorizontal();

            // Spacing //
            GUILayout.Space(contentSpacing);

            // APPLY AND CANCEL SECTION.
            EditorGUILayout.BeginVertical();
            // Apply button.
                if (GUILayout.Button("Apply", buttonStyle))
                {
                    Debug.Log("Apply");
                }

                // Spacing //
                GUILayout.Space(groupSpacing);

                // Cancel button.
                if (GUILayout.Button("Cancel", buttonStyle))
                {
                    Debug.Log("Cancel");
                }
            EditorGUILayout.EndVertical();
        }
        #endregion
    }
    #endregion
}