using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour
{
    public string model_name { get; set; }

    public void On_Ar()
    {
        GameObject Map = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("MiniMap").gameObject;
        GameObject scroll = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("Scrollbar").gameObject;
        Transform On = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("Switch_on_btn");
        Transform Off = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("Switch_off_btn");
        Transform MapDefectBtn = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("MapDefectBtn");

        Transform model = GameObject.Find(model_name + "(Clone)").transform;
        Transform[] children = new Transform[model.childCount];

        for(int i=0; i<model.childCount; i++)
        {
            children[i]=model.GetChild(i);
            children[i].gameObject.SetActive(false);
        }

        Map.SetActive(false);
        scroll.SetActive(false);
        On.gameObject.SetActive(false);
        Off.gameObject.SetActive(true);
        MapDefectBtn.gameObject.SetActive(false);
    }

    public void Off_Ar()
    {
        GameObject Map = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("MiniMap").gameObject;
        GameObject scroll = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("Scrollbar").gameObject;
        Transform On = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("Switch_on_btn");
        Transform Off = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("Switch_off_btn");
        Transform MapDefectBtn = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("MapDefectBtn");

        Transform model = GameObject.Find(model_name + "(Clone)").transform;
        Transform[] children = new Transform[model.childCount];

        for(int i=0; i<model.childCount; i++)
        {
            children[i]=model.GetChild(i);
            children[i].gameObject.SetActive(true);
        }

        Map.SetActive(true);
        scroll.SetActive(true);
        On.gameObject.SetActive(true);
        Off.gameObject.SetActive(false);   
        MapDefectBtn.gameObject.SetActive(true);
    }
}
