using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitSource : PowerManagement
{
  /// <summary>
  /// Since this is a source piece we identify it as so and add it to sources list.
  /// </summary>
  public override void Awake()
  {
    base.Awake();

    _sources.Add(this);

    _isPowered = true;
    _isSource = true;
  }
}
