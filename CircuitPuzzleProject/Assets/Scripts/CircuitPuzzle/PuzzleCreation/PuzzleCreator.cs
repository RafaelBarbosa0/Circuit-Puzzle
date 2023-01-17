using UnityEngine;

namespace CircuitPuzzle
{
    [ExecuteInEditMode]
    public class PuzzleCreator : MonoBehaviour
    {
        #region FIELDS
        private int columns;
        private int rows;

        private GameObject blankPiecePrefab;
        private GameObject boardPrefab;

        private int lastCheckedColumns;
        private int lastCheckedRows;

        private EditorAssetReferences references;
        #endregion

        #region PROPERTIES
        public int Columns { get => columns; set => columns = value; }
        public int Rows { get => rows; set => rows = value; }
        public GameObject BlankPiecePrefab { get => blankPiecePrefab; private set => blankPiecePrefab = value; }
        public GameObject BoardPrefab { get => boardPrefab; private set => boardPrefab = value; }
        public EditorAssetReferences References { get => references; private set => references = value; }
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            // Get assetReferences object.
            references = GetComponent<EditorAssetReferences>();
        }
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Creates the puzzle board in the editor according to row and column input.
        /// </summary>
        public void UpdateBoardSize()
        {
            if (columns != lastCheckedColumns || rows != lastCheckedRows)
            {
                lastCheckedColumns = columns;
                lastCheckedRows = rows;

                if (transform.childCount > 0)
                {
                    GameObject old = transform.GetChild(0).gameObject;
                    DestroyImmediate(old);
                }

                GameObject current = Instantiate(boardPrefab, transform);
                current.transform.SetAsFirstSibling();

                float increment = blankPiecePrefab.transform.localScale.x;
                float startingX = -increment * (lastCheckedColumns / 2);
                float startingY = increment * (lastCheckedRows / 2);

                float x = startingX;
                float y = startingY;
                int nameX = 0;
                int nameY = 0;
                for (int i = 0; i < lastCheckedRows; i++)
                {
                    x = startingX;
                    nameX = 0;

                    for (int j = 0; j < lastCheckedColumns; j++)
                    {
                        GameObject piece = Instantiate(blankPiecePrefab, current.transform);
                        piece.name = nameX.ToString() + ", " + nameY.ToString();
                        piece.transform.position = new Vector2(x, y);

                        x += increment;
                        nameX++;
                    }

                    y -= increment;
                    nameY++;
                }
            }
        }
        #endregion
    }
}