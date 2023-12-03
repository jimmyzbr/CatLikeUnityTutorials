using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace EasyKit
{
    public class CoroutineTask
    {
        private bool mRunning;

        private bool mPaused;

        private MonoBehaviour mBehaviour;

        private Coroutine mCoroutine;

        private Action<CoroutineTask> mHandle;

        private List<IEnumerator> mItorStack;

        internal bool endFrameClear { get; set; }

        public string TaskName { get; set; }

        public bool ForceKill { get; set; }

        public string BelongScene { get; private set; }

        public Exception Error { get; private set; }

        public void Init(IEnumerator coroutine, string taskName, string belong, MonoBehaviour mb,
            Action<CoroutineTask> handle = null)
        {
            mItorStack = new List<IEnumerator>();
            mItorStack.Add(coroutine);
            endFrameClear = false;
            this.TaskName = taskName;
            BelongScene = belong;
            mHandle = handle;
            ForceKill = false;
            mBehaviour = mb;
            Error = null;
        }

        internal void Quit()
        {
            ForceKill = true;
            mRunning = false;
            Clear();
        }

        public void Start()
        {
            mRunning = true;
            mPaused = false;
            mCoroutine = mBehaviour.StartCoroutine(CallWrapper());
        }

        public void Pause(bool pause)
        {
            mPaused = pause;
        }

        private void Clear()
        {
            bool flag = mItorStack != null;
            if (flag)
            {
                while (mItorStack.Count > 0)
                {
                    int index = mItorStack.Count - 1;
                    IEnumerator routine = mItorStack[index];
                    mBehaviour.StopCoroutine(routine);
                    mItorStack.RemoveAt(index);
                }
            }

            mBehaviour = null;
            mCoroutine = null;
        }

        private IEnumerator CallWrapper()
        {
            bool endFrameClear = this.endFrameClear;
            if (endFrameClear)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                yield return null;
            }

            IEnumerator enumerator = null;
            bool flag = mItorStack != null && mItorStack.Count > 0;
            if (flag)
            {
                enumerator = mItorStack[0];
            }

            bool flag2 = enumerator == null;
            if (flag2)
            {
                Error = new NullReferenceException("POP Fail");
                mRunning = false;
            }

            while (mRunning && !ForceKill)
            {
                bool flag3 = mPaused;
                if (flag3)
                {
                    yield return null;
                }
                else
                {
                    try
                    {
                        Profiler.BeginSample(enumerator.ToString());
                        bool flag4 = !enumerator.MoveNext();
                        if (flag4)
                        {
                            mRunning = false;
                            break;
                        }

                        Profiler.EndSample();
                    }
                    catch (Exception ex2)
                    {
                        Exception ex = ex2;
                        Debug.LogErrorFormat("Execute Error:{0}", new object[]
                        {
                            ex.ToString()
                        });
                        Error = ex;
                        mRunning = false;
                        break;
                    }

                    bool flag5 = enumerator != null;
                    if (!flag5)
                    {
                        Error = new NullReferenceException(string.Format("Task: {0}->Host不安全, 已处理此异常", TaskName));
                        mRunning = false;
                        break;
                    }

                    bool flag6 = enumerator.Current is IEnumerator;
                    if (flag6)
                    {
                        mItorStack.Add(enumerator.Current as IEnumerator);
                    }

                    yield return enumerator.Current;
                }
            }

            bool flag7 = !ForceKill && mHandle != null;
            if (flag7)
            {
                mHandle(this);
            }

            yield break;
        }
    }
}