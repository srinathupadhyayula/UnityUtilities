﻿using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Utilities.Attributes;

namespace Utilities.PropertyDrawers
{
    /// <summary>
    /// This class is part of the <i>NaughtyAttributes</i> plugin.
    /// </summary>
    /// <remarks>
    /// Author: <i cref="https://denisrizov.com/">Denis Rizov</i>
    /// <br/><i cref="https://github.com/dbrizov/NaughtyAttributes"><b>See: </b>NaughtyAttributes</i>
    /// <br/><i cref="https://github.com/dbrizov"><b>See Also: </b>Github Profile</i>
    /// </remarks>
    public class ReorderableListPropertyDrawer : SpecialCasePropertyDrawerBase
    {
        public static readonly ReorderableListPropertyDrawer Instance = new();

        private readonly Dictionary<string, ReorderableList> m_reorderableListsByPropertyName = new();

        private GUIStyle m_labelStyle;

        private GUIStyle GetLabelStyle()
        {
            if (m_labelStyle == null)
            {
                m_labelStyle = new GUIStyle(EditorStyles.boldLabel);
                m_labelStyle.richText = true;
            }

            return m_labelStyle;
        }

        private string GetPropertyKeyName(SerializedProperty property)
        {
            return property.serializedObject.targetObject.GetInstanceID() + "." + property.name;
        }

        protected override float GetPropertyHeight_Internal(SerializedProperty property)
        {
            if (property.isArray)
            {
                string key = GetPropertyKeyName(property);

                if (m_reorderableListsByPropertyName.TryGetValue(key, out ReorderableList reorderableList) == false)
                {
                    return 0;
                }

                return reorderableList.GetHeight();
            }

            return EditorGUI.GetPropertyHeight(property, true);
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (property.isArray)
            {
                string key = GetPropertyKeyName(property);

                ReorderableList reorderableList = null;
                if (!m_reorderableListsByPropertyName.ContainsKey(key))
                {
                    reorderableList = new ReorderableList(property.serializedObject, property, true, true, true, true)
                    {
                        drawHeaderCallback = (Rect r) =>
                        {
                            EditorGUI.LabelField(r, string.Format("{0}: {1}", label.text, property.arraySize), GetLabelStyle());
                            HandleDragAndDrop(r, reorderableList);
                        },

                        drawElementCallback = (Rect r, int index, bool isActive, bool isFocused) =>
                        {
                            SerializedProperty element = property.GetArrayElementAtIndex(index);
                            r.y += 1.0f;
                            r.x += 10.0f;
                            r.width -= 10.0f;

                            EditorGUI.PropertyField(new Rect(r.x, r.y, r.width, EditorGUIUtility.singleLineHeight), element, true);
                        },

                        elementHeightCallback = (int index) =>
                        {
                            return EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index)) + 4.0f;
                        }
                    };

                    m_reorderableListsByPropertyName[key] = reorderableList;
                }

                reorderableList = m_reorderableListsByPropertyName[key];

                if (rect == default)
                {
                    reorderableList.DoLayoutList();
                }
                else
                {
                    reorderableList.DoList(rect);
                }
            }
            else
            {
                string message = typeof(ReorderableListAttribute).Name + " can be used only on arrays or lists";
                NaughtyEditorGUI.HelpBox_Layout(message, MessageType.Warning, context: property.serializedObject.targetObject);
                EditorGUILayout.PropertyField(property, true);
            }
        }

        public void ClearCache()
        {
            m_reorderableListsByPropertyName.Clear();
        }

        private Object GetAssignableObject(Object obj, ReorderableList list)
        {
            System.Type listType = PropertyUtility.GetPropertyType(list.serializedProperty);
            System.Type elementType = ReflectionUtility.GetListElementType(listType);

            if (elementType == null)
            {
                return null;
            }

            System.Type objType = obj.GetType();

            if (elementType.IsAssignableFrom(objType))
            {
                return obj;
            }

            if (objType == typeof(GameObject))
            {
                if (typeof(Transform).IsAssignableFrom(elementType))
                {
                    Transform transform = ((GameObject)obj).transform;
                    if (elementType == typeof(RectTransform))
                    {
                        RectTransform rectTransform = transform as RectTransform;
                        return rectTransform;
                    }
                    else
                    {
                        return transform;
                    }
                }
                else if (typeof(MonoBehaviour).IsAssignableFrom(elementType))
                {
                    return ((GameObject)obj).GetComponent(elementType);
                }
            }

            return null;
        }

        private void HandleDragAndDrop(Rect rect, ReorderableList list)
        {
            var currentEvent = Event.current;
            var usedEvent = false;

            switch (currentEvent.type)
            {
                case EventType.DragExited:
                    if (GUI.enabled)
                    {
                        HandleUtility.Repaint();
                    }

                    break;

                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (rect.Contains(currentEvent.mousePosition) && GUI.enabled)
                    {
                        // Check each single object, so we can add multiple objects in a single drag.
                        bool didAcceptDrag = false;
                        Object[] references = DragAndDrop.objectReferences;
                        foreach (Object obj in references)
                        {
                            Object assignableObject = GetAssignableObject(obj, list);
                            if (assignableObject != null)
                            {
                                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                                if (currentEvent.type == EventType.DragPerform)
                                {
                                    list.serializedProperty.arraySize++;
                                    int arrayEnd = list.serializedProperty.arraySize - 1;
                                    list.serializedProperty.GetArrayElementAtIndex(arrayEnd).objectReferenceValue = assignableObject;
                                    didAcceptDrag = true;
                                }
                            }
                        }

                        if (didAcceptDrag)
                        {
                            GUI.changed = true;
                            DragAndDrop.AcceptDrag();
                            usedEvent = true;
                        }
                    }

                    break;
            }

            if (usedEvent)
            {
                currentEvent.Use();
            }
        }
    }
}
