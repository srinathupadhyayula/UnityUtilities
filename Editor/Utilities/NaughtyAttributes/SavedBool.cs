using UnityEditor;

namespace Utilities
{
    /// <summary>
    /// This interface is part of the <i>NaughtyAttributes</i> plugin.
    /// </summary>
    /// <remarks>
    /// Author: <i cref="https://denisrizov.com/">Denis Rizov</i>
    /// <br/><i cref="https://github.com/dbrizov/NaughtyAttributes"><b>See: </b>NaughtyAttributes</i>
    /// <br/><i cref="https://github.com/dbrizov"><b>See Also: </b>Github Profile</i>
    /// </remarks>
    internal class SavedBool
    {
        private          bool   m_value;
        private readonly string m_name;

        public bool Value
        {
            get => m_value;
            set
            {
                if (m_value == value)
                {
                    return;
                }

                m_value = value;
                EditorPrefs.SetBool(m_name, value);
            }
        }

        public SavedBool(string name, bool value)
        {
            m_name = name;
            m_value = EditorPrefs.GetBool(name, value);
        }
    }
}