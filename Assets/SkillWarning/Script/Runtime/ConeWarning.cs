using kTools.Decals;
using System.Collections;
using UnityEngine;

namespace Framework.Rendering
{
    [ExecuteInEditMode]
    public class ConeWarning : SkillWarning
    {
        [SerializeField]
        private float m_Size = 10f;

        [Range(0, 2)]
        private float m_Speed = 0f;

        [SerializeField]
        [Range(0, 360)]
        private float m_Angle = 45;

        public Decal LBorder;
        public Decal RBorder;
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

        public float Angle
        {
            get
            {
                return m_Angle;
            }
            set
            {
                this.m_Angle = value;
                OnValueChanged();
            }
        }

        private void SetAngle(float angle)
        {
            SetShaderFloat("_Expand", Normalize(angle + 1, 360));
            LBorder.transform.localEulerAngles = new Vector3(0, 0, (angle + 2) / 2);
            RBorder.transform.localEulerAngles = new Vector3(0, 0, (-angle - 2) / 2);
        }

        private void SetSize(float scale)
        {
            if (m_Decals == null)
            {
                return;
            }

            foreach (var decal in m_Decals)
            {
                if (decal != Fill)
                {
                    decal.transform.localScale = new Vector3(scale, scale, 1);
                }
            }
        }

        public override void OnValueChanged()
        {
            SetSize(m_Size);
            SetProgress(m_Progress);
            SetAngle(m_Angle);
        }

        protected override void SetProgress(float progress)
        {
            var curSize = progress * Size;
            Fill.transform.localScale = new Vector3(curSize, curSize, 1);
        }

        public override void OnShow()
        {
            base.OnShow();
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            float final = Angle;
            float current = 0;

            if (m_Speed > 0)
            {
                while (current < final)
                {
                    Angle = current;
                    current += final * m_Speed * 0.1f;
                    yield return null;
                }
            }

            Angle = final;
            yield return null;
        }
    }
}
