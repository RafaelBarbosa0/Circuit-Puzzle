using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircuitPuzzle
{
    [CreateAssetMenu(fileName = "PieceAssets")]
    public class PieceAssetReferencesSO : ScriptableObject
    {
        #region FIELDS
        // Styles.
        [SerializeField]
        private GUIStyle inactiveButton;
        [SerializeField]
        private GUIStyle activeButton;

        // Prefabs.
        [SerializeField]
        private GameObject blankPiece;
        [SerializeField]
        private GameObject straightPiece;
        [SerializeField]
        private GameObject tPiece;
        [SerializeField]
        private GameObject cornerPiece;
        [SerializeField]
        private GameObject startPiece;
        [SerializeField]
        private GameObject endPiece;
        #endregion

        #region PROPERTIES
        public GUIStyle InactiveButton { get => inactiveButton; private set => inactiveButton = value; }
        public GUIStyle ActiveButton { get => activeButton; private set => activeButton = value; }
        public GameObject BlankPiece { get => blankPiece; private set => blankPiece = value; }
        public GameObject StraightPiece { get => straightPiece; private set => straightPiece = value; }
        public GameObject TPiece { get => tPiece; private set => tPiece = value; }
        public GameObject CornerPiece { get => cornerPiece; private set => cornerPiece = value; }
        public GameObject StartPiece { get => startPiece; private set => startPiece = value; }
        public GameObject EndPiece { get => endPiece; private set => endPiece = value; }
        #endregion
    }
}