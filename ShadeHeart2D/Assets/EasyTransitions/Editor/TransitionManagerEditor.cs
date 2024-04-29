using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace EasyTransition
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TransitionManager))]
    public class TransitionManagerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
        }
    }

}
    
#endif