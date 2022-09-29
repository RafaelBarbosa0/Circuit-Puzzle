using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerManagement : MonoBehaviour
{
  [SerializeField]
  protected MeshRenderer[] _meshes;

  protected static List<PowerManagement> _powerManagements;
  protected static List<PowerManagement> _sources;
  protected static List<PowerManagement> _endings;
  protected List<GameObject> _collidingObjects;

  protected bool _isSource;
  protected bool _isEnd;
  protected bool _previousState;
  protected bool _isPowered;

  public List<GameObject> CollidingObjects { get => _collidingObjects; set => _collidingObjects = value; }
  public bool IsPowered { get => _isPowered; set => _isPowered = value; }

  public virtual void Awake()
  {
    if(_powerManagements == null)
    {
      _powerManagements = new List<PowerManagement>();
    }
    _powerManagements.Add(this);

    if(_sources == null)
    {
      _sources = new List<PowerManagement>();
    }

    if(_endings == null)
    {
      _endings = new List<PowerManagement>();
    }

    _collidingObjects = new List<GameObject>();
  }

  public virtual void Start()
  {

  }

  public virtual void Update()
  {
    UpdateStatus();
  }

  /// <summary>
  /// Used to check which pieces are receiving power.
  /// This overload is the starting point and is used on the power sources.
  /// Runs the other overload of this function on circuits connected to this one.
  /// </summary>
  public virtual void CheckPower()
  {
    if(_collidingObjects.Count > 0)
    {
      foreach(GameObject obj in _collidingObjects)
      {
        obj.GetComponent<PowerManagement>().CheckPower(gameObject);
      }
    }
    else
    {
      foreach (EndingPiece ending in _endings)
      {
        if (ending._previousState != ending._isPowered)
        {
          ending._previousState = ending._isPowered;

          ending.ExecuteEvents();
        }
      }
    }
  }

  /// <summary>
  /// Sets power to on, running same functions on circuits connected to this one.
  /// Receives previously checked object so it is not checked again.
  /// It every circuit has been checked events are run on ending pieces if necessary.
  /// </summary>
  /// <param name="previous"></param>
  public virtual void CheckPower(GameObject previous)
  {
    if (_isPowered) return;

    _isPowered = true;

    bool finished = true;
    if(_collidingObjects.Count > 0)
    {
      foreach(GameObject obj in _collidingObjects)
      {
        if(obj != previous)
        {
          finished = false;

          obj.GetComponent<PowerManagement>().CheckPower(gameObject);
        }
      }
    }

    if (finished)
    {
      foreach(EndingPiece ending in _endings)
      {
        if(ending._previousState != ending._isPowered)
        {
          ending._previousState = ending._isPowered;

          ending.ExecuteEvents();
        }
      }
    }
  }

  /// <summary>
  /// Resets the power on every circuit except for power sources.
  /// Sets previous state on ending pieces before disabling their power.
  /// This is so we know if there was a change to their power status and if an on powered event needs to be run.
  /// </summary>
  public virtual void ResetPower()
  {
    foreach(PowerManagement manage in _powerManagements)
    {
      if (manage._isEnd)
      {
        manage._previousState = manage._isPowered;
      }

      if (!manage._isSource)
      {
        manage._isPowered = false;
      }
    }
  }

  /// <summary>
  /// Updates necessary information.
  /// </summary>
  public virtual void UpdateStatus()
  {
    UpdateColors();
  }

  /// <summary>
  /// Changes the color of the circuits depending on whether they are powered or not.
  /// </summary>
  public virtual void UpdateColors()
  {
    foreach(MeshRenderer mesh in _meshes)
    {
      if (_isPowered)
      {
        mesh.material.color = Color.green;
      }
      else
      {
        mesh.material.color = Color.gray;
      }
    }
  }
}
