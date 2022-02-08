using UnityEngine;

namespace jCaballol.GraphicsUtils
{
    [CreateAssetMenu(fileName = "NewBakedMaterial", menuName = "Graphics Utils/Baked Material")]
    public class BakedMaterial : ScriptableObject
    {
        public Material material;
        public int shaderPass = 0;
        public Vector2Int resolution = new Vector2Int(128,128);
        public TextureFormat format = TextureFormat.RGBA32;
        public TextureWrapMode wrapMode = TextureWrapMode.Repeat;
        public FilterMode filterMode = FilterMode.Bilinear;

        public Texture2D GeneratedTexture => m_generatedTexture;
        [SerializeField] [HideInInspector] private Texture2D m_generatedTexture;

        public void GenerateTexture()
        {
            RegenerateTexture();
            var rt = PrepareRenderTexture();

            var pass = Mathf.Min(shaderPass, material.shader.passCount);
            Graphics.Blit(null, rt, material, pass);
            RenderTexture.active = rt;
            m_generatedTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            m_generatedTexture.Apply();
            RenderTexture.ReleaseTemporary(rt);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(m_generatedTexture);
#endif
        }

        private RenderTexture PrepareRenderTexture()
        {
            RenderTextureFormat rtFormat = RenderTextureFormat.Default;
            switch (format)
            {
                case TextureFormat.Alpha8:
                case TextureFormat.R8:
                    rtFormat = RenderTextureFormat.R8;
                    break;
                case TextureFormat.R16:
                    rtFormat = RenderTextureFormat.R16;
                    break;
                case TextureFormat.RFloat:
                    rtFormat = RenderTextureFormat.RFloat;
                    break;
                case TextureFormat.RG16:
                    rtFormat = RenderTextureFormat.RG16;
                    break;
                case TextureFormat.RG32:
                    rtFormat = RenderTextureFormat.RG32;
                    break;
            }

            return RenderTexture.GetTemporary(resolution.x, resolution.y, 0, rtFormat);
        }

        private void RegenerateTexture()
        {
            if (m_generatedTexture == null)
            {
                m_generatedTexture = new Texture2D(
                    resolution.x, resolution.y,
                    format, true, true);

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

            if (m_generatedTexture.format != format 
                || m_generatedTexture.width != resolution.x 
                || m_generatedTexture.height != resolution.y)
            {
                m_generatedTexture.Reinitialize(resolution.x, resolution.y, format, true);
            }

            m_generatedTexture.name = name + "_tex";
            m_generatedTexture.wrapMode = wrapMode;
            m_generatedTexture.filterMode = filterMode;
        }
    }
}