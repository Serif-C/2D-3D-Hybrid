using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class CooldownEvent
{
    /// <summary>
    /// Time when the event was last invoked.
    /// </summary>
    public float LastInvokedTime = 0;
    public float Cooldown = 1;
    public float Remaining = 0;
    public float Noise = 0;

    protected const int m_MaxCallers = 3;
    protected float m_CurrentNoise;

    public bool IsDone { get => Remaining <= 0; }

    protected HashSet<int> m_Callers;

    public CooldownEvent() 
    {
        m_Callers = new HashSet<int>(m_MaxCallers);
    }

    public CooldownEvent(float cooldown)
    {
        m_Callers = new HashSet<int>(m_MaxCallers);
        m_CurrentNoise = (float)(new System.Random().NextDouble() * Noise);
        Cooldown = cooldown;
        Remaining = cooldown + m_CurrentNoise;
    }

    public CooldownEvent(float cooldown, float noise)
    {
        m_Callers = new HashSet<int>(m_MaxCallers);
        Noise = noise;
        m_CurrentNoise = (float)(new System.Random().NextDouble() * Noise);
        Cooldown = cooldown;
        Remaining = cooldown + m_CurrentNoise;
    }

    public CooldownEvent(CooldownEvent other) : this(other.Cooldown, other.Noise)
    {
    }

    public void Reset(float cooldown)
    {
        Cooldown = cooldown;
        Reset();
    }

    public void Reset()
    {
        m_Callers.Clear();
        m_CurrentNoise = UnityEngine.Random.value * Noise;
        Remaining = Cooldown + m_CurrentNoise;
    }
    
    public void Finish()
    {
        Remaining = 0f;
    }

    public virtual float GetRemainingNormalized() 
    {
        return Remaining / (Cooldown + m_CurrentNoise);
    }

    public virtual void SetRemainingNormalized(float n)
    {
        Remaining = math.clamp(n, 0, 1) * (Cooldown + m_CurrentNoise);
    }

    public void Decrement(float t)
    {
        Remaining -= t;

        if (Remaining < 0)
        {
            Remaining = 0;
        }
    }

    public void DecrementIf(bool condition, float t)
    {
        if (condition)
        {
            Decrement(t);
        }
    }

    public bool TryAcknowledgeIsDone()
    {
        return TryAcknowledgeIsDone(0);
    }

    public bool TryAcknowledgeIsDone(int callerId)
    {
        if (m_Callers.Contains(callerId))
        {
            return false;
        }

        if (Remaining > 0)
        {
            return false;
        }

        m_Callers.Add(callerId);
        return true;
    }
}
