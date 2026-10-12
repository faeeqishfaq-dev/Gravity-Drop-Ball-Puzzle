using UnityEngine;
using UnityEditor;

namespace PhysicsDraw.EditorScripts
{
    public static class AutoFixLevel11
    {
        [InitializeOnLoadMethod]
        public static void RunFixOnce()
        {
            if (EditorPrefs.GetBool("Level11Fixed", false)) return;

            FixLevel11Setup.Fix();
            EditorPrefs.SetBool("Level11Fixed", true);
        }
    }
}
