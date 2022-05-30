using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

using Newtonsoft.Json; 
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
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
public class InsResponse
{
    public string api_result;
    public string err_message;
    public List<InspectionDto> data = new List<InspectionDto>();
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

[System.Serializable]
public class InspectionDto
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
    public string ad_image_name;
	public string ad_image_url;
	public string ad_image_size;
	public string ad_image_type;
    public string damage_name;
    public string object_name;
    public string state_name;
	public byte[] ins_bytes; 
	public byte[] ad_bytes;
}

public class SIMS_Demo : MonoBehaviour
{
    //private string serverPath = "http://localhost:8080";
    private string serverPath = "http://112.157.106.246:8080";

    private string serverPort = "8080";

    private Inspection _Ins = new Inspection();
    private Model _model = new Model();

    public static string strToEdit = "";

    private bool isConsoleShowing = true;

    private float ta_left;
    private float ta_top;
    private float ta_width;
    private float ta_height;

    private int listCount;
    

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine("CheckPermissionAndroid");
        }
        SimsLog(Application.persistentDataPath);
    }

    private void UpdateServerIpPort()
    {
        //string ip = "localhost";
        string ip = "112.157.106.246";
        string port = "8080";

        if (ip == "" || port == "")
        {
            Debug.Log("IP 및 Port 입력하세요. 서버와 통신을 할 수가 있습니다.");
            SimsLog("IP 및 Port 입력하세요. 서버와 통신을 할 수가 있습니다.");
        }
        else
        {
            serverPath = "http://" + ip + ":" + port;
            Debug.Log(serverPath);
            SimsLog(serverPath);
        }
    }

     private void SimsLog(string text)
    {
       // GameObject.Find("fasd").GetComponent<Text>().text += text + "\n";
       // strToEdit=;
    }


    

    private void UpdateDataInspection()
    {
        UpdateServerIpPort();

        _Ins.model_idx = SingletonModelIdx.instance.ModelIdx;
        _Ins.inspector_name = GameObject.Find("ifInsInspector").GetComponent<InputField>().text.ToString();
        _Ins.inspector_etc = GameObject.Find("ifinspector_etc").GetComponent<InputField>().text.ToString();
        GameObject Capsule = GameObject.Find("Capsule");
        Vector3 pos = Capsule.transform.position;
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
            _Ins.damage_loc_x = pos.x;
        }
        catch (FormatException)
        {
            _Ins.damage_loc_x = -1;
        }
        try
        {
            _Ins.damage_loc_y = pos.y;
        }
        catch (FormatException)
        {
            _Ins.damage_loc_y = -1;
        }
        try
        {
            _Ins.damage_loc_z = pos.z;
        }
        catch (FormatException)
        {
            _Ins.damage_loc_z = -1;
        }

        //_Ins.ins_image_name = GameObject.Find("ifPicturePath").GetComponent<InputField>().text.ToString();
        _Ins.ins_image_name = GameObject.Find("Canvas").transform.Find("panel_Inspection").transform.Find("txtImagepath").GetComponent<Text>().text;

        //Debug.Log("Inspection DB : " + _Ins.idx.ToString() + "/" + _Ins.ins_date + "/" + _Ins.inspector_name + "/" + _Ins.damage_type.ToString() + "/" + _Ins.damage_object + "/" + _Ins.damage_loc_x.ToString() + "/" + _Ins.damage_loc_y.ToString() + "/" + _Ins.damage_loc_z.ToString() + "/" + _Ins.ins_image_name);
    }

    private void UpdateDataForm()
    {
        GameObject.Find("ifInsInspector").GetComponent<InputField>().text = _Ins.inspector_name;
        GameObject.Find("ifinspector_etc").GetComponent<InputField>().text = _Ins.inspector_etc;
        GameObject.Find("DdDamageType").GetComponent<Dropdown>().value = _Ins.damage_type-1;
        GameObject.Find("DdDamageObject").GetComponent<Dropdown>().value = _Ins.damage_object-1;
        //GameObject.Find("ifPicturePath").GetComponent<InputField>().text = _Ins.ins_image_name;
    }

   
    public void OnClick_InsInsert()
    {
        UpdateDataInspection();
        SimsLog("OnClick_InsInsert()");
        StartCoroutine(PostFormDataImage("inspection", "insert", _Ins.ins_image_name));
    }

    public void OnClick_InsSelectList()
    {
        On_List();
        UpdateServerIpPort();
        // var json = JsonConvert.SerializeObject(new Inspection(SingletonModelIdx.instance.ModelIdx));
        InsModelIdx("inspection/select_modelidx"); 
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

                 SimsLog("저장소 권한이 필요함.");
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

    //�̹����� Inspection ����
    private IEnumerator PostFormDataImage(string uri, string id, string path_image)
    {
        var url = string.Format("{0}/{1}/{2}", serverPath, uri, id);
        Debug.Log(url);
        SimsLog(url);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        
        if (_Ins.model_idx > -1)
        {
            
            formData.Add(new MultipartFormDataSection("model_idx", _Ins.model_idx.ToString()));
        }
        else
        {
            Debug.Log("점검 ID을 입력하시기 바랍니다. 다시 확인바랍니다.!");
            SimsLog("점검 ID을 입력하시기 바랍니다. 다시 확인바랍니다.!");
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
             SimsLog("jpg, png 파일만 전송이 가능합니다. 다시 확인바랍니다.!");
            Debug.Log("jpg, png 파일만 전송이 가능합니다. 다시 확인바랍니다.!");
            yield break;
        } 
        formData.Add(new MultipartFormFileSection("file", img, Path.GetFileName(path_image), strImgformat));

        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        //www.SetRequestHeader("Content-Type", "multipart/form-data");

        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success)
        {
             SimsLog("점검 ID " + _Ins.idx.ToString() + "이 전송이 실패했습니다. " + www.responseCode);
             SimsLog(www.error);
            Debug.Log("점검 ID " + _Ins.idx.ToString() + "이 전송이 실패했습니다. " + www.responseCode);
            Debug.Log(www.error);
        }
        else
        {
            if (id == "")
            {
                 SimsLog("점검 ID " + _Ins.idx.ToString() + "가 성공적으로 Upload(삽입) 되었습니다. " + www.responseCode);
                Debug.Log("점검 ID " + _Ins.idx.ToString() + "가 성공적으로 Upload(삽입) 되었습니다. " + www.responseCode);
            }
            else
            {
                 SimsLog("점검 ID " + _Ins.idx.ToString() + "가 성공적으로 업데이트가 되었습니다. " + www.responseCode);
                Debug.Log("점검 ID " + _Ins.idx.ToString() + "가 성공적으로 업데이트가 되었습니다. " + www.responseCode);
            }

            Debug.Log("Request Response: " + www.downloadHandler.text);
            SimsLog("Request Response: " + www.downloadHandler.text);

            //업로드가 완료되면 폼을 클리어한다..          
        }
    }

    private void InsModelIdx(string uri)
    {
        var url = string.Format("{0}/{1}", serverPath, uri);
        string responseText = string.Empty;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Timeout = 30 * 10000; // 30초
        request.ContentType = "application/json; charset=utf-8";

        string postData ="{\"idx\" : "+SingletonModelIdx.instance.ModelIdx+"}";
        byte[] byteArray =Encoding.UTF8.GetBytes(postData);

        Stream dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
        {
            HttpStatusCode status = resp.StatusCode;
            if (status != HttpStatusCode.OK)
            {
                Debug.Log("모델 ID " + SingletonModelIdx.instance.ModelIdx + "이 조회에 실패했습니다. " + status);
           
            }
            Stream respStream = resp.GetResponseStream();
            using (StreamReader sr = new StreamReader(respStream))
            {
                responseText = sr.ReadToEnd();
            }
        }

        var jObject = JObject.Parse(responseText);
        string data =jObject.GetValue("data").ToString();
               
        InsResponse jObjText = (InsResponse) JsonConvert.DeserializeObject<InsResponse>(jObject.ToString());
        List<InspectionDto> list = new List<InspectionDto>(jObjText.data);

        GameObject Item = Resources.Load<GameObject>("Item_Panel");
        int yValue = 0;

        for(int i=0; i<list.Count; i++)
        {
            var index = Instantiate(Item, new Vector3(0, yValue, 0), Quaternion.identity);
            index.name = "item"+i;
            index.transform.SetParent(GameObject.Find("Content").transform);
            yValue -= 200;
        }
        listCount = list.Count;

        int count = 0;

        foreach(InspectionDto j in list)
        {
            //이미지 넣기
            byte[] newBytes22 = j.ins_bytes;

            MemoryStream ms = new MemoryStream(newBytes22);
            newBytes22 = ms.ToArray();

            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(newBytes22);
            
            GameObject imageObj = GameObject.Find("item"+count).transform.Find("Item_Image").gameObject;
            Image image = imageObj.GetComponent<Image>();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.0f), 1.0f);
            image.sprite = sprite; 

            //하자 리스트 정보값 출력
            GameObject.Find("item"+count).transform.Find("ItemInsType_Text").GetComponent<Text>().text = j.damage_name;

            GameObject.Find("item"+count).transform.Find("ItemInsDate_Text").GetComponent<Text>().text = "날짜 : "+j.ins_date; 

            GameObject.Find("item"+count).transform.Find("ItemInsInspector_Text").GetComponent<Text>().text ="점검자 : "+j.inspector_name; 

            GameObject.Find("item"+count).transform.Find("ItemInsLoc_Text").GetComponent<Text>().text ="하자 위치 : "+j.damage_loc_x+" / "+j.damage_loc_y+" / "+j.damage_loc_z; 

            GameObject.Find("item"+count).transform.Find("ItemInsETC_Text").GetComponent<Text>().text ="기타사항 : "+j.inspector_etc;

            count++;
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

    private void On_List()
    {
        Transform On_Panel = GameObject.Find("Canvas").transform.Find("List_Panel");
        On_Panel.gameObject.SetActive(true);
        Transform On_btn = GameObject.Find("Canvas").transform.Find("List_on_btn");
        On_btn.gameObject.SetActive(false);
        Transform Off_btn = GameObject.Find("Canvas").transform.Find("List_off_btn");
        Off_btn.gameObject.SetActive(true);

    }

     public void Off_List()
    {
        int count = 0;
        for(int i=0; i<listCount; i++)
        {
            Destroy(GameObject.Find("item"+count));
            count++;
        }
        Transform On_Panel = GameObject.Find("Canvas").transform.Find("List_Panel");
        On_Panel.gameObject.SetActive(false);
        Transform On_btn = GameObject.Find("Canvas").transform.Find("List_on_btn");
        On_btn.gameObject.SetActive(true);
        Transform Off_btn = GameObject.Find("Canvas").transform.Find("List_off_btn");
        Off_btn.gameObject.SetActive(false);
    }
}

