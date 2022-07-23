using UnityEditor;
using UnityEngine;
using Utilities.Types;

namespace Utilities.PropertyDrawers
{
    /// <summary>
    /// Property Drawer for the Ranged-Types
    /// </summary>
    /// <remarks>
    /// This class is based on <b cref="http://www.demigiant.com">Daniele Giardini</b>'s
    /// <i cref="https://github.com/Demigiant/demilib">demlib</i> package.
    /// Note: This class is heavily modified from the original at demlib
    /// by <i cref="https://github.com/srinathupadhyayula">Srinath Upadhyayula</i>
    /// </remarks>
    [CustomPropertyDrawer(typeof(RangedInt))]
    [CustomPropertyDrawer(typeof(RangedUInt))]
    [CustomPropertyDrawer(typeof(RangedLong))]
    [CustomPropertyDrawer(typeof(RangedULong))]
    [CustomPropertyDrawer(typeof(RangedShort))]
    [CustomPropertyDrawer(typeof(RangedUShort))]
    [CustomPropertyDrawer(typeof(RangedByte))]
    [CustomPropertyDrawer(typeof(RangedUByte))]
    [CustomPropertyDrawer(typeof(RangedFloat))]
    [CustomPropertyDrawer(typeof(RangedDouble))]
    public class RangedPropertyDrawer : PropertyDrawerBase
    {
        private struct SerializedNumericValues
        {
            public sbyte sByteValue;
            public short sShortValue;
            public int   sIntValue;
            public long  sLongValue;

            public byte   uByteValue;
            public ushort uShortValue;
            public uint   uIntValue;
            public ulong  uLongValue;

            public float  floatValue;
            public double doubleValue;
        }

        protected override void OnGUI_Internal(Rect position, SerializedProperty property, GUIContent label)
        {
            property.Next(true);
            SerializedProperty min = property.Copy();
            property.Next(true);
            SerializedProperty max = property.Copy();
            
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, label);
            var positionMin = new Rect(position.x,           position.y,    position.width * 0.5f, position.height);
            var positionMax = new Rect(positionMin.xMax + 3, positionMin.y, positionMin.width - 3, positionMin.height);
            EditorGUI.showMixedValue = property.hasMultipleDifferentValues;

            float defLabelW = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 28;
            var subLabel = new GUIContent("Min");
            EditorGUI.BeginProperty(positionMin, subLabel, min);
            EditorGUI.BeginChangeCheck();
            DrawField(positionMin, subLabel, min, out SerializedNumericValues nums);
            if (EditorGUI.EndChangeCheck())
            {
                GUI.changed = true;
                UpdateMinValue(ref min, max, nums);
            }

            EditorGUI.EndProperty();
            subLabel.text = "Max";
            EditorGUI.BeginProperty(positionMax, subLabel, max);
            EditorGUI.BeginChangeCheck();
            DrawField(positionMax, subLabel, max, out nums);
            if (EditorGUI.EndChangeCheck()) 
            {
                GUI.changed = true;
                UpdateMaxValue(min, ref max, nums);
            }
            EditorGUI.EndProperty();
            EditorGUIUtility.labelWidth = defLabelW;

            EditorGUI.EndProperty();
        }

        private static void DrawField(Rect position, GUIContent subLabel, SerializedProperty property, out SerializedNumericValues nums)
        {
            nums = new SerializedNumericValues();
            string typeString = property.type;
            switch (typeString)
            {
                case "int":
                    nums.sIntValue = EditorGUI.IntField(position, subLabel, property.intValue);
                    break;
                case "uint":
                    nums.uIntValue = (uint)EditorGUI.IntField(position, subLabel, property.intValue);
                    break;
                case "long":
                    nums.sLongValue = EditorGUI.LongField(position, subLabel, property.longValue);
                    break;
                case "ulong":
                    nums.uLongValue = (ulong)EditorGUI.LongField(position, subLabel, property.longValue);
                    break;
                case "short":
                    nums.sShortValue = (short)EditorGUI.IntField(position, subLabel, property.intValue);
                    break;
                case "ushort":
                    nums.uShortValue = (ushort)EditorGUI.IntField(position, subLabel, property.intValue);
                    break;
                case "byte":
                    nums.uByteValue = (byte)EditorGUI.IntField(position, subLabel, property.intValue);
                    break;
                case "sbyte":
                    nums.sByteValue = (sbyte)EditorGUI.IntField(position, subLabel, property.intValue);
                    break;
                case "float":
                    nums.floatValue = EditorGUI.FloatField(position, subLabel, property.floatValue);
                    break;
                case "double":
                    nums.doubleValue = EditorGUI.DoubleField(position, subLabel, property.doubleValue);
                    break;
            }
        }

        private static void UpdateMinValue(ref SerializedProperty min, SerializedProperty max, SerializedNumericValues nums)
        {
            string typeString = min.type;

            switch (typeString)
            {
                case "int":
                    if (nums.sIntValue > max.intValue)
                    {
                        nums.sIntValue = max.intValue;
                    }
                    min.intValue = nums.sIntValue;
                    break;
                case "uint":
                    if (nums.uIntValue > (uint)max.intValue)
                    {
                        nums.uIntValue = (uint)max.intValue;
                    }
                    min.longValue = nums.uIntValue;
                    min.intValue  = (int) min.longValue;
                    break;
                case "long":
                    if (nums.sLongValue > max.longValue)
                    {
                        nums.sLongValue = max.longValue;
                    }
                    min.longValue = nums.sLongValue;
                    break;
                case "ulong":
                    if (nums.uLongValue > (ulong)max.longValue)
                    {
                        nums.uLongValue = (ulong)max.longValue;
                    }
                    min.longValue = (long)nums.uLongValue;
                    break;
                case "short":
                    if (nums.sShortValue > (short)max.intValue)
                    {
                        nums.sShortValue = (short)max.intValue;
                    }
                    min.intValue = nums.sShortValue;
                    break;
                case "ushort":
                    if (nums.uShortValue > (ushort)max.intValue)
                    {
                        nums.uShortValue = (ushort)max.intValue;
                    }
                    min.intValue = nums.uShortValue;
                    break;
                case "byte":
                    if (nums.uByteValue > max.intValue)
                    {
                        nums.uByteValue = (byte)max.intValue;
                    }
                    min.intValue = nums.uByteValue;
                    break;
                case "sbyte":
                    if (nums.sByteValue > max.intValue)
                    {
                        nums.sByteValue = (sbyte)max.intValue;
                    }
                    min.intValue = nums.sByteValue;
                    break;
                case "float":
                    if (nums.floatValue > max.floatValue)
                    {
                        nums.floatValue = max.floatValue;
                    }
                    min.floatValue = nums.floatValue;
                    break;
                case "double":
                    if (nums.doubleValue > max.doubleValue)
                    {
                        nums.doubleValue = max.doubleValue;
                    }
                    min.doubleValue = nums.doubleValue;
                    break;
            }
        }
        
        private static void UpdateMaxValue(SerializedProperty min, ref SerializedProperty max, SerializedNumericValues nums)
        {
            string typeString = min.type;

            switch (typeString)
            {
                case "int":
                    if (nums.sIntValue < min.intValue)
                    {
                        nums.sIntValue = min.intValue;
                    }
                    max.intValue = nums.sIntValue;
                    break;
                case "uint":
                    if (nums.uIntValue < (uint)min.intValue)
                    {
                        nums.uIntValue = (uint)min.intValue;
                    }
                    max.intValue = (int)nums.uIntValue;
                    break;
                case "long":
                    if (nums.sLongValue < min.longValue)
                    {
                        nums.sLongValue = min.longValue;
                    }
                    max.longValue = nums.sLongValue;
                    break;
                case "ulong":
                    if (nums.uLongValue < (ulong)min.longValue)
                    {
                        nums.uLongValue = (ulong)min.longValue;
                    }
                    max.longValue = (long)nums.uLongValue;
                    break;
                case "short":
                    if (nums.sShortValue < (short)min.intValue)
                    {
                        nums.sShortValue = (short)min.intValue;
                    }
                    max.intValue = nums.sShortValue;
                    break;
                case "ushort":
                    if (nums.uShortValue < (ushort)min.intValue)
                    {
                        nums.uShortValue = (ushort)min.intValue;
                    }
                    max.intValue = nums.uShortValue;
                    break;
                case "byte":
                    if (nums.uByteValue < (byte)min.intValue)
                    {
                        nums.uByteValue = (byte)min.intValue;
                    }
                    max.intValue = nums.uByteValue;
                    break;
                case "sbyte":
                    if (nums.sByteValue < (sbyte)min.intValue)
                    {
                        nums.sByteValue = (sbyte)min.intValue;
                    }
                    max.intValue = nums.sByteValue;
                    break;
                case "float":
                    if (nums.floatValue < min.floatValue)
                    {
                        nums.floatValue = min.floatValue;
                    }
                    max.floatValue = nums.floatValue;
                    break;
                case "double":
                    if (nums.doubleValue < min.doubleValue)
                    {
                        nums.doubleValue = min.doubleValue;
                    }
                    max.doubleValue = nums.doubleValue;
                    break;
            }
        }
    }
}