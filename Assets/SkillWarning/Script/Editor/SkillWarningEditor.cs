using UnityEditor;
using Framework.Rendering;

namespace Framework.Editor
{

    public class SkillWarningEditor<T> : UnityEditor.Editor where T : SkillWarning
    {
        private T instance { get { return (T)target; } }

        public override void OnInspectorGUI()
        {
            if (instance == null)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();

            DrawDefaultInspector();

            if (EditorGUI.EndChangeCheck())
            {
                instance.OnValueChanged();
            }
        }
    }

    [CustomEditor(typeof(ConeWarning))]
    public class ConeWarningEditor : SkillWarningEditor<ConeWarning>
    {
    }

    [CustomEditor(typeof(RingWarning))]
    public class RingWarningEditor : SkillWarningEditor<RingWarning>
    {
    }

    [CustomEditor(typeof(CircleWarning))]
    public class CircleWarningEditor : SkillWarningEditor<CircleWarning>
    {
    }

    [CustomEditor(typeof(RectangleWarning))]
    public class RectangleWarningEditor : SkillWarningEditor<RectangleWarning>
    {
    }
}
