using UnityEngine;
using UnityEditor;
using System.IO;

namespace PhysicsDraw.EditorScripts
{
    public static class Phase14_1_GenerateArt
    {
        [MenuItem("PhysicsDraw/Phase 14.1 - Generate Art")]
        public static void GenerateArt()
        {
            string dir = "Assets/_Game/Art/UI";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            // 1. Rounded Rect
            CreateTexture(dir + "/RoundedRect.png", 128, 128, (x, y, w, h) =>
            {
                float rx = x - w / 2f;
                float ry = y - h / 2f;
                float r = 24f; // corner radius
                if (Mathf.Abs(rx) > w/2f - r && Mathf.Abs(ry) > h/2f - r)
                {
                    float dist = Vector2.Distance(new Vector2(Mathf.Abs(rx), Mathf.Abs(ry)), new Vector2(w/2f - r, h/2f - r));
                    return dist < r ? Color.white : Color.clear;
                }
                return Color.white;
            });

            // 2. Circle (Ball / Round Button)
            CreateTexture(dir + "/Circle.png", 128, 128, (x, y, w, h) =>
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(w/2f, h/2f));
                return dist < w/2f ? Color.white : Color.clear;
            });

            // 3. Simple Play Triangle
            CreateTexture(dir + "/Icon_Play.png", 128, 128, (x, y, w, h) =>
            {
                // Triangle pointing right
                float nx = (float)x / w;
                float ny = (float)y / h;
                if (nx > 0.2f && nx < 0.8f)
                {
                    float halfH = 0.5f;
                    float slope = 0.5f * (nx - 0.2f) / 0.6f;
                    if (ny > halfH - (0.5f - slope) && ny < halfH + (0.5f - slope)) return Color.white;
                }
                return Color.clear;
            });

            // 4. Grid Background
            CreateTexture(dir + "/NotebookGrid.png", 256, 256, (x, y, w, h) =>
            {
                if (x % 64 == 0 || y % 64 == 0) return new Color(0.8f, 0.9f, 1f, 0.5f); // light blue lines
                return new Color(0.98f, 0.98f, 0.95f, 1f); // warm paper white
            });

            AssetDatabase.Refresh();

            // Set all to Sprite
            foreach (var file in Directory.GetFiles(dir, "*.png"))
            {
                TextureImporter importer = AssetImporter.GetAtPath(file) as TextureImporter;
                if (importer != null && importer.textureType != TextureImporterType.Sprite)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    if (file.Contains("RoundedRect"))
                    {
                        importer.spriteBorder = new Vector4(32, 32, 32, 32);
                    }
                    if (file.Contains("NotebookGrid"))
                    {
                        importer.wrapMode = TextureWrapMode.Repeat;
                    }
                    importer.SaveAndReimport();
                }
            }

            Debug.Log("[Phase 14.1] UI Art Generated and Configured.");
        }

        private static void CreateTexture(string path, int width, int height, System.Func<int, int, int, int, Color> colorFunc)
        {
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tex.SetPixel(x, y, colorFunc(x, y, width, height));
                }
            }
            tex.Apply();
            File.WriteAllBytes(path, tex.EncodeToPNG());
            Object.DestroyImmediate(tex);
        }
    }
}
