using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpasuleIni : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        Transform points = GameObject.Find("StartCapule").GetComponent<Transform>();
        GameObject Capsule= Resources.Load<GameObject>("DefectPrefab/Capsule");  
        GameObject Instance2 = (GameObject) Instantiate(Capsule, points.position, points.rotation );
    }

}
