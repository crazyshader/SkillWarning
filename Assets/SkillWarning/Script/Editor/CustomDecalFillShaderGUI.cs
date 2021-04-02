using UnityEngine;
using UnityEditor;

namespace Framework.Editor
{
    sealed class CustomDecalFillShaderGUI : CustomShaderBaseGUI
    {
#region Structs
        class Labels
        {
            public static readonly string BaseColor = "Base Color";
            public static readonly string BaseTexture = "Base Texture";
            public static readonly string FillColor = "Fill Color";
            public static readonly string Fill = "Fill Progress";
            public static readonly string Expand = "Cone Angle";
        }

        class PropertyNames
        {
            public static readonly string BaseMap = "_BaseMap";
            public static readonly string BaseColor = "_BaseColor";
            public static readonly string FillColor = "_FillColor";
            public static readonly string Expand = "_Expand";
            public static readonly string Fill = "_Fill";
        }
#endregion

#region Fields
        MaterialProperty m_BaseMapProp;
        MaterialProperty m_BaseColorProp;
        MaterialProperty m_FillColorProp;
        MaterialProperty m_ExpandProp;
        MaterialProperty m_FillProp;
#endregion

#region GUI
        public override void GetProperties(MaterialProperty[] properties)
        {
            m_BaseMapProp = FindProperty(PropertyNames.BaseMap, properties, false);
            m_BaseColorProp = FindProperty(PropertyNames.BaseColor, properties, false);
            m_FillColorProp = FindProperty(PropertyNames.FillColor, properties, false);
            m_ExpandProp = FindProperty(PropertyNames.Expand, properties, false);
            m_FillProp = FindProperty(PropertyNames.Fill, properties, false);
        }

        public override void DrawSurfaceInputs(MaterialEditor materialEditor)
        {
            materialEditor.ColorProperty(m_BaseColorProp, Labels.BaseColor);
            materialEditor.TextureProperty(m_BaseMapProp, Labels.BaseTexture);
        }

        public override void DrawAdvancedOptions(MaterialEditor materialEditor)
        {
            materialEditor.ColorProperty(m_FillColorProp, Labels.FillColor);
            materialEditor.RangeProperty(m_ExpandProp, Labels.Expand);
            materialEditor.RangeProperty(m_FillProp, Labels.Fill);
        }
        #endregion
    }
}
