using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CircuitPuzzle
{
    [ExecuteInEditMode]
    public class PieceSwitcher : MonoBehaviour
    {
        #region FIELDS
        // Reference holder.
        private SOAssetHolder references;

        // Index for this puzzle piece in the puzzle matrix.
        [SerializeField, HideInInspector]
        private int row;
        [SerializeField, HideInInspector]
        private int column;

        // Index to know which piece type this is, used for active button selection.
        [SerializeField, HideInInspector]
        private int typeIndex;
        #endregion

        #region PROPERTIES
        public SOAssetHolder References { get => references; private set => references = value; }
        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }
        public int TypeIndex { get => typeIndex; set => typeIndex = value; }
        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            // Get asset references.
            references = GetComponent<SOAssetHolder>();
        }
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        ///  Switches this pieces type for another while keeping all its settings intact.
        /// </summary>
        /// <param name="piece"></param>
        public void SwitchPiece(GameObject piece, int typeIndex)
        {
            // Get reference to the puzzle matrix this piece is contained in.
            GameObject[,] matrix = transform.parent.parent.GetComponent<PuzzleCreator>().PuzzlePieces;

            // Instantiate the new piece.
            GameObject newPiece = Instantiate(piece, transform.parent.transform);

            // Replace this piece from the puzzle matrix with the new piece.
            matrix[row, column] = newPiece;

            // Transfer this piece's information and components to the new piece.
            // Basic info.
            newPiece.name = name;
            newPiece.transform.localPosition = transform.localPosition;
            newPiece.transform.SetSiblingIndex(transform.GetSiblingIndex());

            // Switcher
            PieceSwitcher newSwitcher = newPiece.GetComponent<PieceSwitcher>();
            newSwitcher.Row = row;
            newSwitcher.Column = column;

            // Set scene as dirty so changes can be saved.
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            // Destroy this piece.
            DestroyImmediate(gameObject);
        }
        #endregion
    }
}
