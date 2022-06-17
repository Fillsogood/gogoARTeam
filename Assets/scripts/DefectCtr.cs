
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DefectCtr : MonoBehaviour
{
    public Transform panel_Inspection;
    public Text txtGyroValue;
    public InputField ifInsInspector;
    public TMP_InputField ifinspector_etc;
    public Dropdown DdSpace;
    public Dropdown DdDamageObject;
    public Dropdown DdDamageType;

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
        panel_Inspection.gameObject.SetActive(true);
        txtGyroValue.text = GyroScopeCtr.GetGyroData();
    }
    
    public void OnSave()
    {
        ClearDataInspection();
        panel_Inspection.gameObject.SetActive(false);
        SaveDefect();
    }

    public void ClearDataInspection()
    {
        ifInsInspector.text = "";
        ifinspector_etc.text ="";
        DdSpace.value = 0;
        DdDamageObject.value = 0;
        DdDamageType.value = 0;
    }
}