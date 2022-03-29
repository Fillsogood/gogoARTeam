using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//script to make secondary camera follow user.
public class FollowUser : MonoBehaviour
{
    public Transform userCamera;
    public Text feedbacktext;
    private void LateUpdate()
    {
        Vector3 newPosition = userCamera.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
        //  transform.eulerAngles = new Vector3(userCamera.rotation.x, 0, userCamera.rotation.z);
        Vector3 rot = userCamera.eulerAngles;
      //  feedbacktext.text = "rot x:" + rot.x + " ||y:" + rot.y + "||z:" + rot.z;
        transform.eulerAngles = new Vector3(90, 0,- rot.y);
    // transform.rotation = Quaternion.Euler(userCamera.rotation.x, userCamera.rotation.y, userCamera.rotation.z);
    }
}
