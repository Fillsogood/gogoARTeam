using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.XR.ARCore;
public class Control : MonoBehaviour
{
    
   public void On_Ar()
    {
       GameObject Map = GameObject.Find("Canvas").transform.Find("MiniMap").gameObject;
        GameObject scroll = GameObject.Find("Canvas").transform.Find("Scrollbar").gameObject;
        Transform On = GameObject.Find("Canvas").transform.Find("Switch_on_btn");
        Transform Off = GameObject.Find("Canvas").transform.Find("Switch_off_btn");
      
        Transform model = GameObject.Find(SingletonModelIdx.instance.ModelIdx+"(Clone)").transform;
        Transform[] children = new Transform[model.childCount];
        for(int i=0;i<model.childCount;i++)
        {
            children[i]=model.GetChild(i);
            children[i].gameObject.SetActive(false);
        }
        Map.SetActive(false);
        scroll.SetActive(false);
        On.gameObject.SetActive(false);
        Off.gameObject.SetActive(true);
    }
    public void Off_Ar()
    {
       GameObject Map = GameObject.Find("Canvas").transform.Find("MiniMap").gameObject;
        GameObject scroll = GameObject.Find("Canvas").transform.Find("Scrollbar").gameObject;
        Transform On = GameObject.Find("Canvas").transform.Find("Switch_on_btn");
        Transform Off = GameObject.Find("Canvas").transform.Find("Switch_off_btn");
        Transform model = GameObject.Find(SingletonModelIdx.instance.ModelIdx+"(Clone)").transform;
        Transform[] children = new Transform[model.childCount];
        for(int i=0;i<model.childCount;i++)
        {
            children[i]=model.GetChild(i);
            children[i].gameObject.SetActive(true);
        }
        Map.SetActive(true);
        scroll.SetActive(true);
        Debug.Log(On);
        On.gameObject.SetActive(true);
        Debug.Log(Off);
        Off.gameObject.SetActive(false);   
    }

    
}
