using UnityEngine;
using UnityEditor;

namespace jCaballol.GraphicsUtils
{
    [CustomEditor(typeof(BakedMaterial))]
    public class BakedMaterialEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Bake"))
            {
                var gradient = (BakedMaterial)target;
                gradient.GenerateTexture();
            }
        }
    }
}