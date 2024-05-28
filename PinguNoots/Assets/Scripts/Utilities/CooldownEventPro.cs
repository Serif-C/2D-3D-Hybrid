using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class CooldownEventPro : CooldownEvent
{

    [SerializeField]
    private AnimationCurve m_Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField]
    private bool m_ReverseCurve = false;

    [SerializeField]
    private float2 m_Remap = new float2(0, 1);

    public CooldownEventPro() : base()
    {
    }

    public CooldownEventPro(float cooldown) : base(cooldown)
    {
    }

    public CooldownEventPro(CooldownEventPro ce, float duration)
    {
        m_Curve = new AnimationCurve(ce.m_Curve.keys);
        m_ReverseCurve = ce.m_ReverseCurve;
        m_Remap = ce.m_Remap;
        Reset(duration);
    }

    public CooldownEventPro(CooldownEventPro ce)
    {
        m_Curve = new AnimationCurve(ce.m_Curve.keys);
        m_ReverseCurve = ce.m_ReverseCurve;
        m_Remap = ce.m_Remap;
        Reset();
    }

    public override float GetRemainingNormalized() 
    {
        return math.lerp(
            m_Remap.x, m_Remap.y,
            m_ReverseCurve
                ? m_Curve.Evaluate(1f - base.GetRemainingNormalized())
                : m_Curve.Evaluate(base.GetRemainingNormalized()));
    }

    public float GetRemainingNormalizedSkipRemap()
    {
        return m_ReverseCurve
                ? m_Curve.Evaluate(1f - base.GetRemainingNormalized())
                : m_Curve.Evaluate(base.GetRemainingNormalized());
    }

    public float GetRemainingNormalizedRaw()
    {
        return base.GetRemainingNormalized();
    }
}
