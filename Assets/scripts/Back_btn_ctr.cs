using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Back_btn_ctr : MonoBehaviour
{
    void Start()
    {
        int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];

        for(int i =0; i< countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
        }
        SceneManager.SetActiveScene(loadedScenes[1]);
        
    }

    public void Back_QRcode()
    {
        //SceneManager.LoadScene("QRScanScene");
        SceneManager.UnloadScene("Test");
        
        //SceneManager.Un
    }
}
