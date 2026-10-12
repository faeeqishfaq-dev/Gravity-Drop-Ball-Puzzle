using UnityEngine;
using UnityEditor;

namespace PhysicsDraw.EditorScripts
{
    [InitializeOnLoad]
    public class ForceRunSettingsUIBuilder
    {
        static ForceRunSettingsUIBuilder()
        {
            EditorApplication.delayCall += () =>
            {
                EditorPrefs.SetBool("SettingsUIBuilt", false);
            };
        }
    }
}
