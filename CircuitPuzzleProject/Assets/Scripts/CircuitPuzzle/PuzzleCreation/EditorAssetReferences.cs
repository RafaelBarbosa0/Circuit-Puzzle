using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CircuitPuzzle
{
    public class EditorAssetReferences : MonoBehaviour
    {
        // Textures.
        [SerializeField]
        private Texture2D leftArrow;
        [SerializeField] 
        private Texture2D rightArrow;

        // Styles.
        [SerializeField]
        private GUIStyle headerStyle;
        [SerializeField]
        private GUIStyle labelStyle;
        [SerializeField]
        private GUIStyle fieldStyle;
        [SerializeField]
        private GUIStyle buttonStyle;
        [SerializeField]

        public GUIStyle LabelStyle { get => labelStyle; private set => labelStyle = value; }
        public GUIStyle FieldStyle { get => fieldStyle; private set => fieldStyle = value; }
        public GUIStyle ButtonStyle { get => buttonStyle; private set => buttonStyle = value; }
        public GUIStyle HeaderStyle { get => headerStyle; private set => headerStyle = value; }
        public Texture2D LeftArrow { get => leftArrow; private set => leftArrow = value; }
        public Texture2D RightArrow { get => rightArrow; private set => rightArrow = value; }
    }
}
