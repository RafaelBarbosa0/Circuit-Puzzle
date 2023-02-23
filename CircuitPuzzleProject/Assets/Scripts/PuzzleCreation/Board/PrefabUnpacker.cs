using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CircuitPuzzle
{
    [ExecuteInEditMode]
    public class PrefabUnpacker : MonoBehaviour
    {
        [SerializeField]
        private bool isUnpacked;

        #region UNITY METHODS
        private void Awake()
        {
            // Unpacks the prefab once it is placed on the hierarchy.
            // Only does this if it hasn't already been unpacked.
            if (!isUnpacked)
            {
                PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

                isUnpacked = true;
            }
        }
        #endregion
    }
}