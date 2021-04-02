using UnityEngine;
using UnityEditor;

namespace Framework.Editor
{
    sealed class CustomDecalBaseShaderGUI : CustomShaderBaseGUI
    {
#region Structs
        class Labels
        {
            public static readonly string BaseColor = "Base Color";
            public static readonly string BaseTexture = "Base Texture";
        }

        class PropertyNames
        {
            public static readonly string BaseMap = "_BaseMap";
            public static readonly string BaseColor = "_BaseColor";
        }
#endregion

#region Fields
        MaterialProperty m_BaseMapProp;
        MaterialProperty m_BaseColorProp;
#endregion

#region GUI
        public override void GetProperties(MaterialProperty[] properties)
        {
            m_BaseMapProp = FindProperty(PropertyNames.BaseMap, properties, false);
            m_BaseColorProp = FindProperty(PropertyNames.BaseColor, properties, false);
        }

        public override void DrawSurfaceInputs(MaterialEditor materialEditor)
        {
            materialEditor.ColorProperty(m_BaseColorProp, Labels.BaseColor);
            materialEditor.TextureProperty(m_BaseMapProp, Labels.BaseTexture);
        }
        #endregion
    }
}
