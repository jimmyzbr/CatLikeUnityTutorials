using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyKit
{
    public class TimerManager : ModuleSingleton<TimerManager>, IModule
    {
        private List<STimer> mReadyLst;

        private List<STimer> mUpdateLst;

        private List<STimer> mRemoveLst;

        private int mCount;

        private bool mAddChange;

        private bool mRemoveChange;

        private STimer mTmpTimer;

        private CycleBuffer<STimer> mPools;

        private static readonly int POOLS_NUMBER = 20;
        
        public void OnCreate(object createParam)
        {
            mAddChange = (mRemoveChange = false);
            mPools = new CycleBuffer<STimer>(POOLS_NUMBER);
            mReadyLst = new List<STimer>();
            mUpdateLst = new List<STimer>();
            mRemoveLst = new List<STimer>();
        }

        public void OnStart()
        {
        }

        public  void Shutdown()
        {
            //todo:fixme
            ClearSceneAll("");
        }

        public void OnUpdate()
        {
            OnTimerLogic(Time.deltaTime);
        }

        public void OnGUI()
        {
        }

        private void OnTimerLogic(float dt)
        {
            bool flag = mAddChange;
            if (flag)
            {
                mAddChange = false;
                mCount = mReadyLst.Count;
                int i = 0;
                int num = mCount;
                while (i < num)
                {
                    mTmpTimer = mReadyLst[i];
                    bool flag2 = mTmpTimer != null;
                    if (flag2)
                    {
                        TimerState state = mTmpTimer.State;
                        if (state != TimerState.Ready)
                        {
                            if (state == TimerState.Remove)
                            {
                                mRemoveLst.Add(mTmpTimer);
                                mRemoveChange = true;
                            }
                        }
                        else
                        {
                            mUpdateLst.Add(mTmpTimer);
                            mTmpTimer.Start();
                        }
                    }

                    i++;
                }

                mReadyLst.Clear();
            }

            mCount = mUpdateLst.Count;
            int j = 0;
            int num2 = mCount;
            while (j < num2)
            {
                mTmpTimer = mUpdateLst[j];
                bool flag3 = mTmpTimer != null;
                if (flag3)
                {
                    TimerState state2 = mTmpTimer.State;
                    if (state2 != TimerState.Run)
                    {
                        if (state2 == TimerState.Remove)
                        {
                            mRemoveChange = true;
                            mRemoveLst.Add(mTmpTimer);
                        }
                    }
                    else
                    {
                        mTmpTimer.Run(dt);
                    }
                }

                j++;
            }

            bool flag4 = mRemoveChange;
            if (flag4)
            {
                mRemoveChange = false;
                mCount = mRemoveLst.Count;
                int k = 0;
                int num3 = mCount;
                while (k < num3)
                {
                    mTmpTimer = mRemoveLst[k];
                    bool flag5 = mTmpTimer != null;
                    if (flag5)
                    {
                        bool flag6 = !mTmpTimer.ForceKill;
                        if (flag6)
                        {
                            bool flag7 = mTmpTimer.TimesComplete != null;
                            if (flag7)
                            {
                                try
                                {
                                    mTmpTimer.TimesComplete();
                                }
                                catch (Exception ex)
                                {
                                    Debug.LogError(ex.ToString());
                                }
                            }
                            else
                            {
                                Debug.LogError("timer callback null--->" + mTmpTimer.Name);
                            }
                        }

                        mUpdateLst.Remove(mTmpTimer);
                        mPools.recycle(mTmpTimer);
                    }

                    k++;
                }

                mRemoveLst.Clear();
            }
        }

        public void OnPreExitMap(string sceneName)
        {
            ClearSceneAll(sceneName);
        }

        public string StartTimer(float duration, Action timesComplete = null, float stepTime = 1f,
            Action<float> stepCallback = null, bool useRealTime = false, bool belongScene = true, string clockName = "")
        {
            if (duration < 0)
                duration = int.MaxValue;
            bool flag = timesComplete == null;
            string result;
            if (flag)
            {
                Debug.LogError("timer times complete null create clcok faill->");
                result = null;
            }
            else
            {
                bool flag2 = duration <= 0f;
                if (flag2)
                {
                    bool flag3 = timesComplete != null;
                    if (flag3)
                    {
                        timesComplete();
                    }

                    result = null;
                }
                else
                {
                    STimer sTimer = mPools.AcquireElement();
                    bool flag4 = sTimer == null;
                    if (flag4)
                    {
                        Debug.LogError("Acquire Clock fail-->");
                        result = null;
                    }
                    else
                    {
                        sTimer.Name = (string.IsNullOrEmpty(clockName)
                            ? ("Clock ID:" + UniqueId.NewId(UniqueIdType.Timer))
                            : clockName);
                        sTimer.StepAction = stepCallback;
                        sTimer.Times = duration;
                        sTimer.StepTime = stepTime;
                        sTimer.TimesComplete = timesComplete;
                        sTimer.UseRealTime = useRealTime;
                        sTimer.BelongScene = (belongScene ? "" : "");
                        mReadyLst.Add(sTimer);
                        mAddChange = true;
                        result = sTimer.Name;
                    }
                }
            }

            return result;
        }

        public void KillTimer(string name)
        {
            bool flag = !string.IsNullOrEmpty(name);
            if (flag)
            {
                InternalKill(name);
            }
        }

        public void KillTimer(STimer sTimer)
        {
            bool flag = sTimer != null;
            if (flag)
            {
                InternalKill(sTimer.Name);
            }
        }

        private void InternalKill(string clockName)
        {
            SetClockState(mReadyLst, clockName, new Action<STimer>(SetKillState));
            SetClockState(mUpdateLst, clockName, new Action<STimer>(SetKillState));
        }

        private void SetKillState(STimer sTimer)
        {
            sTimer.State = TimerState.Remove;
            sTimer.ForceKill = true;
        }

        private void SetClockState(List<STimer> lst, string clock_name, Action<STimer> proc)
        {
            bool flag = lst == null || lst.Count < 1;
            if (!flag)
            {
                int i = 0;
                int count = lst.Count;
                while (i < count)
                {
                    bool flag2 = lst[i] != null && !lst[i].ForceKill &&
                                 lst[i].Name.Equals(clock_name, StringComparison.Ordinal);
                    if (flag2)
                    {
                        bool flag3 = proc != null;
                        if (flag3)
                        {
                            proc(lst[i]);
                        }

                        break;
                    }

                    i++;
                }
            }
        }

        private void ClearSceneClock(List<STimer> lst, string sceneName)
        {
            bool flag = lst == null || lst.Count < 1 || string.IsNullOrEmpty(sceneName);
            if (!flag)
            {
                try
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        STimer sTimer = lst[i];
                        bool flag2 = sTimer != null && !string.IsNullOrEmpty(sTimer.BelongScene) &&
                                     sTimer.BelongScene.Equals(sceneName);
                        if (flag2)
                        {
                            sTimer.State = TimerState.Remove;
                            sTimer.ForceKill = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("清理场景:" + ex.ToString());
                }
            }
        }

        private void ClearSceneAll(string sceneName)
        {
            Debug.LogFormat("清理场景:{0}，当前拥有的clock", new object[]
            {
                sceneName
            });
            ClearSceneClock(mReadyLst, sceneName);
            ClearSceneClock(mUpdateLst, sceneName);
        }
    }
}