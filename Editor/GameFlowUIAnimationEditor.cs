using GameFlow.Component;
using UnityEditor;
using UnityEngine;

namespace GameFlow.Editor
{
    [CustomEditor(typeof(GameFlowUIAnimation), true)]
    public class GameFlowUIAnimationEditor : UnityEditor.Editor
    {
        private static readonly string[] propertyToExclude = new string[]
        {
            "m_Script",
            "element"
        };

        private SerializedProperty elementProperty;

        protected virtual void OnEnable()
        {
            elementProperty = serializedObject.FindProperty("element");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(elementProperty, GUIContent.none);
            EditorGUILayout.Space(5);
            DrawPropertiesExcluding(serializedObject, propertyToExclude);
            serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomEditor(typeof(BubbleUIAnimation))]
    public class GameFlowSimpleUIAnimationEditor : GameFlowUIAnimationEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}