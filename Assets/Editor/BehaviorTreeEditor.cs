using Code.Characters.Doods.AI;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(BehaviorTree))]
    public class BehaviorTreeEditor : UnityEditor.Editor
    {
        private SerializedProperty _root;

        private void OnEnable () {
            _root = serializedObject.FindProperty("Root");
        }

        public override void OnInspectorGUI () {
            base.OnInspectorGUI();
            serializedObject.Update();
//            EditorGUILayout.PropertyField(_root);
        }
    }
}