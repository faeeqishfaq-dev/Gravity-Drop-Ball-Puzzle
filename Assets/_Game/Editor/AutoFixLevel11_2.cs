using UnityEngine;
using UnityEditor;

namespace PhysicsDraw.EditorScripts
{
    [InitializeOnLoad]
    public static class AutoFixLevel11_2
    {
        static AutoFixLevel11_2()
        {
            EditorApplication.delayCall += () =>
            {
                if (!EditorPrefs.GetBool("Level11ForceFixed_v5", false))
                {
                    ForceFixLevel11.ForceRun();
                    EditorPrefs.SetBool("Level11ForceFixed_v5", true);
                    Debug.Log(">>> AutoFixLevel11_2 executed automatically! <<<");
                }
            };
        }
    }
}
