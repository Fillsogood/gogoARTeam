using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GyroScope : MonoBehaviour
{
    GameObject Capsule;
    Vector3 forceVec;
    Rigidbody rb;
    Text x;
    Text y;
    float speed=0.05f; 
    bool isBorder;
    bool isTri;
    Transform xText;
    Transform yText;
    Transform panel;
    float rotLeftRight, rotTopBottom;
    Vector3 m_PlayerRot;
    private float TimeLeft = 1.0f;
    private float nextTime = 0.0f;
    private float time;
    List<float> data = new List<float>();

      void Start()
    {
        Capsule =new GameObject("Capsule");
        Capsule.transform.position =this.transform.position;
        this.transform.parent = Capsule.transform;       
        Input.gyro.enabled=true;
        xText = GameObject.Find("Canvas").transform.Find("XText");
        x=xText.GetComponent<Text>();
        yText = GameObject.Find("Canvas").transform.Find("YText");
        y=yText.GetComponent<Text>();
       //GameObject.Find("Canvas").transform.Find("XText").GetComponent<Text>().text = " X : " + pos.x;   
        
    }

    protected void Update()
    {   
        Vector3 pos;
        pos = this.gameObject.transform.position;
        x.text=" X:"+(int)(pos.x);
        y.text="Y:"+(int)(pos.y);
        gyroupdate();

    }

    void FixedUpdate()
    {
        StopToWall();
        AddRigidbody();

        //1초마다 실행
        // if(Time.time > nextTime){
        //     nextTime = Time.time + TimeLeft;
        //     // MoveMoles();
        //     Debug.Log(average());
        //     // Debug.Log(m_PlayerRot.y);
        // }

        //Debug.Log("Y : " + Input.gyro.userAcceleration.y);
        //Debug.Log("z:"+Input.gyro.rotationRate.z);

        if(average()>=0.0612304173409939 && average()<=0.09&&!isBorder)
        {
            //이동
            Capsule.transform.Translate(-speed,0,0); 
        }
        else if(average()>=-0.00394487800076604||average()<=0.00479192985221744 &&!isBorder)
        {
           //멈추고 돌때...
        }      

        if(isTri)
        {
            panel = GameObject.Find("Canvas").transform.Find("DefectUpdateBtn");
            panel.gameObject.SetActive(true);
            if(Capsule.GetComponent<Rigidbody>()==null)
            {
                 Capsule.AddComponent<Rigidbody>();   
            }
            else
            {
                 rb = Capsule.GetComponent<Rigidbody>();
                 rb.freezeRotation =true;
            }
        }
        else
        {
            panel = GameObject.Find("Canvas").transform.Find("DefectUpdateBtn");
            Destroy(rb);
           panel.gameObject.SetActive(false);

        }
    }

      void MoveMoles()
    {
        Debug.Log(data.Count);
        data.Clear();
    }
    double average()
    {
        double[] arr = new double[98]; //가속도 센서 1초 평균 98개의 데이터 추출
        double result = 0;

        for(int i=0;i<arr.Length;i++)
        {
            arr[i] = Input.gyro.userAcceleration.y; 
        }
        for(int i=0;i<arr.Length;i++)
        {
            result += arr[i];
        }
        return result /= arr.Length;
    }

    double average2()
    {
        double[] arr = new double[65]; //자이로 센서 1초 평균 65개의 데이터 추출
        double result = 0;

        for(int i=0;i<arr.Length;i++)
        {
            arr[i] = m_PlayerRot.y; 
        }
        for(int i=0;i<arr.Length;i++)
        {
            result += arr[i];
        }
        return result /= arr.Length;
    }

     void gyroupdate()
	{   
       
        m_PlayerRot.y -= Input.gyro.rotationRate.z*1.0f;
		Capsule.transform.eulerAngles = m_PlayerRot;
        
		
	}

    void StopToWall()
    {
        Debug.DrawRay(transform.position,-transform.right*0.5f,Color.green);
        isBorder = Physics.Raycast(transform.position,-transform.right,0.5f,LayerMask.GetMask("wall"));
    }

    void AddRigidbody()
    {
         Debug.DrawRay(transform.position,-transform.right*0.8f,Color.red);
         isTri = Physics.Raycast(transform.position,-transform.right,0.8f,LayerMask.GetMask("rb"));
    }
}