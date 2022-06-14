using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GyroScopeCtr : MonoBehaviour
{
    GameObject Capsule;
    Vector3 forceVec;
    Rigidbody rb;
    Text x;
    Text y;
    float speed=0.15f; 
    bool isBorder;
    bool isTri;
    Transform xText;
    Transform yText;
    Transform panel;
    Vector3 m_PlayerRot;
    static double GyroRotY;

    void Start()
    {
        Capsule =new GameObject("Capsule");
        Capsule.transform.position =this.transform.position;
        this.transform.parent = Capsule.transform;       
        Input.gyro.enabled=true;
        xText = GameObject.Find("MobileUX").transform.Find("XText");
        x=xText.GetComponent<Text>();
        yText = GameObject.Find("MobileUX").transform.Find("YText");
        y=yText.GetComponent<Text>();
    }

    protected void Update()
    {   
        if(SceneManager.GetActiveScene().name == "Test")
        {
            Vector3 pos;
            pos = this.gameObject.transform.position;
            x.text=" X:"+pos.x;
            y.text="Y:"+pos.z;
            gyroupdate(); 
        }
    }

    public static string GetGyroData()
    {
        return GyroRotY.ToString();
    }

    void FixedUpdate()
    {
        AddRigidbody();
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
    }
    
    public static double average()
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

    void gyroupdate()
	{
        m_PlayerRot.y -= Input.gyro.rotationRate.z*1.3f;
		Capsule.transform.eulerAngles = m_PlayerRot;
        GyroRotY = m_PlayerRot.y;
	}


    void AddRigidbody()
    {
        Debug.DrawRay(transform.position,-transform.right*0.8f,Color.red);
        isTri = Physics.Raycast(transform.position,-transform.right,0.8f,LayerMask.GetMask("rb"));
    }
}
