using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameFlow.Editor
{
    public class ItemGameFlowContentElement : VisualElement
    {
        private const string kUxmlPath = "Packages/com.huyhung1404.gameflow/Editor/UXML/ItemGameFlowContentElement.uxml";
        private SerializedObject serializedObject;
        private SerializedProperty serializedProperty;
        private SerializedProperty includeInBuild;
        private SerializedProperty instanceID;
        private SerializedProperty reference;
        private bool active;
        private readonly EnumField releaseModeElement;
        private readonly Toggle fullSceneElement;
        private Action<int> removeAtIndex;
        private bool showDialog;

        public ItemGameFlowContentElement()
        {
            var root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(kUxmlPath).CloneTree();
            root.Q<IMGUIContainer>("title_gui").onGUIHandler = DrawTitleGUI;
            root.Q<Button>("remove_button").RegisterCallback<ClickEvent>(OnClickRemove);
            releaseModeElement = root.Q<EnumField>("release_mode");
            fullSceneElement = root.Q<Toggle>("full_scene");
            Add(root);
        }

        private void OnClickRemove(ClickEvent evt)
        {
            if (serializedProperty.ExtractArrayIndex() == null)
            {
                Debug.LogError("Not find array index");
                return;
            }

            showDialog = true;
        }

        private void DrawTitleGUI()
        {
            if (!active) return;
            var guiWidth = EditorGUIUtility.currentViewWidth;
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            var lastValue = includeInBuild.boolValue;
            includeInBuild.boolValue = EditorGUI.Toggle(new Rect(0, 0, 20, 20), GUIContent.none, includeInBuild.boolValue);
            if (includeInBuild.boolValue != lastValue)
            {
                var referenceValue = reference.GetAssetReferenceValue();
                if (referenceValue == null)
                {
                    includeInBuild.boolValue = lastValue;
                }
                else
                {
                    AddressableUtility.AddAddressableGroupGUID(referenceValue.AssetGUID, !lastValue);
                }
            }

            var idWidth = Mathf.Max(30, guiWidth / 4);
            instanceID.stringValue = EditorGUI.TextField(new Rect(22, 1, idWidth, 18), GUIContent.none, instanceID.stringValue);
            EditorGUI.PropertyField(new Rect(30 + idWidth, 1, Mathf.Max(45, guiWidth - idWidth - 125), 18), reference, GUIContent.none);
            if (showDialog)
            {
                ShowConfirmationDialog();
                showDialog = false;
            }

            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
        }

        private void ShowConfirmationDialog()
        {
            var index = serializedProperty.ExtractArrayIndex();
            if (index == null) return;
            var confirm = EditorUtility.DisplayDialog(
                "Remove element",
                "Are you sure you want to remove element?",
                "Yes",
                "No"
            );

            if (!confirm) return;
            removeAtIndex?.Invoke(index.Value);
            active = false;
            serializedObject.ApplyModifiedProperties();
        }

        public void UpdateGraphic(bool isUserInterface, Type type, SerializedProperty serialized, Action<int> removeAt)
        {
            serializedProperty = serialized;
            removeAtIndex = removeAt;
            serializedObject = serializedProperty.serializedObject;
            includeInBuild = serializedProperty.FindPropertyRelative(nameof(GameFlowElement.includeInBuild));
            instanceID = serializedProperty.FindPropertyRelative(nameof(GameFlowElement.instanceID));
            reference = serializedProperty.FindPropertyRelative(nameof(GameFlowElement.reference));
            releaseModeElement.BindProperty(serializedProperty.FindPropertyRelative(nameof(GameFlowElement.releaseMode)));
            if (isUserInterface)
            {
                fullSceneElement.BindProperty(serializedProperty.FindPropertyRelative(nameof(UserInterfaceFlowElement.fullScene)));
                fullSceneElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            }
            else
            {
                fullSceneElement.Unbind();
                fullSceneElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            }

            active = true;
        }

        public new class UxmlFactory : UxmlFactory<ItemGameFlowContentElement, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }
        }
    }
}