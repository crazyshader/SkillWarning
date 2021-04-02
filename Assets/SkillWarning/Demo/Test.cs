using Framework.Rendering;
using UnityEngine;

public class Test : MonoBehaviour
{
    public SkillWarning[] SkillWarning;

    private int m_Index = 0;

    private void Start()
    {
        HideAll();
    }

    private void HideAll()
    {
        if (SkillWarning == null || SkillWarning.Length == 0)
        {
            return;
        }

        foreach (var skillWarning in SkillWarning)
        {
            skillWarning.OnHide();
        }
    }

    void Update()
    {
        if (SkillWarning == null || SkillWarning.Length == 0)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_Index = (int)Mathf.Repeat(m_Index - 1, SkillWarning.Length);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_Index = (int)Mathf.Repeat(m_Index + 1, SkillWarning.Length);
        }

        var skillWarning = SkillWarning[m_Index % SkillWarning.Length];
        if (Input.GetMouseButtonDown(0))
        {
            HideAll();
            skillWarning.OnShow();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            skillWarning.OnHide();
        }
    }
}
