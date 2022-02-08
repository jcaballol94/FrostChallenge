using UnityEngine;

namespace jCaballol.GraphicsUtils
{
    [CreateAssetMenu(fileName = "NewGradient", menuName = "Graphics Utils/Gradient")]
    public class GradientAsset : ScriptableObject
    {
        private const int RESOLUTION = 32;
        public Gradient gradientDefinition = new Gradient();

        public Texture2D GeneratedTexture => m_generatedTexture;
        [SerializeField] [HideInInspector] private Texture2D m_generatedTexture;

        public void GenerateTexture()
        {
            RegenerateTexture();

            var texData = m_generatedTexture.GetRawTextureData<Color32>();
            for (int i = 0; i < texData.Length; ++i)
            {
                var idx = i % RESOLUTION;
                var ratio = idx / (float)(RESOLUTION - 1);

                texData[i] = gradientDefinition.Evaluate(ratio);
            }

            m_generatedTexture.Apply();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(m_generatedTexture);
#endif
        }

        private void RegenerateTexture()
        {
            if (m_generatedTexture == null)
            {
                m_generatedTexture = new Texture2D(
                    RESOLUTION, 4,
                    TextureFormat.RGBA32,
                    false, true);

#if UNITY_EDITOR
                m_generatedTexture.name = name + "_tex";
                var path = UnityEditor.AssetDatabase.GetAssetPath(this);
                if (!string.IsNullOrEmpty(path))
                {
                    UnityEditor.AssetDatabase.AddObjectToAsset(m_generatedTexture, this);
                    UnityEditor.AssetDatabase.ImportAsset(path);

                }
#endif
            }

            m_generatedTexture.name = name + "_tex";
            m_generatedTexture.wrapMode = TextureWrapMode.Clamp;
        }
    }
}