using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CircuitPuzzle
{
    public class PuzzleManager : MonoBehaviour
    {
        #region FIELDS
        // Matrix containing the individual pieces for this puzzle
        private GameObject[,] puzzlePieces;

        // Reference to the currently active puzzle instance.
        private static PuzzleManager activeInstance;

        // Vector2 which holds the index of the currently selected puzzle piece.
        private Vector2 selectedPieceIndex;

        // Reference to materials used on piece border's to indicate current selection.
        [SerializeField]
        private Material defaultBorder;
        [SerializeField]
        private Material selectedBorder;

        // Reference to the GameObjects of the various types of pieces in this puzzle.
        private List<GameObject> startingPieces;
        private List<GameObject> connectorPieces;
        private List<GameObject> endingPieces;

        // Reference to the PowerManager component of the various types of pieces in this puzzle.
        private List<PowerManager> startingPieceManagers;
        private List<PowerManager> connectorPieceManagers;
        private List<PowerManager> endingPieceManagers;

        // Reference to this puzzle's puzzle settings.
        private PuzzleSettings puzzleSettings;
        #endregion

        #region PROPERTIES
        public static PuzzleManager ActiveInstance { get => activeInstance; private set => activeInstance = value; }
        #endregion

        #region UNITY METHODS
        private void Awake()
        {
            // Get the puzzle piece matrix from the puzzle creator.
            puzzlePieces = GetComponent<PuzzleCreator>().PuzzlePieces;

            // Initialize lists.
            startingPieces = new List<GameObject>();
            connectorPieces = new List<GameObject>();
            endingPieces = new List<GameObject>();

            startingPieceManagers = new List<PowerManager>();
            connectorPieceManagers = new List<PowerManager>();
            endingPieceManagers = new List<PowerManager>();

            // Get reference to PowerManager component of pieces as well as their GameObjects.
            for(int i = 0; i < puzzlePieces.GetLength(0); i++)
            {
                for(int j = 0; j < puzzlePieces.GetLength(1); j++)
                {
                    // Starting pieces.
                    if (puzzlePieces[i,j].tag == "StartingCircuitPiece")
                    {
                        // GameObject.
                        startingPieces.Add(puzzlePieces[i,j]);

                        // PowerManager.
                        startingPieceManagers.Add(puzzlePieces[i, j].GetComponent<PowerManager>());
                    }

                    // Connector pieces.
                    else if (puzzlePieces[i,j].tag == "ConnectorCircuitPiece")
                    {
                        // GameObject.
                        connectorPieces.Add(puzzlePieces[i, j]);

                        // PowerManager.
                        connectorPieceManagers.Add(puzzlePieces[i,j].GetComponent<PowerManager>());
                    }

                    // Ending pieces.
                    else if (puzzlePieces[i,j].tag == "EndingCircuitPiece")
                    {
                        // GameObject.
                        endingPieces.Add(puzzlePieces[i,j]);

                        // PowerManager.
                        endingPieceManagers.Add(puzzlePieces[i, j].GetComponent<PowerManager>());
                    }
                }
            }

            // Get PuzzleSettings reference.
            puzzleSettings = GetComponent<PuzzleSettings>();

            // Initial power check.
            StartCoroutine(IStartupPowerCheck());
        }
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Method that runs when player begins interaction with puzzle instance.
        /// </summary>
        public void StartPuzzle()
        {
            if(ActiveInstance == null)
            {
                // Reset the selected piece index.
                selectedPieceIndex = new Vector2(puzzlePieces.GetLength(0) - 1, 0);

                // Set the indicator for the selected piece (starts at leftmost top corner).
                SetSelectionIndicator((int)selectedPieceIndex.x, (int)selectedPieceIndex.y);

                // Initial power check.
                CheckPowerStatus();

                // Set this instance as the currently active one.
                ActiveInstance = this;
            }
        }

        /// <summary>
        /// Method used to move the piece selection horizontally.
        /// Use -1 as parameter to move left and 1 to move right.
        /// </summary>
        /// <param name="direction"></param>
        public void MoveSelectionHorizontal(int direction)
        {
            // If method is not called using active instance give error message and return.
            if(ActiveInstance == null || ActiveInstance != this)
            {
                Debug.LogError("Circuit puzzle has no active instance or you are trying to call it's methods directly.");
                Debug.LogError("Please use BeginInteraction method to set a puzzle's instance as active and then call it's methods through the ActiveInstance property.");
                return;
            }

            // If the received direction parameter is not valid give error message and return.
            if(direction != -1 && direction != 1)
            {
                Debug.LogError("Invalid direction value used for selection movement, please only use -1 or 1 values");
                return;
            }

            // If the received direction parameter is valid and the desired movement doesn't exceed the matrix's length.
            if (selectedPieceIndex.y + direction >= 0 && selectedPieceIndex.y + direction < puzzlePieces.GetLength(1))
            {
                // Change the selected piece index according to received direction.
                selectedPieceIndex = new Vector2(selectedPieceIndex.x, selectedPieceIndex.y + direction);

                // Set the indicator for the newly selected piece.
                SetSelectionIndicator((int)selectedPieceIndex.x, (int)selectedPieceIndex.y);
            }
        }

        /// <summary>
        /// Method used to move the piece selection vertically.
        /// Use -1 as parameter to move down and 1 to move up.
        /// </summary>
        /// <param name="direction"></param>
        public void MoveSelectionVertical(int direction)
        {
            // If method is not called using active instance give error message and return.
            if (ActiveInstance == null || ActiveInstance != this)
            {
                Debug.LogError("Circuit puzzle has no active instance or you are trying to call it's methods directly.");
                Debug.LogError("Please use BeginInteraction method to set a puzzle's instance as active and then call it's methods through the ActiveInstance property.");
                return;
            }

            // If the received direction parameter is not valid, give error message and return.
            if (direction != -1 && direction != 1)
            {
                Debug.LogError("Invalid direction value used for selection movement, please only use -1 or 1 values");
                return;
            }

            // If the received direction parameter is valid and the desired movement doesn't exceed the matrix's length.
            if (selectedPieceIndex.x + direction >= 0 && selectedPieceIndex.x + direction < puzzlePieces.GetLength(0))
            {
                // Change the selected piece index according to received direction.
                selectedPieceIndex = new Vector2(selectedPieceIndex.x + direction, selectedPieceIndex.y);

                // Set the indicator for the newly selected piece.
                SetSelectionIndicator((int)selectedPieceIndex.x, (int)selectedPieceIndex.y);
            }
        }

        /// <summary>
        /// Method to rotate the currently selected piece 90 degrees clockwise.
        /// </summary>
        public void RotatePieceRight()
        {
            // If method is not called using active instance give error message and return.
            if (ActiveInstance == null || ActiveInstance != this)
            {
                Debug.LogError("Circuit puzzle has no active instance or you are trying to call it's methods directly.");
                Debug.LogError("Please use BeginInteraction method to set a puzzle's instance as active and then call it's methods through the ActiveInstance property.");
                return;
            }

            // Get selected piece.
            GameObject puzzlePiece = puzzlePieces[(int)selectedPieceIndex.x, (int)selectedPieceIndex.y];

            // If selected piece is a starting or ending piece, we check if starting and ending pieces rotation are locked in this puzzle.
            // If so, method returns and piece won't rotate.
            if((startingPieces.Contains(puzzlePiece) && puzzleSettings.LockStartingPieces) || (endingPieces.Contains(puzzlePiece) && puzzleSettings.LockEndingPieces))
            {
                return;
            }

            // Rotate piece.
            puzzlePiece.transform.Rotate(0, 0, -90);
        }

        /// <summary>
        /// Method to rotate the currently selected piece 90 degrees counter-clockwise.
        /// </summary>
        public void RotatePieceLeft()
        {
            // If method is not called using active instance give error message and return.
            if (ActiveInstance == null || ActiveInstance != this)
            {
                Debug.LogError("Circuit puzzle has no active instance or you are trying to call it's methods directly.");
                Debug.LogError("Please use BeginInteraction method to set a puzzle's instance as active and then call it's methods through the ActiveInstance property.");
                return;
            }

            // Get selected piece.
            GameObject puzzlePiece = puzzlePieces[(int)selectedPieceIndex.x, (int)selectedPieceIndex.y];

            // If selected piece is a starting or ending piece, we check if starting and ending pieces rotation are locked in this puzzle.
            // If so, method returns and piece won't rotate.
            if ((startingPieces.Contains(puzzlePiece) && puzzleSettings.LockStartingPieces) || (endingPieces.Contains(puzzlePiece) && puzzleSettings.LockEndingPieces))
            {
                return;
            }

            // Rotate piece.
            puzzlePiece.transform.Rotate(0, 0, 90);
        }

        public void EndPuzzle()
        {
            if(ActiveInstance != null)
            {
                // Disable the selection indicator.
                DisableSelectionIndicator();

                // Make the active instance variable null, since no puzzle is currently being interacted with anymore.
                ActiveInstance = null;
            }
        }

        /// <summary>
        /// This function will check the current power status of the puzzle.
        /// It will do so by turning off the power to all non starting pieces and then checking which pieces are receiving power.
        /// </summary>
        public void CheckPowerStatus()
        {
            // Reset power.
            ResetPower();

            // Next, run power check on starting pieces.
            foreach (PowerManager manager in startingPieceManagers)
            {
                manager.CheckConnections();
            }
        }

        /// <summary>
        /// Calls IPowerOffEndingPieces coroutine.
        /// </summary>
        public void PowerOffEndingPieces()
        {
            StartCoroutine(IPowerOffEndingPieces());
        }
        #endregion

        #region COROUTINES
        /// <summary>
        /// Coroutine for initial power check.
        /// For some reason I can't figure out, if power is checked at startup it causes issues.
        /// This probably isn't the best solution but it's easy enough and I don't give a fuck.
        /// </summary>
        /// <returns></returns>
        private IEnumerator IStartupPowerCheck()
        {
            // Wait before checking power to avoid errors.
            yield return new WaitForSeconds(0.01f);

            // Check power.
            CheckPowerStatus();
        }

        private IEnumerator IPowerOffEndingPieces()
        {
            // Wait before running to make sure all pieces have finished checking their connections.
            yield return new WaitForSeconds(0.05f);

            // Power off events for ending pieces.
            foreach(PowerManager manager in endingPieceManagers)
            {
                manager.PowerOffEndingPiece();
            }
        }
        #endregion

        #region PRIVATE METHODS
        /// <summary>
        /// Changes the material of the border of the selected piece to indicate that it is selected.
        /// Changes the material of the border of every other piece back to the default material.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        private void SetSelectionIndicator(int column, int row)
        {
            // Loop through matrix.
            for (int i = 0; i < puzzlePieces.GetLength(0); i++)
            {
                for (int j = 0; j < puzzlePieces.GetLength(1); j++)
                {
                    // Get reference for this piece's border's MeshRenderer component.
                    Transform model = puzzlePieces[i, j].transform.GetChild(0);
                    Transform border = model.GetChild(1);
                    MeshRenderer renderer = border.GetComponent<MeshRenderer>();

                    // If this piece is the selected piece.
                    if (i == column && j == row)
                    {
                        // Set the border's material to the selected border material.
                        renderer.material = selectedBorder;
                    }

                    // If this piece is not the selected piece.
                    else
                    {
                        // Set the border's material to the default border material.
                        renderer.material = defaultBorder;
                    }
                }
            }
        }

        /// <summary>
        /// Turns off the selected border material on every puzzle piece.
        /// </summary>
        private void DisableSelectionIndicator()
        {
            // Loop through matrix.
            for(int i = 0; i < puzzlePieces.GetLength(0); i++)
            {
                for(int j = 0;j < puzzlePieces.GetLength(1); j++)
                {
                    // Get reference for this piece's border's MeshRenderer component.
                    Transform model = puzzlePieces[i, j].transform.GetChild(0);
                    Transform border = model.GetChild(1);
                    MeshRenderer renderer = border.GetComponent<MeshRenderer>();

                    // Set this piece's border's material to defaul border material.
                    renderer.material = defaultBorder;
                }
            }
        }

        /// <summary>
        /// Turn off power to all puzzle pieces.
        /// </summary>
        private void ResetPower()
        {
            // First turn off the power to all non starting pieces.
            // Connector pieces.
            foreach (PowerManager manager in connectorPieceManagers)
            {
                manager.IsPowered = false;
                manager.ResetWireColor();
            }

            // Ending pieces.
            foreach (PowerManager manager in endingPieceManagers)
            {
                manager.IsPowered = false;
                manager.ResetWireColor();
            }
        }
        #endregion
    }
}