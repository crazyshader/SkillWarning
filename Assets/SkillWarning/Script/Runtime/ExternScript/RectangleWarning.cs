using kTools.Decals;
using UnityEngine;

namespace Framework.Rendering
{
    [ExecuteInEditMode]
    public class RectangleWarning : SkillWarning
    {
        [SerializeField]
        private float m_Size = 10f;

        public Decal Fill;

        public float Size
        {
            get
            {
                return m_Size;
            }
            set
            {
                this.m_Size = value;
                OnValueChanged();
            }
        }

        private void SetSize(float scale)
        {
            if (m_Decals == null)
            {
                return;
            }

            foreach (var decal in m_Decals)
            {
                decal.transform.localScale = new Vector3(scale, scale, 1);
            }
        }

        public override void OnValueChanged()
        {
            SetSize(m_Size);
            SetProgress(m_Progress);
        }

        protected override void SetProgress(float progress)
        {
            SetShaderFloat("_Fill", progress);
        }
    }
}
