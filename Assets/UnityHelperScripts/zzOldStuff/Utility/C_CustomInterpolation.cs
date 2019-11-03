using UnityEngine;

public enum InterpolationType { linear, smoothStep, smootherStep, easeIn, easeOut, exponential }
public class C_CustomInterpolation
{

    float _lerpLength = 0;
    float currentLerpTime = 0;
    float alpha = 0;

    public bool b_PauseLerp = false;
    public bool b_ReverseOnComplete = false;

    public bool b_isReversed = false;
    bool b_lerpComplete;
    //TODO ADD PLAY IN REVERSE
    //Set Alpha min/max
    public delegate void OnLerpComplete();
    public OnLerpComplete DelegateOnlerpComplete;

    public void UpdateLerp(float deltaTime)
    {
        CheckIfComplete();

        if (b_PauseLerp) return;

        currentLerpTime += deltaTime;
        currentLerpTime = currentLerpTime > _lerpLength ? _lerpLength : currentLerpTime;

        float perc = currentLerpTime == 0 ? 0 : currentLerpTime / _lerpLength;
        alpha = b_isReversed ? 1 - perc : perc;


    }

    public float Evaluate(float alpha, bool clamp)
    {
        return clamp ? Mathf.Clamp(alpha, 0, 1) : (alpha * _lerpLength) / _lerpLength;
    }
    void CheckIfComplete()
    {
        if (currentLerpTime == _lerpLength)
        {
            OnComplete();
        }
        else
        {
            b_lerpComplete = false;
        }
    }

    public void OnComplete()
    {
        if (!b_lerpComplete)
        {
            b_lerpComplete = true;

            if (DelegateOnlerpComplete != null)
            {
                DelegateOnlerpComplete();
            }

            if (b_ReverseOnComplete)
            {
                Restartlerp();
                b_isReversed = !b_isReversed;
            }
        }
    }

    public void SetLerpLength(float lerpTime)
    {
        _lerpLength = lerpTime;
        currentLerpTime = 0;
    }

    public void Restartlerp()
    {
        currentLerpTime = 0;
    }

    public void JumpToPositionAlpha(float alpha, bool clamp)
    {
        alpha = clamp ? Mathf.Clamp01(alpha) : alpha;

        currentLerpTime = _lerpLength * alpha;
    }
    #region Alpha Calculations
    public float GetAlpha(InterpolationType lerpType)
    {
        switch (lerpType)
        {
            case InterpolationType.linear: return alpha;
            case InterpolationType.smoothStep: return SmoothStepAlpha();
            case InterpolationType.smootherStep: return SmootherStepAlpha();
            case InterpolationType.easeIn: return b_isReversed ? EaseOutAlpha() : EaseInAlpha();
            case InterpolationType.easeOut: return b_isReversed ? EaseInAlpha() : EaseOutAlpha();
            case InterpolationType.exponential: return ExponentialAlpha();

            default: return alpha;
        }
    }

    public float GetAlpha()
    {
        return alpha;
    }

    float SmoothStepAlpha()
    {
        return alpha * alpha * (3f - 2f * alpha);
    }

    float SmootherStepAlpha()
    {
        return alpha * alpha * alpha * (alpha * (6f * alpha - 15f) + 10f);
    }

    float EaseOutAlpha()
    {
        return Mathf.Sin(alpha * Mathf.PI * 0.5f);
    }
    float EaseInAlpha()
    {
        return (1f - Mathf.Cos(alpha * Mathf.PI * 0.5f));
    }

    float ExponentialAlpha()
    {
        return alpha * alpha;
    }

    #endregion

    public void OnLerpCompleteAttachListener(OnLerpComplete function)
    {
        DelegateOnlerpComplete = function;
    }

    public void DebugData()
    {
        Debug.Log("Logging Lerp Data");
        Debug.Log("LerpTime: " + _lerpLength);
        Debug.Log("CurrentLerpTime: " + currentLerpTime);
        Debug.Log("Current alpha: " + alpha);
        Debug.Log("Log Complete");
    }

    public float CurrentLerpLength
    {
        get { return currentLerpTime; }
    }
}


