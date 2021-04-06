using kTools.Decals;
using UnityEngine;

namespace Framework.Rendering
{
    [ExecuteInEditMode]
    public class RingWarning : SkillWarning
    {
        [SerializeField]
        private float m_InnerSize = 5f;

        [SerializeField]
        private float m_OuterSize = 10f;

        public Decal Inner;
        public Decal Outer;
        public Decal Fill;

        public float InnerSize
        {
            get
            {
                return m_InnerSize;
            }
            set
            {
                this.m_InnerSize = value;
                OnValueChanged();
            }
        }

        public float OuterSize
        {
            get
            {
                return m_OuterSize;
            }
            set
            {
                this.m_OuterSize = value;
                OnValueChanged();
            }
        }

        private void SetInnerSize(float scale)
        {
            Inner.transform.localScale = new Vector3(scale, scale, 1);
        }

        private void SetOuterSize(float scale)
        {
            Outer.transform.localScale = new Vector3(scale, scale, 1);
        }

        public override void OnValueChanged()
        {
            SetInnerSize(m_InnerSize);
            SetOuterSize(m_OuterSize);
            SetProgress(m_Progress);
        }

        protected override void SetProgress(float progress)
        {
            var curSize = InnerSize + (progress * (OuterSize - InnerSize));
            Fill.transform.localScale = new Vector3(curSize, curSize, 1);
        }
    }
}
