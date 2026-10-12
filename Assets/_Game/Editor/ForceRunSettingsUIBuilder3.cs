using UnityEngine;
using UnityEditor;

namespace PhysicsDraw.EditorScripts
{
    [InitializeOnLoad]
    public class ForceRunSettingsUIBuilder3
    {
        static ForceRunSettingsUIBuilder3()
        {
            EditorApplication.delayCall += () =>
            {
                EditorPrefs.SetBool("SettingsUIBuilt", false);
            };
        }
    }
}
