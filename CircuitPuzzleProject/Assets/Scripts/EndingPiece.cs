using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndingPiece : PowerManagement
{
  [SerializeField]
  private UnityEvent _onPoweredOn;
  [SerializeField]
  private UnityEvent _onPoweredOff;

  /// <summary>
  /// This is an ending piece so we identify it as so and add it to ending pieces list.
  /// </summary>
  public override void Awake()
  {
    base.Awake();

    _isEnd = true;
    _endings.Add(this);
  }

  /// <summary>
  /// Colors are different for ending pieces.
  /// </summary>
  public override void UpdateColors()
  {
    foreach(MeshRenderer mesh in _meshes)
    {
      if (_isPowered)
      {
        mesh.material.color = Color.green;
      }
      else
      {
        mesh.material.color = Color.red;
      }
    }
  }

  /// <summary>
  /// Executes different events when ending piece is powered on or off.
  /// Events can be set up in the inspector.
  /// </summary>
  public void ExecuteEvents()
  {
    if (_isPowered)
    {
      _onPoweredOn.Invoke();
    }
    else
    {
      _onPoweredOff.Invoke();
    }
  }
}
