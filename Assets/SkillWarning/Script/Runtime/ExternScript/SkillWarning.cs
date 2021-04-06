using kTools.Decals;
using UnityEngine;

namespace Framework.Rendering
{
    public abstract class SkillWarning : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 5)]
        private float m_Time = 1;

        public float Time
        {
            get
            {
                return m_Time;
            }
            set
            {
                this.m_Time = value;
                OnValueChanged();
            }
        }

        protected Decal[] m_Decals;
        protected float m_LastTime;
        protected float m_Progress = 0;
        protected bool m_IsShow = true;

        private void Awake()
        {
            m_Decals = GetComponentsInChildren<Decal>();
        }

        private void Update()
        {
            if (m_IsShow)
            {
                var totalTime = UnityEngine.Time.time - m_LastTime;
                if (totalTime <= m_Time)
                {
                    m_Progress = Normalize(totalTime, Mathf.Max(0.01f, m_Time));
                    SetProgress(m_Progress);
                }
                else
                {
                    m_Progress = 1;
                    SetProgress(m_Progress);
                }
            }
        }

        public virtual void OnShow()
        {
            if (m_Decals == null)
            {
                return;
            }

            m_IsShow = true;
            m_LastTime = UnityEngine.Time.time;

            foreach (var decal in m_Decals)
            {
                decal.enabled = true;
            }

            OnValueChanged();
        }

        public virtual void OnHide()
        {
            if (m_Decals == null)
            {
                return;
            }

            m_IsShow = false;
            foreach (var decal in m_Decals)
            {
                decal.enabled = false;
            }
        }

        protected virtual void SetProgress(float progress)
        {

        }

        public virtual void OnValueChanged()
        {

        }

        protected float Normalize(float portion, float max)
        {
            return Mathf.Clamp(portion / Mathf.Max(0.001f, max), 0, 1f);
        }

        protected float Remap(float input, float inputMin, float inputMax, float outputMin, float outPutMax)
        {
            return outputMin + (input - inputMin) * (outPutMax - outputMin) / (inputMax - inputMin);
        }

        protected void SetShaderFloat(string property, float value)
        {
            if (m_Decals == null)
            {
                return;
            }

            foreach (var decal in m_Decals)
            {
                var material = decal.decalData.material;
                if (material.HasProperty(property))
                {
                    material.SetFloat(property, value);
                }
            }
        }
    }
}
