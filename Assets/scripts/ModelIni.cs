﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelIni : MonoBehaviour
{ 
    //string ModelName;
     void Start()
     {
        Transform points = GameObject.Find("StartPoint").GetComponent<Transform>();     
        //GameObject Building= Resources.Load<GameObject>("BuildingPrefab/BuildingStructure");
        
        GameObject Building= Resources.Load<GameObject>("BuildingPrefab/GNU-LOD300");
        //GameObject Building= Resources.Load<GameObject>("BuildingPrefab/"+ModelName);
       //ChangeLayersRecursively( Building.transform, "wall"); 
        GameObject Instance = (GameObject) Instantiate(Building, points.position, points.rotation );   
         
     }
    public static void ChangeLayersRecursively(Transform trans, string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform child in trans)
        {
            ChangeLayersRecursively(child, name);
        }
    }
}
