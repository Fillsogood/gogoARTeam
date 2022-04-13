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
    float speed=0.01f; 
    bool isBorder;
    bool isTri;
    Transform xText;
    Transform yText;
    Transform panel;
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
        
        //Vector3 gyroEuler = Input.gyro.attitude.eulerAngles;
        Capsule.transform.Rotate(0,-Input.gyro.rotationRate.z*1.5f,0);
        //Capsule.transform.rotation=new Quaternion(0,Input.gyro.attitude.y*0.5f,0,-Input.gyro.attitude.w*0.5f);
        
     
    }
    void FixedUpdate()
    {
        StropToWall();
        AddRigidbody();
           if(Input.gyro.userAcceleration.y>0.2&&!isBorder)
        {
            Capsule.transform.Translate(-speed,0,0);
           
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
    void StropToWall()
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
