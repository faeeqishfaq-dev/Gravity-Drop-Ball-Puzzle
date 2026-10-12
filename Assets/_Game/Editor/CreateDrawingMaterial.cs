using UnityEngine;
using UnityEditor;

namespace PhysicsDraw.EditorScripts
{
    public static class CreateDrawingMaterial
    {
        public static void Create()
        {
            string path = "Assets/_Game/Resources/Materials/DrawingMaterial.mat";
            Material mat = new Material(Shader.Find("Sprites/Default"));
            mat.color = Color.blue;
            AssetDatabase.CreateAsset(mat, path);
            AssetDatabase.SaveAssets();
            Debug.Log("Created DrawingMaterial at " + path);
        }
    }
}
