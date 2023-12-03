using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EasyKit
{
    public class CoroutineManager : ModuleSingleton<CoroutineManager> ,IModule
    {
        private static readonly string TAG = nameof(CoroutineManager);

        private class TaskMount : MonoBehaviour
        {
        }

        private GameObject mMount;

        private MonoBehaviour mBehaviour;

        private const int DEFAULT_CAPACITY = 20;

        private Dictionary<string, CoroutineTask> mTasks;

        private List<string> mDeath;
        

        public void OnCreate(object createParam)
        {
            mTasks = new Dictionary<string, CoroutineTask>(DEFAULT_CAPACITY);
            mDeath = new List<string>();
            mMount = new GameObject("TaskMount");
            mBehaviour = mMount.AddComponent<TaskMount>();
            GameObject.DontDestroyOnLoad(mMount);
        }

        public void OnStart()
        {
        }

        public void Shutdown()
        {
            Clear();
            mBehaviour = null;
            bool flag = mMount != null;
            if (flag)
            {
                UnityEngine.Object.Destroy(mMount);
            }

            mDeath.Clear();
            mDeath = null;
            mMount = null;
            mTasks = null;
        }

        public void OnUpdate()
        {
        }

        public void OnGUI()
        {
        }

        public bool IsExistTask(string taskName)
        {
            return mTasks.ContainsKey(taskName);
        }

        public string CreateTask(IEnumerator coroutine, string taskName = "", bool belongScene = true, bool autoStart = true)
        {
            bool flag = mTasks.ContainsKey(taskName);
            string result;
            if (flag)
            {
                Debug.LogError(taskName + "已经存在相同task");
                result = null;
            }
            else
            {
                CoroutineTask coroutineTask = new CoroutineTask();
                coroutineTask.Init(coroutine,
                    string.IsNullOrEmpty(taskName) ? ("TId:" + UniqueId.NewId(UniqueIdType.Task)) : taskName,
                    "", mBehaviour, TaskFinshed);
                mTasks[coroutineTask.TaskName] = coroutineTask;
                if (autoStart)
                {
                    coroutineTask.Start();
                }

                result = coroutineTask.TaskName;
            }

            return result;
        }

        public bool RemoveTask(string taskName)
        {
            bool flag = string.IsNullOrEmpty(taskName);
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                CoroutineTask coroutineTask;
                mTasks.TryGetValue(taskName, out coroutineTask);
                bool flag2 = coroutineTask != null;
                if (flag2)
                {
                    coroutineTask.Quit();
                    mTasks.Remove(taskName);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public void PauseTask(string taskName, bool pause)
        {
            CoroutineTask coroutineTask;
            mTasks.TryGetValue(taskName, out coroutineTask);
            bool flag = coroutineTask != null;
            if (flag)
            {
                coroutineTask.Pause(pause);
            }
        }

        private void TaskFinshed(CoroutineTask coroutineTask)
        {
            bool flag = coroutineTask.Error != null;
            if (flag)
            {
                Debug.LogError(coroutineTask.Error.ToString());
            }

            RemoveTask(coroutineTask.TaskName);
        }

        private void Clear()
        {
            Dictionary<string, CoroutineTask>.Enumerator enumerator = mTasks.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, CoroutineTask> current = enumerator.Current;
                current.Value.Quit();
            }

            mTasks.Clear();
        }

        private void ClearSceneAllTask(string sceneName)
        {
            bool flag = mDeath == null || mTasks == null || string.IsNullOrEmpty(sceneName);
            if (!flag)
            {
                mDeath.Clear();
                Dictionary<string, CoroutineTask>.Enumerator enumerator = mTasks.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, CoroutineTask> current = enumerator.Current;
                    CoroutineTask value = current.Value;
                    bool flag2 = value != null;
                    if (flag2)
                    {
                        bool flag3 = !string.IsNullOrEmpty(value.BelongScene);
                        if (flag3)
                        {
                            bool flag4 = value.BelongScene.Equals(sceneName, StringComparison.Ordinal);
                            if (flag4)
                            {
                                bool flag5 = !string.IsNullOrEmpty(value.TaskName);
                                if (flag5)
                                {
                                    mDeath.Add(value.TaskName);
                                }

                                value.Quit();
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("清理task->null");
                    }
                }

                int i = 0;
                int count = mDeath.Count;
                while (i < count)
                {
                    mTasks.Remove(mDeath[i]);
                    i++;
                }
            }
        }
    }
}
