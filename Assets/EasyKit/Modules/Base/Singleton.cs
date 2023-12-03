using System;

namespace EasyKit
{
    /// <summary>
    /// 单例类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> where T : class ,new()
    {
        private static T s_Inst;

        public static T Instance 
        {
            get {
                if (s_Inst == null)
                {
                    s_Inst = Activator.CreateInstance<T>();
                    if (s_Inst != null)
                    {
                        (s_Inst as Singleton<T>).OnInit();
                    }
                }

                return s_Inst;
            }
        }

        public static bool HasInstance()
        {
            return s_Inst != null;
        }

        public static void DestroyInstance()
        {
            if(s_Inst != null)
            {
                (s_Inst as Singleton<T>)?.OnDestroy();
                s_Inst = (T)((object) null);
            }
        }

        public virtual void OnInit()
        {

        }

        public  virtual void OnDestroy(){

        }
    
    }
}