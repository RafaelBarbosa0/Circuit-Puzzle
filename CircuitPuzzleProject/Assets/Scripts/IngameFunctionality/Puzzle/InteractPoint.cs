using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CircuitPuzzle
{
    public class InteractPoint : MonoBehaviour
    {
        #region FIELDS
        // Reference to the puzzle manager component in parent transform.
        private PuzzleManager puzzleManager;
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            // Get puzzle manager reference.
            puzzleManager = transform.parent.GetComponent<PuzzleManager>();
        }
        #endregion

        #region EVENTS
        // Events that will be called when the player starts and stops interacting with the interact point.
        [SerializeField]
        private UnityEvent OnBeginInteraction;
        [SerializeField]
        private UnityEvent OnEndInteraction;
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Calls the puzzle manager's StartPuzzle method, followed by the OnBeginInteraction event.
        /// Allows user to customize what happens when puzzle interaction begins.
        /// </summary>
        public void BeginInteraction()
        {
            if(PuzzleManager.ActiveInstance == null)
            {
                // Start puzzle.
                puzzleManager.StartPuzzle();

                // Invoke event.
                OnBeginInteraction.Invoke();
            }
        }

        /// <summary>
        /// Calls the puzzle manager's EndPuzzle method, followed by OnEndInteraction event.
        /// Allows user to customize what happens when puzzle interaction ends.
        /// </summary>
        public void EndInteraction()
        {
            if(PuzzleManager.ActiveInstance != null)
            {
                // End puzzle.
                PuzzleManager.ActiveInstance.EndPuzzle();

                //Invoke event.
                OnEndInteraction.Invoke();
            }
        }
        #endregion
    }
}