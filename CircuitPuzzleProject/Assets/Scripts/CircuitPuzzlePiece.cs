using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitPuzzlePiece : PowerManagement
{
  public override void Update()
  {
    base.Update();
    ClickPiece();
  }

  /// <summary>
  /// Behavior for clicking on a puzzle piece.
  /// First, the piece is rotated.
  /// Then the power for every piece is resetted, followed by a check of which pieces are receiving power.
  /// Once we know which pieces are powered, necessary updates are made.
  /// </summary>
  private void ClickPiece()
  {
    Ray ray;
    RaycastHit hit;

    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if(Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0))
    {
      if(hit.collider.tag == "Circuit Piece")
      {
        if(hit.collider.gameObject == gameObject)
        {
          RotatePiece();
        }
      }
    }

    ResetPower();
    foreach(PowerManagement source in _sources)
    {
      source.CheckPower();
    }
  }

  /// <summary>
  /// Rotates a puzzle piece.
  /// </summary>
  private void RotatePiece()
  {
    if(gameObject.transform.localRotation.z == -270)
    {
      gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
    else
    {
      gameObject.transform.Rotate(0, 0, -90);
    }
  }
}
