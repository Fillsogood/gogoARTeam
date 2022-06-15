
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class DefectCtr : MonoBehaviour
{
    void Start()
    {
        Input.gyro.enabled=true;
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
        Transform DefectSaveBtn = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("panel_Inspection");
        DefectSaveBtn.gameObject.SetActive(true);
        GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("panel_Inspection").transform.Find("txtGyroValue").GetComponent<Text>().text=GyroScopeCtr.GetGyroData();
    }
    
    public void OnSave()
    {
        ClearDataInspection();
        Transform DefectSaveBtn = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("panel_Inspection");
        DefectSaveBtn.gameObject.SetActive(false);
        SaveDefect();

        // GameObject ARStateText = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("CreateFlowText").gameObject;
        // GameObject ARButton = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("CreateFlowButton").gameObject;
        // GameObject DefectBtn = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("DefectBtn").gameObject;
        // ARStateText.SetActive(true);
        // ARButton.SetActive(true);
        // DefectBtn.SetActive(false);
       
       
    }
     public void ClearDataInspection()
    {
        GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("panel_Inspection").transform.Find("ifInsInspector").GetComponent<InputField>().text = "";
        GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("panel_Inspection").transform.Find("ifinspector_etc").GetComponent<TMP_InputField>().text ="";
        GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("panel_Inspection").transform.Find("DdDamageType").GetComponent<Dropdown>().value = 0;
        GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("panel_Inspection").transform.Find("DdDamageObject").GetComponent<Dropdown>().value = 0;
        //GameObject.Find("ifPicturePath").GetComponent<InputField>().text = "";
    }


}
