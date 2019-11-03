using System;
//TODO: MAKE LOOPABLE, CURRENTLY HAVE TO RESTART ALL THE TIME!!!
public class C_Timer {

    public Action OnTimerComplete;
    public Action OnTimerUpdated;

    public float ElapsedTime { get; private set; }
    public float Alpha { get; private set; }
    public float TimerLength { get; private set; }
    public bool TimerCompleted { get; private set; }
    public bool TimerCompletedThisFrame { get; private set; }
    public bool TimerPaused { get; private set; } = false;

    public void UpdateTimer(float deltaTime)
    {
        if (TimerPaused) return;

        ElapsedTime += deltaTime;
        ElapsedTime = ElapsedTime > TimerLength ? TimerLength : ElapsedTime;

        float perc = ElapsedTime == 0 ? 0 : ElapsedTime / TimerLength;
        Alpha = perc;

        OnTimerUpdated?.Invoke();
        CheckIfComplete();
    }

    void CheckIfComplete()
    {
        if (ElapsedTime == TimerLength)
        {
            OnComplete();
        }
        else
        {
            TimerCompleted = false;
            TimerCompletedThisFrame = false;
        }
    }

    void OnComplete()
    {
        if (!TimerCompleted)
        {
            Alpha = 1;
            TimerCompleted = true;
            TimerCompletedThisFrame = true;

            OnTimerComplete?.Invoke();
        }
        else
        {
            TimerCompletedThisFrame = false;
        }
    }

    public void SetTimerLength(float lerpTime)
    {
        TimerLength = lerpTime;
        ElapsedTime = 0;

        if (TimerLength == 0)
        {
            ElapsedTime = 1;
            Alpha = 1;
            TimerCompletedThisFrame = false;
            TimerCompleted = false;
        }
    }

    public void SetTimerAlphaPosition(float alpha)
    {
        ElapsedTime = TimerLength * alpha;
        Alpha  = alpha;
    }

    public void Restart()
    {
        ElapsedTime = 0;
        Alpha = 0;

        if (TimerLength == 0)
        {
            ElapsedTime = 1;
            Alpha = 1;
            TimerCompletedThisFrame = false;
            TimerCompleted = false;
        }
    }

    public void Pause()
    {
        TimerPaused = true;
    }

    public void UnPause()
    {
        TimerPaused = false;
    }
}
