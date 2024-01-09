using System;
using EasyKit.ECS;
using UnityEngine;

namespace EasyKit
{
    public enum AppMode
    {
        Developing,
        Qa,
        Release
    }

    public class GameApp : MonoSingleton<GameApp>
    {
        public AppMode m_appMode = AppMode.Developing;
        
        public bool m_useAssetsBundle = false;
        
        public static AppMode AppMode
        {
            get
            {
#if APPMODE_DEV
	                return AppMode.Developing;
#elif APPMODE_QA
	                return AppMode.QA;
#elif APPMODE_REL
	                return AppMode.Release;
#else
                return Instance.m_appMode;
#endif
            }
        }
        
        protected override void OnInit()
        {
            base.OnInit();
            Debug.Log("===================== on Init");
            ModuleManager.CreateModule<CoroutineManager>();
            ModuleManager.CreateModule<TimerManager>(true);
            ModuleManager.CreateModule<GameManager>(true);
        }
        
        protected override void OnAwake()
        {
            base.OnAwake();
            Debug.Log("===================== on Awake");
        }
        
        protected override void OnStart()
        {
            base.OnStart();
            ModuleManager.StartAllModules();
        }


        private void Update()
        {
            ModuleManager.OnUpdate();
        }
    
        private void OnGUI()
        {
            ModuleManager.OnModuleGUI();
        }
        
        private void LateUpdate()
        {
        }

        private void OnApplicationPause(bool pauseStatus)
        {
        }

        private void OnApplicationQuit()
        {
            ModuleManager.OnApplicationQuit();
        }

    }
}