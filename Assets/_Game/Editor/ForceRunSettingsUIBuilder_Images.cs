using UnityEngine;
using UnityEditor;

namespace PhysicsDraw.EditorScripts
{
    [InitializeOnLoad]
    public class ForceRunSettingsUIBuilder_Images
    {
        static ForceRunSettingsUIBuilder_Images()
        {
            EditorApplication.delayCall += () =>
            {
                EditorPrefs.SetBool("SettingsUIBuilt", false);
            };
        }
    }
}
