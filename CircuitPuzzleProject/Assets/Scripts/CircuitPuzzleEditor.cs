using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CircuitPuzzleEditor : MonoBehaviour
{
  [Header("CUSTOMIZE HERE")]
  [SerializeField]
  private int _columns;
  [SerializeField]
  private int _rows;

  [Header("DON'T TOUCH THESE")]
  [SerializeField]
  private GameObject _blankPiecePrefab;
  [SerializeField]
  private GameObject _boardPrefab;

  private int _lastCheckedColumns;
  private int _lastCheckedRows;

  private void Awake()
  {
    _lastCheckedColumns = _columns;
    _lastCheckedRows = _rows;
  }

  private void Update()
  {
    UpdateBoardSize();
  }

  /// <summary>
  /// Creates the puzzle board in the editor according to row and column input.
  /// </summary>
  private void UpdateBoardSize()
  {
    if (_columns != _lastCheckedColumns || _rows != _lastCheckedRows)
    {
      _lastCheckedColumns = _columns;
      _lastCheckedRows = _rows;

      if(transform.childCount > 0)
      {
        GameObject old = transform.GetChild(0).gameObject;
        DestroyImmediate(old);
      }

      GameObject current = Instantiate(_boardPrefab, transform);
      current.transform.SetAsFirstSibling();

      float increment = _blankPiecePrefab.transform.localScale.x;
      float startingX = -increment * (_lastCheckedColumns / 2);
      float startingY = increment * (_lastCheckedRows / 2);

      float x = startingX;
      float y = startingY;
      int nameX = 0;
      int nameY = 0;
      for (int i = 0; i < _lastCheckedRows; i++)
      {
        x = startingX;
        nameX = 0;

        for (int j = 0; j < _lastCheckedColumns; j++)
        {
          GameObject piece = Instantiate(_blankPiecePrefab, current.transform);
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
}
