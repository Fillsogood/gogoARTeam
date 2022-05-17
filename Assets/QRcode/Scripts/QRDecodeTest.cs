using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TBEasyWebCam;

public class QRDecodeTest : MonoBehaviour
{
	public QRCodeDecodeController e_qrController;

	//public Text UiText;

	public GameObject resetBtn;

	public GameObject scanLineObj;
    
	public Image torchImage;
	/// <summary>
	/// when you set the var is true,if the result of the decode is web url,it will open with browser.
	/// </summary>
	public bool isOpenBrowserIfUrl;

	private bool isPlaying;

	private void Start()
	{
		
	}

	private void Update()
	{
		
		if(SceneManager.GetActiveScene().name == "Test")
		{
			//Debug.Log(isPlaying);
			if(isPlaying != true)
			{
				//Debug.Log("Playing");
				Play();
			}
			
			//Debug.Log("dasd");
			//Debug.Log(GameObject.Find("startButton"));
			GameObject.Find("startButton").GetComponent<Button>().interactable = false;
		}
	}

	public void qrScanFinished(string dataText)
	{
        Debug.Log(dataText);
		Debug.Log(isOpenBrowserIfUrl);
		
		if (isOpenBrowserIfUrl) {
			if (Utility.CheckIsUrlFormat(dataText))
			{
				if (!dataText.Contains("http://") && !dataText.Contains("https://"))
				{
					//dataText = "http://" + dataText;

					var msg = dataText.Split('/');
					dataText= msg[2];
				}
				dataText = "11"; //   <================================================================= 여기에 마커 모델 숫자 들어와야함 숫자만
				SingletonModelIdx.instance.ModelIdx =Convert.ToInt32(dataText);
				GameObject.Find("startButton").GetComponent<Button>().interactable = true;
				
				//Application.OpenURL(dataText);
				//GotoNextScene("Test");
				
			}
		}
		//this.UiText.text = dataText;
		if (this.resetBtn != null)
		{
			this.resetBtn.SetActive(true);
		}
		if (this.scanLineObj != null)
		{
			//this.scanLineObj.SetActive(false);
		}

	}

	public void Reset()
	{
		if (this.e_qrController != null)
		{
			this.e_qrController.Reset();
		}

		// if (this.UiText != null)
		// {
		// 	this.UiText.text = string.Empty;
		// }
		if (this.resetBtn != null)
		{
			this.resetBtn.SetActive(false);
		}
		if (this.scanLineObj != null)
		{
			this.scanLineObj.SetActive(true);
		}
	}

	public void Play()
	{
		Reset ();
		if (this.e_qrController != null)
		{
			isPlaying = true;
			this.e_qrController.StartWork();
		}
	}

	public void Stop()
	{
		isPlaying = false;
		if (this.e_qrController != null)
		{
			this.e_qrController.StopWork();
		}

		if (this.resetBtn != null)
		{
			this.resetBtn.SetActive(false);
		}
		if (this.scanLineObj != null)
		{
			this.scanLineObj.SetActive(false);
		}
	}

	public void GotoNextScene()
	{
		
		//this.e_qrController.StopWork();
		// if (this.e_qrController != null)
		// {
		// 	this.e_qrController.StopWork();
		// }
		//Application.LoadLevel(scenename);
		SceneManager.LoadScene("Test",LoadSceneMode.Additive);
		Stop();
	}
    

}
