using UnityEngine;
using UnityEditor;

namespace PhysicsDraw.EditorScripts
{
    [InitializeOnLoad]
    public static class AutoFixLevel11_3
    {
        static AutoFixLevel11_3()
        {
            EditorApplication.delayCall += () =>
            {
                if (!EditorPrefs.GetBool("Level11ForceFixed_v7", false))
                {
                    ForceFixLevel11.ForceRun();
                    EditorPrefs.SetBool("Level11ForceFixed_v7", true);
                    Debug.Log(">>> AutoFixLevel11_3 executed automatically! <<<");
                }
            };
        }
    }
}
