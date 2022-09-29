using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[ExecuteInEditMode]
public class CircuitPieceEditor : MonoBehaviour
{
  private enum PieceType
  {
    BLANK,
    BEGINNING,
    END,
    STRAIGHT,
    CORNER,
    TSHAPE
  }

  [Header("CUSTOMIZE HERE")]
  [SerializeField]
  private PieceType _pieceType;
  [SerializeField]
  [Range(0, 3)]
  private int _rotation;

  [Header("DON'T TOUCH THESE")]
  [SerializeField]
  private GameObject[] _piecePrefabs;

  private PieceType _starterType;
  private int _lastCheckedRotation;

  private void Awake()
  {
    _starterType = _pieceType;
    _lastCheckedRotation = 0;
  }

  private void Update()
  {
    SwitchPieceType();
    RotatePiece();
  }

  /// <summary>
  /// Switch the type of a piece of the board.
  /// Do so on the inspector by selecting the enum corresponding to the wanted type.
  /// </summary>
  private void SwitchPieceType()
  {
    if(_pieceType != _starterType)
    {
      GameObject original = gameObject;
      Transform parent = original.transform.parent;

      int index = (int)_pieceType;

      GameObject piece = Instantiate(_piecePrefabs[index], parent);
      piece.name = original.name;
      piece.transform.localPosition = original.transform.localPosition;
      piece.transform.SetSiblingIndex(original.transform.GetSiblingIndex());

      DestroyImmediate(original);
    }
  }
  
  /// <summary>
  /// Rotates a piece on the board.
  /// Do so by changing a slider with 4 values, one for each orientation.
  /// </summary>
  private void RotatePiece()
  {
    if(_rotation != _lastCheckedRotation)
    {
      _lastCheckedRotation = _rotation;

      Vector3 eulerRotation = new Vector3(0, 0, -(90 * _rotation));
      gameObject.transform.localRotation = Quaternion.Euler(eulerRotation);
    }
  }
}
