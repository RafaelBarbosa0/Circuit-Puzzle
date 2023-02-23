using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CircuitPuzzle
{
    [ExecuteInEditMode]
    public class PuzzleCreator : MonoBehaviour, ISerializationCallbackReceiver
    {
        #region FIELDS
        // References to assets to be used for custom inspector.
        private SOAssetHolder references;

        // Reference to transform where piece prefabs will be instantiated.
        private Transform boardTransform;

        // The number of rows and columns the user currently has inputted in the inspector.
        [SerializeField, HideInInspector]
        private int selectedRows;
        [SerializeField, HideInInspector]
        private int selectedColumns;

        // The number of rows and columns that the last created puzzle iteration contains.
        [SerializeField, HideInInspector]
        private int setRows;
        [SerializeField, HideInInspector]
        private int setColumns;

        // Reference to the puzzle piece prefab.
        private GameObject blankPiece;

        // Matrix that contains the gameobject prefab of each individual puzzle piece.
        private GameObject[,] puzzlePieces;

        // List the matrix will be converted to so it can be serialized.
        [SerializeField, HideInInspector]
        private List<PuzzlePackage<GameObject>> serializablePieces;

        // Struct with info to convert the matrix to and from the list.
        [System.Serializable]
        private struct PuzzlePackage<TElement>
        {
            public int Row;
            public int Column;
            public TElement Element;

            public PuzzlePackage(int row, int column, TElement element)
            {
                Row = row;
                Column = column;
                Element = element;
            }
        }

        // Boolean that determines whether the visual representation of the puzzle is made with 3D models or 2D sprites.
        [SerializeField, HideInInspector]
        private bool is2D = true;

        // References to the MeshRenderer and SpriteRenderer rendering the puzzle piece's model and sprite respectively.
        private MeshRenderer pieceMeshRenderer;
        private SpriteRenderer pieceSpriteRenderer;

        // Boolean to limit the number of pieces allowed for rows and columns.
        private bool isLimited;
        #endregion

        #region PROPERTIES
        // Assures that the number of rows does does not go below 0 or above 100.
        public int SelectedRows
        {
            get { return selectedRows; }
            set
            {
                if (value < 1)
                {
                    selectedRows = 1;
                }
                else if (value > 20 && isLimited)
                {
                    selectedRows = 20;
                }
                else
                {
                    selectedRows = value;
                }
            }
        }
        // Same as the property for rows.
        public int SelectedColumns
        {
            get { return selectedColumns; }
            set
            {
                if (value < 1)
                {
                    selectedColumns = 1;
                }
                else if (value > 20 && isLimited)
                {
                    selectedColumns = 20;
                }
                else
                {
                    selectedColumns = value;
                }
            }
        }
        public SOAssetHolder References { get => references; private set => references = value; }
        public int SetRows { get => setRows; private set => setRows = value; }
        public int SetColumns { get => setColumns; private set => setColumns = value; }
        public bool IsLimited { get => isLimited; set => isLimited = value; }
        public GameObject[,] PuzzlePieces { get => puzzlePieces; set => puzzlePieces = value; }
        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            // Get assetReferences object.
            references = GetComponent<SOAssetHolder>();

            // Get board tranform reference.
            boardTransform = transform.GetChild(0);

            // Get piece prefab from assets.
            blankPiece = references.PuzzleCreatorAssets.BlankPiecePrefab;

            // Get MeshRenderer and SpriteRenderer references.
            pieceMeshRenderer = blankPiece.transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>();
            pieceSpriteRenderer = blankPiece.transform.GetChild(1).transform.GetChild(0).GetComponent<SpriteRenderer>();

            // Turn limiter on.
            isLimited = true;

            // If puzzle matrix has not been initialized, do so.
            if (puzzlePieces == null)
            {
                puzzlePieces = new GameObject[0, 0];
            }
        }
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Applies changes to the puzzle according to the current row and column input.
        /// </summary>
        public void ApplyChanges()
        {

            // Changes will only be applied if user changed row or column input.
            if (selectedColumns != setColumns || selectedRows != setRows)
            {
                // If no puzzle iteration exists, create a new puzzle.
                if (SetRows == 0 || setColumns == 0)
                {
                    puzzlePieces = CreatePuzzle();
                }

                // Else, modify the current iteration according to new row and column input.
                else
                {
                    // Container for previous puzzle iteration, will be used to delete removed rows or columns.
                    GameObject[,] oldPieces = puzzlePieces;

                    // Create a new puzzle iteration, keeping unchanged pieces from previous iteration.
                    puzzlePieces = CreatePuzzle(oldPieces);

                    // Delete pieces from previous iteration that were removed in new iteration.
                    DeleteRemovedPieces(oldPieces);
                }

                // Set the local positions of the puzzle pieces inside the puzzle matrix.
                SetPiecePositions(puzzlePieces);

                // Adjust setRows and setColumns value to match changes.
                setRows = selectedRows;
                setColumns = selectedColumns;

                // Mark scene as dirty so hierarchy changes can be saved.
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }

        /// <summary>
        /// Cancels the changes made to row and column inputs.
        /// </summary>
        public void CancelChanges()
        {
            // Function will only run if user made changes to row or column inputs.
            // Will also only run if a puzzle iteration already exists.
            if ((selectedColumns != setColumns || selectedRows != setRows) && setColumns != 0 && SetRows != 0)
            {
                selectedRows = setRows;
                selectedColumns = setColumns;

                // Mark scene as dirty so hierarchy changes can be saved.
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }

        /// <summary>
        /// Clears the current board, deleting all pieces.
        /// </summary>
        public void ClearBoard()
        {
            // Delete the board.
            DeleteBoard(puzzlePieces);

            // Reset the setRows and setColumns variables.
            setRows = 0;
            SetColumns = 0;

            // Mark scene as dirty so changes can be saved.
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
        #endregion

        #region SERIALIZATION
        /// <summary>
        /// Converts the puzzle matrix to a list and serializes it.
        /// </summary>
        public void OnBeforeSerialize()
        {
            serializablePieces = new List<PuzzlePackage<GameObject>>();
            for (int i = 0; i < puzzlePieces.GetLength(0); i++)
            {
                for (int j = 0; j < puzzlePieces.GetLength(1); j++)
                {
                    serializablePieces.Add(new PuzzlePackage<GameObject>(i, j, puzzlePieces[i, j]));
                }
            }
        }

        /// <summary>
        /// Converts the serialized list back into the puzzle matrix.
        /// </summary>
        public void OnAfterDeserialize()
        {
            puzzlePieces = new GameObject[setRows, setColumns];

            if (puzzlePieces.GetLength(0) > 0 && puzzlePieces.GetLength(1) > 0)
            {
                foreach (var package in serializablePieces)
                {
                    puzzlePieces[package.Row, package.Column] = package.Element;
                }
            }
        }
        #endregion

        #region PRIVATE METHODS
        /// <summary>
        /// Creates the puzzle board by populating puzzle matrix with individual piece prefabs.
        /// This creates the first iteration of this instance's puzzle, with default puzzle piece prefabs only.
        /// </summary>
        /// <returns>Matrix containing the gameobjects for the puzzle's pieces</returns>
        private GameObject[,] CreatePuzzle()
        {
            // Create container for new puzzle.
            GameObject[,] pieces = new GameObject[selectedRows, selectedColumns];

            // Loop through matrix.
            for (int i = 0; i < pieces.GetLength(0); i++)
            {
                for (int j = 0; j < pieces.GetLength(1); j++)
                {
                    // Fill matrix with blank piece prefabs.
                    pieces[i, j] = Instantiate(blankPiece, boardTransform);

                    // Feed the piece it's own position in the matrix so it can be switched later.
                    SetPieceIndex(pieces[i,j], i, j);
                }
            }

            // Return the piece matrix.
            return pieces;
        }

        /// <summary>
        /// Creates the puzzle board by populating puzzle matrix with individual piece prefabs.
        /// This overload creates any iteration of the puzzle after the first one.
        /// Either new pieces are added to already existing puzzle, or pieces are removed from it.
        /// Changes made to piece prefabs belonging to the previous puzzle iteration are not affected.
        /// </summary>
        /// <param name="oldPieces"></param>
        /// <returns>Matrix containing the gameobjects for the puzzle's pieces</returns>
        private GameObject[,] CreatePuzzle(GameObject[,] oldPieces)
        {
            // Create container for newly created puzzle.
            GameObject[,] newPieces = new GameObject[selectedRows, selectedColumns];

            // Loop through matrix.
            for (int i = 0; i < selectedRows; i++)
            {
                for (int j = 0; j < selectedColumns; j++)
                {
                    // If previous iteration contains this element.
                    if (i < oldPieces.GetLength(0) && j < oldPieces.GetLength(1))
                    {
                        // Piece remains the same.
                        newPieces[i, j] = oldPieces[i, j];
                    }

                    // If previous iteration does not contain this element.
                    else
                    {
                        // New instance is created.
                        newPieces[i, j] = Instantiate(blankPiece, boardTransform);

                        // Set piece index.
                        SetPieceIndex(newPieces[i, j], i, j);
                    }
                }
            }

            // Return the matrix with the newly created puzzle pieces.
            return newPieces;
        }

        /// <summary>
        ///  Sets the pieces index according to its position in the puzzle matrix.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        private void SetPieceIndex(GameObject piece, int row, int column)
        {
            PieceSwitcher switcher = piece.GetComponent<PieceSwitcher>();
            switcher.Row = row;
            switcher.Column = column;
        }

        /// <summary>
        /// This method will remove rows and columns that were in the previous puzzle iteration but not on the new one.
        /// </summary>
        private void DeleteRemovedPieces(GameObject[,] oldPieces)
        {
            // Loop through matrix.
            for (int i = 0; i < oldPieces.GetLength(0); i++)
            {
                for (int j = 0; j < oldPieces.GetLength(1); j++)
                {
                    // If old iteration contains this element but new one does not.
                    if (i >= puzzlePieces.GetLength(0) || j >= puzzlePieces.GetLength(1))
                    {
                        // Destroy piece GameObject.
                        DestroyImmediate(oldPieces[i, j]);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the position of each individual piece prefab on the board.
        /// Piece positions are properly aligned according to row and column input, as well as the size of its model or sprite.
        /// </summary>
        /// <param name="pieces"></param>
        private void SetPiecePositions(GameObject[,] pieces)
        {
            // Value that will be used to increment the position of each puzzle piece.
            float increment = 0;

            // If the current visual representation of the puzzle pieces are 3D models.
            if (!is2D)
            {
                // Set the increment according to the model's width.
                increment = pieceMeshRenderer.bounds.size.x;
            }

            // If the current visual representation of the puzzle pieces are 2D sprites.
            else
            {
                // Set the increment according to the sprite's width.
                increment = pieceSpriteRenderer.bounds.size.x;
            }

            // Get starting position for the X axis.
            float startingPositionX = 0;
            // If X axis elements are even.
            if (pieces.GetLength(1) % 2 == 0)
            {
                startingPositionX += increment / 2;
                startingPositionX -= increment * ((pieces.GetLength(1)) / 2);
            }
            // If X axis elements are odd.
            else
            {
                startingPositionX -= increment * ((pieces.GetLength(1) - 1) / 2);
            }

            // Get starting position for the Y axis.
            float startingPositionY = 0;
            // If Y axis elements are even.
            if (pieces.GetLength(0) % 2 == 0)
            {
                startingPositionY += increment / 2;
                startingPositionY -= increment * ((pieces.GetLength(0)) / 2);
            }
            // If Y axis elements are odd.
            else
            {
                startingPositionY -= increment * ((pieces.GetLength(0) - 1) / 2);
            }

            // Loop through matrix.
            for (int i = 0; i < selectedRows; i++)
            {
                for (int j = 0; j < selectedColumns; j++)
                {
                    // Gets the X and Y position for the current puzzle piece.
                    float positionX = startingPositionX + (increment * j);
                    float positionY = startingPositionY + (increment * i);

                    // Sets the piece's position.
                    pieces[i, j].transform.localPosition = new Vector3(positionX, positionY, 0);
                    // Sets the piece's name the same as its index in the matrix.
                    pieces[i, j].name = "[" + i + ", " + j + "]";
                }
            }
        }

        /// <summary>
        /// Deletes all the pieces from the puzzle matrix and makes it into a an empty matrix of 0 length.
        /// </summary>
        /// <param name="pieces"></param>
        private void DeleteBoard(GameObject[,] pieces)
        {
            // Loop through matrix.
            for(int i = 0; i < pieces.GetLength(0); i++)
            {
                for(int j = 0; j < pieces.GetLength(1); j++)
                {
                    // Destroy the gameobject for each piece.
                    DestroyImmediate(pieces[i, j]);
                }
            }

            // Reset the matrix.
            pieces = new GameObject[0, 0];
        }
        #endregion
    }
}