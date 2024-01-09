using System;
using UnityEngine;

namespace EasyKit.ECS
{
    public class GameManager : ModuleSingleton<GameManager>,IModule
    {
        public bool IsRunning { get; private set; }
        
        public SystemsManager SystemsManager { get; private set; }
        
        
        public void OnCreate(object createParam)
        {
            SystemsManager = new SystemsManager(this);
            SystemsManager.Setup();
        }

        public void OnStart()
        {
        }

        public void StartGame()
        {
            if(IsRunning)
                return;
            //注意此处需要再游戏开始时执行run
            SystemsManager.DoStart();
            
            IsRunning = true;
        }

        public void Shutdown()
        {
            SystemsManager.DoDestroy();
        }

        public void OnUpdate()
        {
            if(!IsRunning)
                return;
            SystemsManager.DoUpdate(Time.deltaTime);
        }

        public void OnGUI()
        {
        }
    }
}