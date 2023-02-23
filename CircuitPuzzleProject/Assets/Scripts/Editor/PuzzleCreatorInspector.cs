using Unity.VisualScripting;
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
        private GUIStyle fieldChangesStyle;
        private GUIStyle buttonGreenStyle;
        private GUIStyle buttonRedStyle;

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
            headerStyle = creator.References.PuzzleCreatorAssets.HeaderStyle;
            labelStyle = creator.References.PuzzleCreatorAssets.LabelStyle;
            fieldStyle = creator.References.PuzzleCreatorAssets.FieldStyle;
            buttonStyle= creator.References.PuzzleCreatorAssets.ButtonStyle;
            fieldChangesStyle = creator.References.PuzzleCreatorAssets.FieldChangesStyle;
            buttonGreenStyle = creator.References.PuzzleCreatorAssets.ButtonGreenStyle;
            buttonRedStyle = creator.References.PuzzleCreatorAssets.ButtonRedStyle;

            // Set textures for UI elements.
            leftArrowTexture = creator.References.PuzzleCreatorAssets.LeftArrow;
            rightArrowTexture = creator.References.PuzzleCreatorAssets.RightArrow;
            #endregion

            #region LAYOUT
            // Board size settings header.
            EditorGUILayout.LabelField("Board Size", headerStyle);

            // Spacing //
            GUILayout.Space(contentSpacing);

            // ROWS SECTION.
            // Field.
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Rows", labelStyle);
                // If changes to the number of rows were made.
                if(creator.SelectedRows != creator.SetRows)
                {
                    creator.SelectedRows = EditorGUILayout.IntField(creator.SelectedRows, fieldChangesStyle);
                }
                 //If no changes were made.
                else
                {
                    creator.SelectedRows = EditorGUILayout.IntField(creator.SelectedRows, fieldStyle);
                }
            EditorGUILayout.EndHorizontal();

            // Spacing //
            GUILayout.Space(groupSpacing);

            // Arrow Buttons.
            EditorGUILayout.BeginHorizontal();
                // Left Arrow.
                if (GUILayout.Button(leftArrowTexture))
                {
                    creator.SelectedRows--;
                }

                // Right Arrow.
                if (GUILayout.Button(rightArrowTexture))
                {
                    creator.SelectedRows++;
                }
            EditorGUILayout.EndHorizontal();

            // Spacing //
            GUILayout.Space(contentSpacing);

            // COLUMNS SECTION.
            // Field.
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Columns", labelStyle);
            // If changes to the number of columns have been made.
            if (creator.SetColumns != creator.SelectedColumns)
            {
                creator.SelectedColumns = EditorGUILayout.IntField(creator.SelectedColumns, fieldChangesStyle);
            }

            // If no changes were made.
            else
            {
                creator.SelectedColumns = EditorGUILayout.IntField(creator.SelectedColumns, fieldStyle);
            }
            EditorGUILayout.EndHorizontal();

            // Spacing //
            GUILayout.Space(groupSpacing);

            // Arrow Buttons.
            EditorGUILayout.BeginHorizontal();
            // Left Arrow.
            if (GUILayout.Button(leftArrowTexture))
            {
                creator.SelectedColumns--;
            }

            // Right Arrow.
            if (GUILayout.Button(rightArrowTexture))
            {
                creator.SelectedColumns++;
            }
            EditorGUILayout.EndHorizontal();

            // Spacing //
            GUILayout.Space(contentSpacing);

            // LIMITER SECTION.
            // Set text on toggle to reflect current limiter state.
            string limiterState;
            if (creator.IsLimited)
            {
                limiterState = "Limiter enabled";
            }

            else
            {
                limiterState = "Limiter disabled";
            }

            // Toggle.
            creator.IsLimited = EditorGUILayout.Toggle(new GUIContent(limiterState, "Turn the limit on row and column input on or off, use at own risk"), creator.IsLimited);

            // Spacing //
            GUILayout.Space(contentSpacing);

            // APPLY AND CANCEL SECTION.
            // Change styles for the buttons depending on whether changes were made to row or column count.
            GUIStyle currentApply = new GUIStyle();
            GUIStyle currentCancel = new GUIStyle();
            // If changes were made.
            if(creator.SelectedColumns != creator.SetColumns || creator.SelectedRows != creator.SetRows)
            {
                currentApply = buttonGreenStyle;
                currentCancel = buttonRedStyle;
            }

            // If changes were not made.
            else
            {
                currentApply = buttonStyle;
                currentCancel = buttonStyle;
            }
            EditorGUILayout.BeginVertical();
                // Apply button.
                if (GUILayout.Button("Apply", currentApply))
                {
                    creator.ApplyChanges();
                }

                // Spacing //
                GUILayout.Space(groupSpacing);

                // Cancel button.
                if (GUILayout.Button("Cancel", currentCancel))
                {
                    creator.CancelChanges();
                }
            EditorGUILayout.EndVertical();

            // Spacing //
            GUILayout.Space(contentSpacing);

            // Clear board button.
            if(GUILayout.Button("Clear Board", buttonStyle))
            {
                // Create popup to confirm whether user wants to clear the board.
                bool clearOutput = EditorUtility.DisplayDialog("Clear Board", "Are you sure you wanna clear the current board?", "Yes", "No");

                // If user clicked yes, clear the board.
                if(clearOutput)
                {
                    creator.ClearBoard();
                }
            }

            // Repaint so button hover states are reflected in real time.
            Repaint();
        }
        #endregion
    }
    #endregion
}