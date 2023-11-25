using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleClock : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject IndicatorTemplate;

    public GameObject HourHand;
    public GameObject MinuteHand;
    public GameObject SecondHand;

    public float indicatorLen = 5.2f;
    
    void Awake()
    {
        //创建12个刻度
        for (int i = 0; i < 12; i++)
        {
            GameObject indicator = GameObject.Instantiate(IndicatorTemplate,this.transform);
            var rot =  Quaternion.Euler(0,0,-30 * i).normalized;
            var pos = Vector3.zero + rot * Vector3.up * indicatorLen;
            indicator.transform.localPosition = new Vector3(pos.x,pos.y,-0.1f);
            indicator.transform.localRotation = rot;
        }
    }

    
    IEnumerator Start()
    {
        while(true){
            CalcuTime();
            yield return new WaitForSeconds(1);
        }
    }

    void CalcuTime()
    {
        //计算过去了多少小时
        DateTime dateTime = DateTime.Now;
        var totoalSeconds = dateTime.TimeOfDay.TotalSeconds;
        var hour = totoalSeconds / 3600;
        hour = hour > 12 ? hour - 12 : hour;
        HourHand.transform.localRotation =  Quaternion.Euler(0,0,-30 * (float)hour);
        
        //计算剩余多少分钟
        var minutes = totoalSeconds % 3600 / 60;
        MinuteHand.transform.localRotation =  Quaternion.Euler(0,0,-6 * (float)minutes);

        //计算剩余多少秒
        var seconds = totoalSeconds % 3600 % 60;
        SecondHand.transform.localRotation =  Quaternion.Euler(0,0,-6 * (float)seconds);

    }
}
