using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using Newtonsoft.Json; 
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.Android;
using Jacovone.AssetBundleMagic;
using System.Runtime.Serialization.Formatters.Binary;
using Dummiesman;

[System.Serializable]
public class Model
{
    public int model_id;
    public string model_3dfile;
    public string model_2dfile;
}

[System.Serializable]
public class ModelDto
{
    public int idx;
    public int model_id;
    public string model_3dfile_name;
    public string model_3dfile;
    public string model_3dfile_type;
    public string model_3dfile_size;
    public string model_2dfile_name;
    public string model_2dfile;
    public string model_2dfile_type;
    public string model_2dfile_size;
    public byte[] model_3dbytes;
    public byte[] model_2dbytes;
}

[System.Serializable]
public class ModelResponse
{
    public string api_result;
    public string err_message;
    public List<ModelDto> data = new List<ModelDto>();
}

[System.Serializable]
public class InsResponse
{
    public string api_result;
    public string err_message;
    public List<InspectionDto> data = new List<InspectionDto>();
}

[System.Serializable]
public class InsObjectResponse
{
    public string api_result;
    public string err_message;
    public List<DamageObjectTypeDto> data = new List<DamageObjectTypeDto>();
}

[System.Serializable]
public class InsTypeResponse
{
    public string api_result;
    public string err_message;
    public List<DamageTypeDto> data = new List<DamageTypeDto>();
}

[System.Serializable]
public class Inspection
{
	public int idx;
    public int model_idx;
    public string inspector_name;
    public string admin_name;
    public int damage_space;
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
    public int rowkey;
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
    public string ins_image_type;
	public string ins_image_size;
    public string ad_image_name;
	public string ad_image_url;
    public string ad_image_type;
	public string ad_image_size;
    public string space_name_en;
    public string damage_name_en;
    public string object_name_en;
    public string state_name;
	public byte[] ins_bytes; 
	public byte[] ad_bytes;
    public int rowkey;
}

[System.Serializable]
public class DamageObjectTypeDto
{
    public int idx;
    public int space_idx;
    public string object_name_en;
    public string object_name_ko;
}

[System.Serializable]
public class DamageTypeDto
{
    public int idx;
    public int object_idx;
    public string damage_name_en;
    public string damage_name_ko;
}

public class SIMS_Demo : MonoBehaviour
{
    //private string serverPath = "http://localhost:8080";
    private string serverPath = "http://112.157.106.35:8080";
    private string serverPort = "8080";

    private Inspection _Ins = new Inspection();
    private Model _model = new Model();
    
    public InputField ifInsInspector;
    public TMP_InputField ifinspector_etc;
    public Dropdown DdSpace;
    public Dropdown DdDamageObject;
    public Dropdown DdDamageType;
    public Text txtImagepath;

    public GameObject panel_Inspection;
    public GameObject List_Panel;
    public GameObject List_on_btn;
    public GameObject List_off_btn;
    public Transform StartModelPoint;

    private List<string> m_DropOptions = new List<string>();

    private int listCount;
    
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine("CheckPermissionAndroid");
        }
        SimsLog(Application.persistentDataPath);
        Start_ModelInstantiate();
    }

    private void UpdateServerIpPort()
    {
        //string ip = "localhost";
        string ip = "112.157.106.35";
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
        //GameObject.Find("afas").GetComponent<Text>().text += text + "\n";
    }

    private void UpdateDataInspection()
    {
        UpdateServerIpPort();

        _Ins.model_idx = SingletonModelIdx.instance.ModelIdx;
        _Ins.inspector_name = ifInsInspector.text.ToString();
        _Ins.inspector_etc = ifinspector_etc.text.ToString();

        GameObject Capsule = GameObject.Find("Capsule");

        Vector3 pos = Capsule.transform.position;
        try
        {
            _Ins.damage_space = (DdSpace.value)+1;
        }
        catch (FormatException)
        {
            _Ins.damage_space = -1;
        }
        try
        {
            _Ins.damage_type = (DdDamageObject.value)+1;
        }
        catch (FormatException)
        {
            _Ins.damage_type = -1;
        }
        try
        {
            _Ins.damage_object = (DdDamageType.value)+1;
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

        _Ins.ins_image_name = txtImagepath.text;

        //Debug.Log("Inspection DB : " + _Ins.idx.ToString() + "/" + _Ins.ins_date + "/" + _Ins.inspector_name + "/" + _Ins.damage_type.ToString() + "/" + _Ins.damage_object + "/" + _Ins.damage_loc_x.ToString() + "/" + _Ins.damage_loc_y.ToString() + "/" + _Ins.damage_loc_z.ToString() + "/" + _Ins.ins_image_name);
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

    public void Start_ModelInstantiate()
    {
        UpdateServerIpPort();
        string postData ="{\"idx\" : " + SingletonModelIdx.instance.ModelIdx + "}";
        ModelInstantiate("model/select_idx", postData);
    }

    public void OnClick_InsObjectList()
    {   
        UpdateServerIpPort();

        int val = DdSpace.value + 1;
        string postData ="{\"space_idx\" : " + val + "}";

        StartCoroutine(InsObjectIdx("inspection/damageobject_select_spaceidx",postData));
    }

    public void OnClick_InsTypeList()
    {   
        UpdateServerIpPort();

        int s_Dd = DdSpace.value+1;
        int o_Dd = DdDamageObject.value+1;

        //object
        int obj_Idx = 0 ;

        if(s_Dd == 1) {    //Bath room
            if(o_Dd == 1) obj_Idx = 1;
            else if(o_Dd == 2) obj_Idx = 2;
            else if(o_Dd == 3) obj_Idx = 3;
            else obj_Idx = 4;
        }
        else if(s_Dd == 2) {   //Living room
            if(o_Dd == 1) obj_Idx = 5;
            else obj_Idx = 6;
        }
        else if(s_Dd == 3) {   //Kitchen
            if(o_Dd == 1) obj_Idx = 7;
            else if(o_Dd == 2) obj_Idx = 8;
            else obj_Idx = 9;
        }
        else if(s_Dd == 4) {   //Bed room
            if(o_Dd == 1) obj_Idx = 10;
            else obj_Idx = 11;
        }
        else {  //Office
            if(o_Dd == 1) obj_Idx = 12;
            else obj_Idx = 13;
        }
        
        string postData ="{\"object_idx\" : " + obj_Idx + "}";

        StartCoroutine(InsTypeIdx("inspection/damage_select_objectidx",postData));
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
        formData.Add(new MultipartFormDataSection("damage_loc_x", _Ins.damage_loc_x.ToString()));
        formData.Add(new MultipartFormDataSection("damage_loc_y", _Ins.damage_loc_y.ToString()));
        formData.Add(new MultipartFormDataSection("damage_loc_z", _Ins.damage_loc_z.ToString()));
        formData.Add(new MultipartFormDataSection("inspector_etc", _Ins.inspector_etc != "" ? _Ins.inspector_etc : "-1"));

        //object
        int obj_Idx = 0 ;

        if(_Ins.damage_space == 1) {    //Bath room
            if(_Ins.damage_object == 1) obj_Idx = 1;
            else if(_Ins.damage_object == 2) obj_Idx = 2;
            else if(_Ins.damage_object == 3) obj_Idx = 3;
            else obj_Idx = 4;
        }
        else if(_Ins.damage_space == 2) {   //Living room
            if(_Ins.damage_object == 1) obj_Idx = 5;
            else obj_Idx = 6;
        }
        else if(_Ins.damage_space == 3) {   //Kitchen
            if(_Ins.damage_object == 1) obj_Idx = 7;
            else if(_Ins.damage_object == 2) obj_Idx = 8;
            else obj_Idx = 9;
        }
        else if(_Ins.damage_space == 4) {   //Bed room
            if(_Ins.damage_object == 1) obj_Idx = 10;
            else obj_Idx = 11;
        }
        else {  //Office
            if(_Ins.damage_object == 1) obj_Idx = 12;
            else obj_Idx = 13;
        }

        formData.Add(new MultipartFormDataSection("damage_object", obj_Idx > -1 ? obj_Idx.ToString() : "-1"));
        
        //type
        int type_Idx = 0 ;

        if(obj_Idx == 1) {    //Tile
            if(_Ins.damage_type == 1) type_Idx = 1;
            else if(_Ins.damage_type == 2) type_Idx = 2;
            else type_Idx = 3;
        }
        else if(obj_Idx == 2) {   //Wall/Floor/ceiling
            if(_Ins.damage_type == 1) type_Idx = 4;
            else if(_Ins.damage_type == 2) type_Idx = 5;
            else if(_Ins.damage_type == 3) type_Idx = 6;
            else type_Idx = 7;
        }
        else if(obj_Idx == 3) {   //Light
            if(_Ins.damage_type == 1) type_Idx = 8;
            else type_Idx = 9;
        }
        else if(obj_Idx == 4) {   //Pipes
            if(_Ins.damage_type == 1) type_Idx = 10;
            else type_Idx = 11;
        }
        else if(obj_Idx == 5) {   //Wall/Floor/ceiling
            if(_Ins.damage_type == 1) type_Idx = 12;
            else if(_Ins.damage_type == 2) type_Idx = 13;
            else if(_Ins.damage_type == 3) type_Idx = 14;
            else type_Idx = 15;
        }
        else if(obj_Idx == 6) {   //Light
            if(_Ins.damage_type == 1) type_Idx = 16;
            else type_Idx = 17;
        }
        else if(obj_Idx == 7) {   //Sink
            if(_Ins.damage_type == 1) type_Idx = 18;
            else if(_Ins.damage_type == 2) type_Idx = 19;
            else if(_Ins.damage_type == 3) type_Idx = 20;
            else type_Idx = 21;
        }
        else if(obj_Idx == 8) {   //Wall/Floor/ceiling
            if(_Ins.damage_type == 1) type_Idx = 22;
            else if(_Ins.damage_type == 2) type_Idx = 23;
            else if(_Ins.damage_type == 3) type_Idx = 24;
            else type_Idx = 25;
        }
        else if(obj_Idx == 9) {   //Light
            if(_Ins.damage_type == 1) type_Idx = 26;
            else type_Idx = 27;
        }
        else if(obj_Idx == 10) {   //Wall/Floor/ceiling
            if(_Ins.damage_type == 1) type_Idx = 28;
            else if(_Ins.damage_type == 2) type_Idx = 29;
            else if(_Ins.damage_type == 3) type_Idx = 30;
            else type_Idx = 31;
        }
        else if(obj_Idx == 11) {   //Light
            if(_Ins.damage_type == 1) type_Idx = 32;
            else type_Idx = 33;
        }
        else if(obj_Idx == 12) {   //Wall/Floor/ceiling
            if(_Ins.damage_type == 1) type_Idx = 34;
            else if(_Ins.damage_type == 2) type_Idx = 35;
            else if(_Ins.damage_type == 3) type_Idx = 36;
            else type_Idx = 37;
        }
        else {  //Light
            if(_Ins.damage_type == 1) type_Idx = 38;
            else type_Idx = 39;
        }
        
        formData.Add(new MultipartFormDataSection("damage_type", type_Idx  > -1 ? type_Idx.ToString() : "-1"));
        
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

        string RowKey = GameObject.Find("UXParent").transform.Find("MobileUX").transform.Find("RowKeyText").GetComponent<Text>().text;
        formData.Add(new MultipartFormDataSection("rowkey", RowKey != "-1" ? RowKey : "-1"));

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

        string postData ="{\"idx\" : " + SingletonModelIdx.instance.ModelIdx + "}";
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
        string data = jObject.GetValue("data").ToString();
               
        InsResponse jObjText = (InsResponse) JsonConvert.DeserializeObject<InsResponse>(jObject.ToString());
        List<InspectionDto> list = new List<InspectionDto>(jObjText.data);

        GameObject Item = Resources.Load<GameObject>("Item_Panel");
        int yValue = 0;

        for(int i=0; i<list.Count; i++)
        {
            var index = Instantiate(Item, new Vector3(0, yValue, 0), Quaternion.identity);
            index.name = "item" + i;
            index.transform.SetParent(GameObject.Find("Content").transform);
            yValue -= 200;
        }
        listCount = list.Count;

        int count = 0;

        foreach(InspectionDto j in list)
        {
            //이미지 넣기
            byte[] newBytes22 = j.ins_bytes;
            Debug.Log(j.ins_bytes);
            MemoryStream ms = new MemoryStream(newBytes22);
            newBytes22 = ms.ToArray();

            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(newBytes22);
            
            GameObject imageObj = GameObject.Find("item"+count).transform.Find("Item_Image").gameObject;
            Image image = imageObj.GetComponent<Image>();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.0f), 1.0f);
            image.sprite = sprite; 

            //하자 리스트 정보값 출력
            GameObject.Find("item"+count).transform.Find("ItemInsType_Text").GetComponent<Text>().text = j.space_name_en+"_"+j.object_name_en+"_"+j.damage_name_en;
            GameObject.Find("item"+count).transform.Find("ItemInsDate_Text").GetComponent<Text>().text = "Date : "+j.ins_date; 
            GameObject.Find("item"+count).transform.Find("ItemInsInspector_Text").GetComponent<Text>().text ="Inspector : "+j.inspector_name; 
            GameObject.Find("item"+count).transform.Find("ItemInsLoc_Text").GetComponent<Text>().text ="Location Defect : "+j.damage_loc_x+" / "+j.damage_loc_y+" / "+j.damage_loc_z; 
            GameObject.Find("item"+count).transform.Find("ItemInsETC_Text").GetComponent<Text>().text ="Etc : "+j.inspector_etc;

            count++;
        }
    }

    private IEnumerator InsObjectIdx(string uri,string data)
    {
        var url = string.Format("{0}/{1}", serverPath, uri);

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
       
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        
        request.SetRequestHeader("Content-Type", "application/json");

        //응답을 기다립니다.
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("장소 Idx로 조회가 실패했습니다. " + request.responseCode);
        }
        else
        {
            byte[] results = request.downloadHandler.data;
            var message = Encoding.UTF8.GetString(results);
            Debug.Log(message);     //응답했다.
            
            InsObjectResponse ins = (InsObjectResponse)JsonUtility.FromJson<InsObjectResponse>(message);
            List<DamageObjectTypeDto> list1 = new List<DamageObjectTypeDto>(ins.data);
            
            DdDamageObject.ClearOptions();
            DdDamageType.ClearOptions();

            foreach (DamageObjectTypeDto c in list1)
            {
                Dropdown.OptionData option = new Dropdown.OptionData();
                option.text = c.object_name_en;
                DdDamageObject.options.Add(option);
            }
            DdDamageObject.value=-1;
        }
    }
    private IEnumerator InsTypeIdx(string uri,string data)
    {
        var url = string.Format("{0}/{1}", serverPath, uri);

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
       
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        
        request.SetRequestHeader("Content-Type", "application/json");

        //응답을 기다립니다.
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("객체 Idx로 조회가 실패했습니다. " + request.responseCode);
        }
        else
        {
            byte[] results = request.downloadHandler.data;
            var message = Encoding.UTF8.GetString(results);
            //Debug.Log(message);     //응답했다.

            InsTypeResponse ins = (InsTypeResponse)JsonUtility.FromJson<InsTypeResponse>(message);
            List<DamageTypeDto> list1 = new List<DamageTypeDto>(ins.data);
            
            DdDamageType.ClearOptions();
            
            foreach (DamageTypeDto c in list1)
            {
                Dropdown.OptionData option = new Dropdown.OptionData();
                option.text = c.damage_name_en;
                DdDamageType.options.Add(option);
            }
            DdDamageType.value=-1;
        }
    }

    private void ModelInstantiate(string uri, string postData)
    {
        var url = string.Format("{0}/{1}", serverPath, uri);
        string responseText = string.Empty;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Timeout = 30 * 10000; // 30초
        request.ContentType = "application/json; charset=utf-8";

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

        ModelResponse jObjText = (ModelResponse) JsonConvert.DeserializeObject<ModelResponse>(jObject.ToString());
        List<ModelDto> list = new List<ModelDto>(jObjText.data);

        Control con = GameObject.Find("Manager").GetComponent<Control>();
        con.model_name = list[0].model_3dfile_name;

        byte[] msgByte = list[0].model_3dbytes;

        string write_path = Application.persistentDataPath + "/" + list[0].model_3dfile_name; 
        
        // SimsLog(write_path);
        File.WriteAllBytes(write_path, msgByte);

        StartCoroutine(LoadFromMemoryAsync(write_path, list[0].model_3dfile_name));
    }

    IEnumerator LoadFromMemoryAsync(string path, string object_name)
    {
        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromFileAsync(path); //Asset bundle load
        yield return createRequest;

        AssetBundle bundle = createRequest.assetBundle;

        var prefab = bundle.LoadAsset(object_name); //asset bundle에서 사용하고 싶은 object find
        Instantiate(prefab, StartModelPoint.position, StartModelPoint.rotation); //prefab instance화

        Transform model = GameObject.Find(object_name + "(Clone)").transform;
        Transform[] children = new Transform[model.childCount];

        for(int i=0; i<model.childCount; i++)
        {
            children[i]=model.GetChild(i);
            children[i].gameObject.SetActive(false);
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
        panel_Inspection.gameObject.SetActive(false);
    }

    private void On_List()
    {
        List_Panel.SetActive(true);
        List_on_btn.gameObject.SetActive(false);
        List_off_btn.gameObject.SetActive(true);
    }

    public void Off_List()
    {
        int count = 0;
        for(int i=0; i<listCount; i++)
        {
            Destroy(GameObject.Find("item" + count));
            count++;
        }

        List_Panel.gameObject.SetActive(false);
        List_on_btn.gameObject.SetActive(true);
        List_off_btn.gameObject.SetActive(false);
    }
}

