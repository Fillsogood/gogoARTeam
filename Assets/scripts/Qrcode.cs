using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using TMPro;
using UnityEngine.UI;

public class Qrcode : MonoBehaviour
{
    [SerializeField]
    private RawImage _rawImageBackground;
    [SerializeField]
    private AspectRatioFitter _aspectRatioFitter;
    [SerializeField]
    private TextMeshProUGUI _textOut;
    [SerializeField]
    private RectTransform _scanZone;

    private bool _isCamAvbaible;
    private WebCamTexture _cameratexture;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SetUpCamera()
    {
        WebCamDevice [] devices = WebCamTexture.devices;
        if(devices.Length==0)
        {
            _isCamAvbaible = false;
            return;
        }
        for(int i =0;i<devices.Length;i++)
        {
            if(devices[i].isFrontFacing==false)
            {
                _cameratexture = new WebCamTexture(devices[i].name,(int)_scanZone.rect.width,(int)_scanZone.rect.height);
            }
        }
        _cameratexture.Play();
        _rawImageBackground.texture = _cameratexture;
        _isCamAvbaible = true;
    }
 private void UpdateCameraRender()
 {
     if(_isCamAvbaible==false)
    {
        return;
    }
    float ratio = (float)_cameratexture.width/(float)_cameratexture.height;
    _aspectRatioFitter.aspectRatio=ratio;
    int orientation = -_cameratexture.videoRotationAngle;
    _rawImageBackground.rectTransform.localEulerAngles =new Vector3(0,0,orientation);
 }
 
    public void OnClickScan()
    {
        Scan();
    }
    private void Scan()
    {
        // try
        // {
        //     //IBarcodeReader barcodeReader = new BarcodeReader();
        //     //Result result = barcodeReader.Decode(_cameratexture.GetPixels32(),_cameratexture.width,_cameratexture.height);
        //    
        //     else{
        //         _textOut.text = "QR코드를 읽지 못 했습니다";
        //     }
        // }
        // catch
        // {
        //     _textOut.text ="실패";
        // }
    }
}
