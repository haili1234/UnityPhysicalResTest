using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RandomPointMove : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    float LeftPos = -600f;  //左边界
    float RightPos = 600f;  //右边界
    float DownPos = 0f;     //下边界
    float UpPos = 300f;     //上边界

    Vector3 RandomPos;    //随机点
    public GameObject FZ; //随即移动的物体对象：风筝(风筝在UGUI里被创建，是一个Image图片)
    float speed=100f;     //风筝移动的速度
    float TimeLength;     //风筝移动的时间间隔

    float r = 200f;      //圆的半径
    List<Vector2> myList;//存储圆上所有的点的坐标的集合
    float x0, y0;        //圆心坐标
    Vector2 item;        //表示圆上的任意一点
    int index;           //表示集合中的索引，任意一点

    void Start()
    {
        StartCoroutine(RandomCoordinate()); //开启协同
        myList = new List<Vector2>();       //初始化集合
    }
    IEnumerator RandomCoordinate()
    {
        RandomPos = new Vector3(Random.Range(LeftPos, RightPos), Random.Range(DownPos, UpPos), 0f);          //生成随机点
        TimeLength = Vector3.Distance(FZ.GetComponent<RectTransform>().localPosition, RandomPos) / (speed);  //TimeLength=距离除以速度=应该移动的时间
        yield return new WaitForSeconds(TimeLength); //等待本次移动完毕/结束后再开启下一次协同/生成下一个随机点
        StartCoroutine(RandomCoordinate());    
    }
    void Update()
    {
        //以固定的速度移向随机点
        FZ.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(FZ.GetComponent<RectTransform>().localPosition, RandomPos, speed * Time.deltaTime);      
    }
    public void OnPointerDown(PointerEventData eventData)  //鼠标点击到物体对象上面
    {        
        StopAllCoroutines();  //停止所有协同
        speed = 400f;         //速度变快
        
        x0 = FZ.GetComponent<RectTransform>().localPosition.x;  //以风筝此时的位置为圆心x
        y0 = FZ.GetComponent<RectTransform>().localPosition.y;  //以风筝此时的位置为圆心y

        for (int i = 0; i < 360; i++)  //i为角度,每一度生成一个坐标点
        {
            //= x0 + r * Mathf.Cos(i * Mathf.PI / 180); //求圆上一点X坐标公式
            //= y0 + r * Mathf.Sin(i * Mathf.PI / 180); //求圆上一点Y坐标公式
            item = new Vector2(x0 + r * Mathf.Cos(i * Mathf.PI / 180), y0 + r * Mathf.Sin(i * Mathf.PI / 180));//item表示在圆上的360个点坐标             
            
            if (item.x <= RightPos && item.x >= LeftPos && item.y <= UpPos && item.y >=DownPos) //item是否在边界内
            {
                myList.Add(item); //将每一个在边界内的点添加到集合中(边界外的点不要添加到集合中)
            }
        }
        index = Random.Range(0, myList.Count);//随机圆上的某一个索引，myList.Count表示集合的长度
        RandomPos = myList[index];            //将圆上的某一点赋给随机点
        TimeLength = r / (speed);             //半径除以速度=需要移动的时间=需要延迟的时间
        Invoke("wait", TimeLength);           //延迟时间, TimeLength时间后，速度恢复，再次开启协同
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        myList.Clear();     //清空集合,以免下次按下的时候累加
    }
    void wait()
    { 
        CancelInvoke("wait");              //取消延迟
        speed = 100f;                      //速度恢复
        StartCoroutine(RandomCoordinate());//开启协同
    }
}