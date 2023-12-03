using System.Collections;
using UnityEngine;

namespace EasyKit.Testing
{
    public class EasyKitTest : MonoBehaviour
    {

        [ContextMenu("TestTimer")]
        public void TestTimer()
        {
            Debug.Log("---------TestTimer " + Time.realtimeSinceStartup);
            TimerManager.Instance.StartTimer(5, () =>
            {
                Debug.Log("Timer call " + Time.realtimeSinceStartup);
            },1, (times) =>
            {
                Debug.Log("Timer Step call  times: "  + times + "   "+ Time.realtimeSinceStartup);
            });
        }
        
        
        [ContextMenu("TestCoroutine")]
        public void TestCoroutine()
        {
            Debug.Log("---------TestCoroutine  frameCount:" + Time.frameCount);
            CoroutineManager.Instance.CreateTask(TestCoroutineFunc());
        }

        IEnumerator TestCoroutineFunc()
        {
            Debug.Log("---------TestCoroutineFunc  frameCount:" + Time.frameCount);
            yield return null;
            Debug.Log("---------TestCoroutineFunc 1  frameCount:" + Time.frameCount);
            yield return new WaitForSeconds(1);
            Debug.Log("---------TestCoroutineFunc 2  frameCount:" + Time.frameCount);
        }
    }
}