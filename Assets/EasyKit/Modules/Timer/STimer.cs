using System;
using UnityEngine;

namespace EasyKit
{
    public enum TimerState
    {
        Ready,
        Run,
        Remove
    }

    public class STimer : IPooledObject
    {
        public string BelongScene;

        public float StepTime;

        public bool UseRealTime;

        public string Name;

        public Action<float> StepAction;

        public Action TimesComplete;

        public float Times;

        public TimerState State;

        public bool ForceKill;

        private float mStartTime;

        private float mLastTime;

        private bool mIsIntegral;

        private float mCurrentStepTime;

        public bool Init(object param = null)
        {
            return true;
        }

        public void OnDispose()
        {
        }

        public void Spawning()
        {
            BelongScene = null;
            State = TimerState.Ready;
            ForceKill = false;
            mStartTime = (mLastTime = 0f);
            mCurrentStepTime = (StepTime = (Times = 0f));
            StepAction = null;
            TimesComplete = null;
            Name = null;
            UseRealTime = false;
        }

        public void DeSpawning()
        {
        }

        public void Start()
        {
            State = TimerState.Run;
            mStartTime = Time.realtimeSinceStartup;
            float num = Mathf.Ceil(StepTime);
            float num2 = num - StepTime;
            mIsIntegral = (num2 == 0f);
        }

        public void Run(float dt)
        {
            bool flag = ForceKill;
            if (!flag)
            {
                bool flag2 = UseRealTime;
                if (flag2)
                {
                    mLastTime = Time.realtimeSinceStartup;
                    float num = mLastTime - mStartTime;
                    mCurrentStepTime += num;
                    mStartTime = mLastTime;
                    Times -= num;
                }
                else
                {
                    Times -= dt;
                    mCurrentStepTime += dt;
                }

                bool flag3 = mCurrentStepTime >= StepTime;
                if (flag3)
                {
                    mCurrentStepTime -= StepTime;
                    bool flag4 = StepAction != null;
                    if (flag4)
                    {
                        try
                        {
                            StepAction((Times <= 0f) ? 0f : (mIsIntegral ? Mathf.Ceil(Times) : Times));
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError(ex.ToString());
                        }
                    }
                }

                bool flag5 = Times <= 0f;
                if (flag5)
                {
                    State = TimerState.Remove;
                }
            }
        }
    }
}