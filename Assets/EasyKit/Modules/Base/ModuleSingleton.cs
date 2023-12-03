using UnityEngine;

namespace EasyKit
{
    public abstract class ModuleSingleton<T> where T : class, IModule
    {
        private static T s_Instance;
        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                    Debug.LogError($"{typeof(T)} is not create");
                return s_Instance;
            }
        }

        protected ModuleSingleton()
        {
            if (s_Instance != null)
                throw new System.Exception($"{typeof(T)} instance already created.");
            s_Instance = this as T;
        }
    }
}