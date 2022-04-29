
using UnityEngine;
using UnityEngine.UI;
using System;
public class DefectCtr : MonoBehaviour
{
    void Start()
    {
        
    }

    void SaveDefect()
    {
        GameObject Capsule = GameObject.Find("Capsule");
      
        GameObject tmp= Resources.Load<GameObject>("DefectPrefab/Defect");
      
        GameObject Defect = Instantiate(tmp);

        Defect.transform.position = Capsule.transform.position;
      
    }

    public void OnPanel()
    {         
        Transform DefectSaveBtn = GameObject.Find("Canvas").transform.Find("panel_Inspection");
        DefectSaveBtn.gameObject.SetActive(true);
        
    }
    
    public void OnSave()
    {
        ClearDataInspection();
        Transform DefectSaveBtn = GameObject.Find("Canvas").transform.Find("panel_Inspection");
        DefectSaveBtn.gameObject.SetActive(false);
        SaveDefect();
    }
     public void ClearDataInspection()
    {
        GameObject.Find("ifInsInspector").GetComponent<InputField>().text = "";
        GameObject.Find("ifinspector_etc").GetComponent<InputField>().text ="";
        GameObject.Find("DdDamageType").GetComponent<Dropdown>().value = 0;
        GameObject.Find("DdDamageObject").GetComponent<Dropdown>().value = 0;
        //GameObject.Find("ifPicturePath").GetComponent<InputField>().text = "";
    }


}
