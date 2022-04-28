﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//script to make secondary camera follow user.
public class FollowUser : MonoBehaviour
{  
    GameObject Target; // Capsule 게임오브젝트 가져오기
    public Transform TargetCapsule;
    public Transform Navigation;

    void Start()
    {
       
    }
    void Update()
    {
		if(TargetCapsule==null)
		{
			Target = GameObject.Find("Capsule");

			TargetCapsule = Target.transform;
		}
		else
		{
			
		}
    }

    private void LateUpdate()
    {     
		Navigation.position =new Vector3(TargetCapsule.position.x,TargetCapsule.position.y+20f,TargetCapsule.position.z);
		Navigation.LookAt(TargetCapsule);
    }

}
