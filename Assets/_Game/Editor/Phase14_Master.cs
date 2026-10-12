using UnityEditor;
using UnityEngine;

namespace PhysicsDraw.EditorScripts
{
    //[InitializeOnLoad]
    public static class Phase14_Master
    {
        static Phase14_Master()
        {
            if (!SessionState.GetBool("Phase14_AutoRun_Done_v2", false))
            {
                SessionState.SetBool("Phase14_AutoRun_Done_v2", true);
                EditorApplication.delayCall += RunAll;
            }
        }

        [MenuItem("PhysicsDraw/Phase 14 - FULL VISUAL OVERHAUL (Click Me)")]
        public static void RunAll()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.LogWarning("Exit Play Mode first!");
                return;
            }

            Debug.Log("Starting Full Visual Overhaul...");

            Phase14_1_GenerateArt.GenerateArt();
            Phase14_2_BuildScenes.BuildScenes();
            Phase14_6_BuildLevelSelect.BuildLevelSelect();
            Phase14_3_BuildGameplay.BuildGameplay();
            Phase14_4_RebuildLevels.RebuildLevels();
            Phase14_5_PolishPrefabs.PolishPrefabs();

            Debug.Log("===================================");
            Debug.Log("VISUAL OVERHAUL COMPLETE!");
            Debug.Log("===================================");
        }
    }
}
