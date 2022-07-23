using UnityEditor;
using UnityEngine;

//-----------------------------------------------------------------------------
namespace Utilities.Editors
{
    public class EditorTools : Editor
    {
        public static void RegisterUndo(string name, params Object[] objects)
        {
            if (objects is not
                {
                    Length: > 0
                })
            {
                return;
            }
            
            Undo.RecordObjects(objects, name);
            foreach (Object obj in objects)
            {

                if (obj == null)
                {
                    continue;
                }

                EditorUtility.SetDirty(obj);
            }
        }
    }
}