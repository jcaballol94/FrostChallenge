using UnityEngine;
using UnityEditor;

namespace jCaballol.GraphicsUtils
{
    [CustomEditor(typeof(GradientAsset))]
    public class GradientAssetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Bake"))
            {
                var gradient = (GradientAsset)target;
                gradient.GenerateTexture();
            }
        }
    }
}