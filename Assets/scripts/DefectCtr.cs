
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
        GameObject Capsule = GameObject.Find("Capsule");
        Transform DefectSaveBtn = GameObject.Find("Canvas").transform.Find("panel_Inspection");
        DefectSaveBtn.gameObject.SetActive(true);
            Vector3 pos = Capsule.transform.position;
            GameObject.Find("ifDamageX").GetComponent<InputField>().text = pos.x.ToString();
            GameObject.Find("ifDamageY").GetComponent<InputField>().text = pos.y.ToString();
            GameObject.Find("ifDamageZ").GetComponent<InputField>().text = pos.z.ToString();
            GameObject.Find("ifInsDate").GetComponent<InputField>().text = DateTime.Now.ToString("yyyy-MM-dd");
    }
    
    public void OnSave()
    {
        Transform DefectSaveBtn = GameObject.Find("Canvas").transform.Find("panel_Inspection");
        DefectSaveBtn.gameObject.SetActive(false);
        SaveDefect();
    }

}
