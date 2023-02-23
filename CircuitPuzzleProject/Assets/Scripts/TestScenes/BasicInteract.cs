using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    public class BasicInteract : MonoBehaviour
    {
        #region FIELDS
        // Reference to the interaction point.
        [SerializeField]
        private InteractPoint interactPoint;
        #endregion

        #region UNITY METHODS
        private void Update()
        {
            // Begin interaction.
            if (Input.GetKeyDown(KeyCode.F))
            {
                interactPoint.BeginInteraction();
            }

            // Rotate left.
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if(PuzzleManager.ActiveInstance != null)
                {
                    PuzzleManager.ActiveInstance.RotatePieceLeft();
                }
            }

            // Rotate right.
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(PuzzleManager.ActiveInstance != null)
                {
                    PuzzleManager.ActiveInstance.RotatePieceRight();
                }
            }

            // Move up.
            if (Input.GetKeyDown(KeyCode.W))
            {
                if(PuzzleManager.ActiveInstance != null)
                {
                    PuzzleManager.ActiveInstance.MoveSelectionVertical(1);
                }
            }

            // Move down.
            if (Input.GetKeyDown(KeyCode.S))
            {
                if(PuzzleManager.ActiveInstance != null)
                {
                    PuzzleManager.ActiveInstance.MoveSelectionVertical(-1);
                }
            }

            // Move left.
            if (Input.GetKeyDown(KeyCode.A))
            {
                if(PuzzleManager.ActiveInstance != null)
                {
                    PuzzleManager.ActiveInstance.MoveSelectionHorizontal(-1);
                }
            }

            // Move right.
            if (Input.GetKeyDown(KeyCode.D))
            {
                if(PuzzleManager.ActiveInstance != null)
                {
                    PuzzleManager.ActiveInstance.MoveSelectionHorizontal(1);
                }
            }

            // End interaction.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                interactPoint.EndInteraction();
            }
        }
        #endregion
    }
}