using UnityEditor;
using UnityEngine;
using Utilities.Attributes;
using Utilities.Types;

namespace Utilities.DecoratorDrawers
{
    /// <summary>
    /// This class is part of the <i>NaughtyAttributes</i> plugin.
    /// </summary>
    /// <remarks>
    /// Author: <i cref="https://denisrizov.com/">Denis Rizov</i>
    /// <br/><i cref="https://github.com/dbrizov/NaughtyAttributes"><b>See: </b>NaughtyAttributes</i>
    /// <br/><i cref="https://github.com/dbrizov"><b>See Also: </b>Github Profile</i>
    /// </remarks>
    [CustomPropertyDrawer(typeof(InfoBoxAttribute))]
    public class InfoBoxDecoratorDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            return GetHelpBoxHeight();
        }

        public override void OnGUI(Rect rect)
        {
            InfoBoxAttribute infoBoxAttribute = (InfoBoxAttribute)attribute;

            float indentLength = NaughtyEditorGUI.GetIndentLength(rect);
            Rect infoBoxRect = new Rect(
                rect.x + indentLength,
                rect.y,
                rect.width - indentLength,
                GetHelpBoxHeight());

            DrawInfoBox(infoBoxRect, infoBoxAttribute.Text, infoBoxAttribute.Type);
        }

        private float GetHelpBoxHeight()
        {
            InfoBoxAttribute infoBoxAttribute = (InfoBoxAttribute)attribute;
            float minHeight = EditorGUIUtility.singleLineHeight * 2.0f;
            float desiredHeight = GUI.skin.box.CalcHeight(new GUIContent(infoBoxAttribute.Text), EditorGUIUtility.currentViewWidth);
            float height = Mathf.Max(minHeight, desiredHeight);

            return height;
        }

        private void DrawInfoBox(Rect rect, string infoText, EInfoBoxType infoBoxType)
        {
            MessageType messageType = MessageType.None;
            switch (infoBoxType)
            {
                case EInfoBoxType.Normal:
                    messageType = MessageType.Info;
                    break;

                case EInfoBoxType.Warning:
                    messageType = MessageType.Warning;
                    break;

                case EInfoBoxType.Error:
                    messageType = MessageType.Error;
                    break;
            }

            NaughtyEditorGUI.HelpBox(rect, infoText, messageType);
        }
    }
}
