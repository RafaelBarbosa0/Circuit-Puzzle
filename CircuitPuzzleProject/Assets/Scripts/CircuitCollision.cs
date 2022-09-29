using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class manages a list of collisions and sends it to Power Management script.
// This way, in each circuit, we know what other circuits it is connected to.
public class CircuitCollision : MonoBehaviour
{
  private PowerManagement _powerManagement;

  private void Start()
  {
    _powerManagement = transform.parent.gameObject.GetComponent<PowerManagement>();
  }

  private void OnTriggerEnter(Collider other)
  {
    if(other.tag == "Circuit Connection")
    {
      GameObject go = other.transform.parent.gameObject;

      bool add = true;
      foreach(GameObject obj in _powerManagement.CollidingObjects)
      {
        if(go == obj)
        {
          add = false;
          break;
        }
      }

      if (add)
      {
        _powerManagement.CollidingObjects.Add(go);
      }
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if(other.tag == "Circuit Connection")
    {
      GameObject go = other.transform.parent.gameObject;

      bool remove = false;
      foreach(GameObject obj in _powerManagement.CollidingObjects)
      {
        if(go == obj)
        {
          remove = true;
          break;
        }
      }

      if (remove)
      {
        _powerManagement.CollidingObjects.Remove(go);
      }
    }
  }
}
