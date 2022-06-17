using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GyroScopeCtr : MonoBehaviour
{
    private GameObject Capsule;
    private Transform xText;
    private Transform yText;

    private Text x;
    private Text y;
    private Text ARText;

    private Rigidbody rb;

    private Vector3 forceVec;
    private Vector3 m_PlayerRot;

    private float speed = 0.15f; 
    private static double GyroRotY;
    
    private bool isBorder;
    private bool isTri;

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

        ARText = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("CreateFlowText").GetComponent<Text>();
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
        //AR 앵커 저장 전, 저장 후에만 움직임
        if(ARText.text == "Next: Please Touch the Save Defect Button" || ARText.text == "Next: Touch the Save Defect Button to continue saving")
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
        }
        else
        {
            //AR 애져앵커 저장 중에는 캡슐이 움직이지 않음
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
