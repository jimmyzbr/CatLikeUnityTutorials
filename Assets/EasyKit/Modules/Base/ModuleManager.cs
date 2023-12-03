using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyKit
{
    public static class ModuleManager
    {
        private class ModuleWrapper
        {
            public int Priority { private set; get; }
            public IModule Module { private set; get; }

            public bool NeedUpdate { private set; get; }
            
            public bool IsStarted { set; get; }

            public ModuleWrapper(IModule module, int priority, bool needUpdate)
            {
                Module = module;
                Priority = priority;
                NeedUpdate = needUpdate;
                IsStarted = false;
            }
        }
        
        //所有模块合集
        private static  readonly Dictionary<Type, ModuleWrapper> mAllModules = new Dictionary<Type, ModuleWrapper>();
        //需要更新的模块合集
        private static readonly LinkedList<ModuleWrapper> mNeedUpdateModules = new LinkedList<ModuleWrapper>();
        
        
        /// <summary>
        /// 创建子模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="priority"> 运行时的优先级，优先级越大越早执行。如果没有设置优先级，那么会按照添加顺序执行 </param>
        /// <param name="needUpdate"></param>
        /// <param name="createParam"> 创建参数</param>
        /// <returns></returns>
        public static T CreateModule<T>(bool needUpdate = false,int priority = 0, System.Object createParam = null) where T : IModule
        {
            if (priority < 0)
                throw new Exception("The priority can not be negative");
            
            if(Contains(typeof(T)))
                throw new Exception($"Game module {typeof(T)} is already existed");
            
            if (priority == 0)
            {
                var min = GetMinPriority();
                priority = --min;
            }

            var moduleType = typeof(T);
            T module = Activator.CreateInstance<T>();
            ModuleWrapper wrapper = new ModuleWrapper(module, priority,needUpdate);
            wrapper.Module.OnCreate(createParam);
            
            mAllModules.Add(moduleType,wrapper);
        
            if (needUpdate)
            {
                LinkedListNode<ModuleWrapper> currrent = mNeedUpdateModules.First;
                while (currrent != null)
                {
                    if (wrapper.Priority > currrent.Value.Priority)
                        break;
                    currrent = currrent.Next;
                }
                if (currrent != null)
                {
                    mNeedUpdateModules.AddBefore(currrent, wrapper);
                }
                else
                {
                    mNeedUpdateModules.AddLast(wrapper);
                }
            }
            return (T)module;
        }
        
        /// <summary>
        /// 查询游戏模块是否存在
        /// </summary>
        public static bool Contains(System.Type moduleType)
        {
            return mAllModules.ContainsKey(moduleType);
        }
        
        /// <summary>
        /// 获取游戏模块
        /// </summary>
        /// <typeparam name="T">模块类</typeparam>
        public static T GetModule<T>() where T : class, IModule
        {
            System.Type type = typeof(T);
            if (mAllModules.TryGetValue(type, out ModuleWrapper wrapper))
            {
                return (T)wrapper?.Module;
            }
            return null;
        }
        
        
        /// <summary>
        /// 启动所有系统
        /// </summary>
        public static void StartAllModules()
        {
            List<ModuleWrapper> allModules = mAllModules.Values.ToList();
            allModules.Sort((modA, modB) =>
            {
                if (modA.Priority > modB.Priority)
                    return -1;
                else if (modA.Priority == modB.Priority)
                    return 0;
                else
                    return 1;
            });

            for (int i = 0; i < allModules.Count; i++)
            {
                if (allModules[i] != null && !allModules[i].IsStarted)
                {
                    allModules[i].Module.OnStart();
                    allModules[i].IsStarted = true;
                }
            }
        }

        public static void OnApplicationQuit()
        {
            Shutdown();
        }
        
        /// <summary>
        /// 停止所有系统
        /// </summary>
        private static void Shutdown()
        {
            var moduleList = mAllModules.Values.ToList();
            for(int i = moduleList.Count - 1; i >= 0; i--)
            {
                moduleList[i].Module.Shutdown();
            }
            mNeedUpdateModules.Clear();
            mAllModules.Clear();
        }

        /// <summary>
        /// 更新所有模块
        /// </summary>
        public static void OnUpdate()
        {
            LinkedListNode<ModuleWrapper> currrent = mNeedUpdateModules.First;
            while(currrent != null)
            {
                var moduleWrapper = currrent.Value;
                if(moduleWrapper.IsStarted)
                {
                    moduleWrapper.Module.OnUpdate();
                }
                currrent = currrent.Next;
            }
        }
        
        /// <summary>
        /// 绘制所有模块的GUI内容
        /// </summary>
        public static void OnModuleGUI()
        {
            foreach (var kvp in mAllModules)
            {
                kvp.Value.Module.OnGUI();
            }
        }
        

        /// <summary>
        /// 获取当前模块里最小的优先级
        /// </summary>
        private static int GetMinPriority()
        {
            int minPriority = 0;
            foreach (var kvp in mAllModules)
            {
                if (kvp.Value.Priority < minPriority)
                    minPriority = kvp.Value.Priority;
            }
            return minPriority; //小于等于零
        }
        
    }
}