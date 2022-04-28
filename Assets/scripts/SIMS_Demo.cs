using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.Android;

[System.Serializable]
public class Model
{
    public int model_id;
    public string model_3dfile;
    public string model_2dfile;
}

[System.Serializable]
public class Inspection
{
	public int idx;
    public int model_idx;
    public string inspector_name;
    public string admin_name;
	public int damage_type;
	public int damage_object;
	public float damage_loc_x;
	public float damage_loc_y;
	public float damage_loc_z;
	public string ins_date;
    public string admin_date;
    public string inspector_etc;
    public string admin_etc;
    public int state;
	public string ins_image_name;
	public string ins_image_url;
	public string ins_image_size;
	public string ins_image_type;
	public byte[] ins_bytes; 
    public string ad_image_name;
	public string ad_image_url;
	public string ad_image_size;
	public string ad_image_type;
	public byte[] ad_bytes;

}

public class SIMS_Demo : MonoBehaviour
{
    //private string serverPath = "http://localhost:8080";
    private string serverPath = "http://14.7.197.230:8080";

    private string serverPort = "8080";

    private Inspection _Ins = new Inspection();
    private Model _model = new Model();

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine("CheckPermissionAndroid");
        }
    }

    private void UpdateServerIpPort()
    {
        string ip = "14.7.197.230";
        string port = "8080";

        if (ip == "" || port == "")
        {
            Debug.Log("IP 및 Port 입력하세요. 서버와 통신을 할 수가 있습니다.");
        }
        else
        {
            serverPath = "http://" + ip + ":" + port;
            Debug.Log(serverPath);
        }
    }

    private void UpdateDataInspection()
    {
        UpdateServerIpPort();

        _Ins.model_idx = SingletonModelIdx.instance.ModelIdx;
        _Ins.ins_date = GameObject.Find("ifInsDate").GetComponent<InputField>().text.ToString();
        _Ins.inspector_name = GameObject.Find("ifInsInspector").GetComponent<InputField>().text.ToString();
        _Ins.inspector_etc = GameObject.Find("ifinspector_etc").GetComponent<InputField>().text.ToString();
        
        try
        {
            _Ins.damage_type = (GameObject.Find("DdDamageType").GetComponent<Dropdown>().value)+1;
        }
        catch (FormatException)
        {
            _Ins.damage_type = -1;
        }
        try
        {
            _Ins.damage_object = (GameObject.Find("DdDamageObject").GetComponent<Dropdown>().value)+1;
        }
        catch (FormatException)
        {
             _Ins.damage_object = -1;
        }
        try
        {
            _Ins.damage_loc_x = Convert.ToSingle(GameObject.Find("ifDamageX").GetComponent<InputField>().text.ToString());
            Debug.Log(_Ins.damage_loc_x);
        }
        catch (FormatException)
        {
            _Ins.damage_loc_x = -1;
        }
        try
        {
            _Ins.damage_loc_y = Convert.ToSingle(GameObject.Find("ifDamageY").GetComponent<InputField>().text.ToString());
        }
        catch (FormatException)
        {
            _Ins.damage_loc_y = -1;
        }
        try
        {
            _Ins.damage_loc_z = Convert.ToSingle(GameObject.Find("ifDamageZ").GetComponent<InputField>().text.ToString());
        }
        catch (FormatException)
        {
            _Ins.damage_loc_z = -1;
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            _Ins.ins_image_name = "/storage/emulated/0/DCIM/" + GameObject.Find("ifPicturePath").GetComponent<InputField>().text.ToString();
        }
        else
        {
            _Ins.ins_image_name = GameObject.Find("ifPicturePath").GetComponent<InputField>().text.ToString();
        }

        //Debug.Log("Inspection DB : " + _Ins.idx.ToString() + "/" + _Ins.ins_date + "/" + _Ins.inspector_name + "/" + _Ins.damage_type.ToString() + "/" + _Ins.damage_object + "/" + _Ins.damage_loc_x.ToString() + "/" + _Ins.damage_loc_y.ToString() + "/" + _Ins.damage_loc_z.ToString() + "/" + _Ins.ins_image_name);
    }

    private void UpdateDataForm()
    {
        GameObject.Find("ifInsID").GetComponent<InputField>().text = _Ins.idx.ToString();
        GameObject.Find("ifInsDate").GetComponent<InputField>().text = _Ins.ins_date;
        GameObject.Find("ifInsInspector").GetComponent<InputField>().text = _Ins.inspector_name;
        GameObject.Find("ifinspector_etc").GetComponent<InputField>().text = _Ins.inspector_etc;
        GameObject.Find("DdDamageType").GetComponent<Dropdown>().value = _Ins.damage_type-1;
        GameObject.Find("DdDamageObject").GetComponent<Dropdown>().value = _Ins.damage_object-1;
        GameObject.Find("ifDamageX").GetComponent<InputField>().text = _Ins.damage_loc_x.ToString();
        GameObject.Find("ifDamageY").GetComponent<InputField>().text = _Ins.damage_loc_y.ToString();
        GameObject.Find("ifDamageZ").GetComponent<InputField>().text = _Ins.damage_loc_z.ToString();
        GameObject.Find("ifPicturePath").GetComponent<InputField>().text = _Ins.ins_image_name;
    }

    public void ClearDataInspection()
    {
        GameObject.Find("ifInsID").GetComponent<InputField>().text = "";
        GameObject.Find("ifInsDate").GetComponent<InputField>().text = "";
        GameObject.Find("ifInsInspector").GetComponent<InputField>().text = "";
         GameObject.Find("ifinspector_etc").GetComponent<InputField>().text ="";
        GameObject.Find("DdDamageType").GetComponent<Dropdown>().value = 0;
        GameObject.Find("DdDamageObject").GetComponent<Dropdown>().value = 0;
        GameObject.Find("ifDamageX").GetComponent<InputField>().text = "";
        GameObject.Find("ifDamageY").GetComponent<InputField>().text = "";
        GameObject.Find("ifDamageZ").GetComponent<InputField>().text = "";
        GameObject.Find("ifPicturePath").GetComponent<InputField>().text = "";
    }

    public void OnClick_InsInsert()
    {
        UpdateDataInspection();

        StartCoroutine(PostFormDataImage("inspection", "insert", _Ins.ins_image_name));
    }

    IEnumerator CheckPermissionAndroid()
    {
        yield return new WaitForEndOfFrame();

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            Permission.RequestUserPermission(Permission.ExternalStorageRead);

            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(() => Application.isFocused == true);

            if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
            {
                //다이얼로그를 위해 별도의 플러그인을 사용했었다. 이 코드는 주석 처리함.
                //AGAlertDialog.ShowMessageDialog("권한 필요", "스크린샷을 저장하기 위해 저장소 권한이 필요합니다.",
                //"Ok", () => OpenAppSetting(),
                //"No!", () => AGUIMisc.ShowToast("저장소 요청 거절됨"));

                // 별도로 확인 팝업을 띄우지 않을꺼면 OpenAppSetting()을 바로 호출함.
                //OpenAppSetting();
                Debug.Log("저장소 권한이 필요함.");
                yield break;
            }
        }
        //string fileLocation = "/storage/emulated/0" + "/DCIM/Screenshots/"; // "mnt/sdcard/DCIM/Screenshots/";
    }
    
    private void ViewImage(string component_name, byte[] image_bytes)
    {
        GameObject imageObj = GameObject.Find(component_name);

        Image image = imageObj.GetComponent<Image>();
        image.type = Image.Type.Simple;
        image.preserveAspect = true;

        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGB24, false);
        tex.filterMode = FilterMode.Trilinear;
        tex.LoadImage(image_bytes);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.0f), 1.0f);
        Debug.Log(tex.width + ", " + tex.height);

        //sprite.
        image.sprite = sprite;
    }

    //이미지와 Inspection 삽입
    private IEnumerator PostFormDataImage(string uri, string id, string path_image)
    {
        var url = string.Format("{0}/{1}/{2}", serverPath, uri, id);
        Debug.Log(url);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        if (_Ins.model_idx > -1)
        {
            formData.Add(new MultipartFormDataSection("model_idx", _Ins.model_idx.ToString()));
        }
        else
        {
            Debug.Log("점검 ID을 입력하시기 바랍니다. 다시 확인바랍니다.!!");

            yield break;;
        }

        formData.Add(new MultipartFormDataSection("inspector_name", _Ins.inspector_name != "" ? _Ins.inspector_name : "-1"));
        formData.Add(new MultipartFormDataSection("damage_type", _Ins.damage_type > -1 ? _Ins.damage_type.ToString() : "-1"));
        formData.Add(new MultipartFormDataSection("damage_object", _Ins.damage_object  > -1 ? _Ins.damage_object.ToString() : "-1"));
        formData.Add(new MultipartFormDataSection("damage_loc_x", _Ins.damage_loc_x > -1 ? _Ins.damage_loc_x.ToString() : "-1"));
        formData.Add(new MultipartFormDataSection("damage_loc_y", _Ins.damage_loc_y > -1 ? _Ins.damage_loc_y.ToString() : "-1"));
        formData.Add(new MultipartFormDataSection("damage_loc_z", _Ins.damage_loc_z > -1 ? _Ins.damage_loc_z.ToString() : "-1"));
        formData.Add(new MultipartFormDataSection("inspector_etc", _Ins.inspector_etc != "" ? _Ins.inspector_etc : "-1"));

        byte[] img = null;
        string strImgformat = "";
        if (Path.GetExtension(path_image) == ".jpg")
        {
            img = File.ReadAllBytes(path_image);
            strImgformat = "image/jpeg";
        }
        else if (Path.GetExtension(path_image) == ".png")
        {
            img = File.ReadAllBytes(path_image);
            strImgformat = "image/png";
        }
        else
        {
            Debug.Log("jpg, png 파일만 전송이 가능합니다. 다시 확인바랍니다.!!");
            yield break;
        }

        formData.Add(new MultipartFormFileSection("file", img, Path.GetFileName(path_image), strImgformat));
        
        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        //www.SetRequestHeader("Content-Type", "multipart/form-data");
        
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("점검 ID " + _Ins.idx.ToString() + "이 전송이 실패했습니다. " + www.responseCode);
            Debug.Log(www.error);
        }
        else
        {
            if (id == "")
            {
                Debug.Log("점검 ID " + _Ins.idx.ToString() + "가 성공적으로 Upload(삽입) 되었습니다. " + www.responseCode);
            }
            else
            {
                Debug.Log("점검 ID " + _Ins.idx.ToString() + "가 성공적으로 업데이트가 되었습니다. " + www.responseCode);
            }

            Debug.Log("Request Response: " + www.downloadHandler.text);

            //업로드가 완료되면 폼을 클리어한다.
            ClearDataInspection();
        }
    }
    
    public void OnQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void Back()
    {
       Transform back = GameObject.Find("Canvas").transform.Find("panel_Inspection");
        back.gameObject.SetActive(false);
    }
}

