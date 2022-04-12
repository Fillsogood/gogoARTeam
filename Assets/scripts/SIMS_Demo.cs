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
	public int ins_id;
	public string ins_date;
	public string inspector;
	public int damage_type;
	public string damage_object;
	public int damage_loc_x;
	public int damage_loc_y;
	public int damage_loc_z;

	public string image_name;
	public string image_size;
	public string image_type;
	public byte[] image; 

}

public class SIMS_Demo : MonoBehaviour
{
    private string serverPath = "http://localhost:8080";
    //private string serverPath = "http://192.168.219.104:8080";

    private string serverPort = "8080";


    public static string strToEdit = "";

    private bool isConsoleShowing = true;

    private float ta_left;
    private float ta_top;
    private float ta_width;
    private float ta_height;

    private Inspection _Ins = new Inspection();
    private Model _model = new Model();

    void Start()
    {
        Screen.SetResolution(360, 700, false);

        // GameObject panelObj = GameObject.Find("panel_console");
        // RectTransform rt = panelObj.GetComponent<RectTransform>();

        // ta_left = rt.offsetMin.x;
        // //ta_right = -rt.offsetMax.x;
        // ta_top = -rt.offsetMax.y;
        // //ta_bottom = rt.offsetMin.y;
        // ta_width = rt.rect.width;
        // ta_height = rt.rect.height;

        // Debug.Log("LEFT : " + ta_left);
        // Debug.Log("TOP : " + ta_top);
        // Debug.Log("WIDTH : " + ta_width);
        // Debug.Log("HEIGHT : " + ta_height);

        // GameObject.Find("ifServerIP").GetComponent<InputField>().text = "192.168.";
        // GameObject.Find("ifServerPort").GetComponent<InputField>().text = "8080";
        // GameObject.Find("ifPicturePath").GetComponent<InputField>().text = "";

        if (Application.platform == RuntimePlatform.Android)
        {
            GameObject.Find("ifPicturePath").GetComponent<InputField>().text = "";

            StartCoroutine("CheckPermissionAndroid");
        }
        //SimsLog(Application.persistentDataPath);
    }

    private void UpdateServerIpPort()
    {
        string ip = "192.168.0.8";
        string port = "8080";
        Debug.Log(ip);
        Debug.Log(port);
        if (ip == "" || port == "")
        {
            SimsLog("IP �� Port �Է��ϼ���. ������ ����� �� ���� �ֽ��ϴ�.");
        }
        else
        {
            serverPath = "http://" + ip + ":" + port;
        }
    }

    private void UpdateDataModel()
    {
        UpdateServerIpPort();

        try
        {
            _model.model_id = Convert.ToInt32(GameObject.Find("ifModelID").GetComponent<InputField>().text.ToString());
        }
        catch (FormatException)
        {
            _model.model_id = -1;
        }
        _model.model_3dfile = GameObject.Find("if3Dfile").GetComponent<InputField>().text.ToString();
        _model.model_2dfile = GameObject.Find("if2Dfile").GetComponent<InputField>().text.ToString();

        Debug.Log("Model DB : " + _model.model_id.ToString() + "/" + _model.model_3dfile + "/" + _model.model_2dfile);
        SimsLog("Model DB : " + _model.model_id.ToString() + "/" + _model.model_3dfile + "/" + _model.model_2dfile);

    }

    private void UpdateDataFormModel()
    {
        GameObject.Find("ifModelID").GetComponent<InputField>().text = _model.model_id.ToString();
        GameObject.Find("if3Dfile").GetComponent<InputField>().text = _model.model_3dfile;
        GameObject.Find("if2Dfile").GetComponent<InputField>().text = _model.model_2dfile;
    }

    public void ClearDataModel()
    {
        GameObject.Find("ifModelID").GetComponent<InputField>().text = "";
        GameObject.Find("if3Dfile").GetComponent<InputField>().text = "";
        GameObject.Find("if2Dfile").GetComponent<InputField>().text = "";
    }

    public void ClearConsole()
    {
        strToEdit = "";
    }

    private void UpdateDataInspection()
    {
        UpdateServerIpPort();

        try
        {
            _Ins.ins_id = Convert.ToInt32(GameObject.Find("ifInsID").GetComponent<InputField>().text.ToString());
        }
        catch (FormatException)
        {
            _Ins.ins_id = -1;
        }
        _Ins.ins_date = GameObject.Find("ifInsDate").GetComponent<InputField>().text.ToString();
        _Ins.inspector = GameObject.Find("ifInsInspector").GetComponent<InputField>().text.ToString();
        
        try
        {
            _Ins.damage_type = Convert.ToInt32(GameObject.Find("ifDamageType").GetComponent<InputField>().text.ToString());
        }
        catch (FormatException)
        {
            _Ins.damage_type = -1;
        }
        _Ins.damage_object = GameObject.Find("ifDamageObject").GetComponent<InputField>().text.ToString();
        try
        {
            _Ins.damage_loc_x = Convert.ToInt32(GameObject.Find("ifDamageX").GetComponent<InputField>().text.ToString());
        }
        catch (FormatException)
        {
            _Ins.damage_loc_x = -1;
        }
        try
        {
            _Ins.damage_loc_y = Convert.ToInt32(GameObject.Find("ifDamageY").GetComponent<InputField>().text.ToString());
        }
        catch (FormatException)
        {
            _Ins.damage_loc_y = -1;
        }

        try
        {
            _Ins.damage_loc_z = Convert.ToInt32(GameObject.Find("ifDamageZ").GetComponent<InputField>().text.ToString());
        }
        catch (FormatException)
        {
            _Ins.damage_loc_z = -1;
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            _Ins.image_name = "/storage/emulated/0/DCIM/" + GameObject.Find("ifPicturePath").GetComponent<InputField>().text.ToString();
        }
        else
        {
            _Ins.image_name = GameObject.Find("ifPicturePath").GetComponent<InputField>().text.ToString();
        }

        Debug.Log("Inspection DB : " + _Ins.ins_id.ToString() + "/" + _Ins.ins_date + "/" + _Ins.inspector + "/" + _Ins.damage_type.ToString() + "/" + _Ins.damage_object + "/" + _Ins.damage_loc_x.ToString() + "/" + _Ins.damage_loc_y.ToString() + "/" + _Ins.damage_loc_z.ToString() + "/" + _Ins.image_name);
        SimsLog("Inspection DB : " + _Ins.ins_id.ToString() + "/" + _Ins.ins_date + "/" + _Ins.inspector + "/" + _Ins.damage_type.ToString() + "/" + _Ins.damage_object + "/" + _Ins.damage_loc_x.ToString() + "/" + _Ins.damage_loc_y.ToString() + "/" + _Ins.damage_loc_z.ToString() + "/" + _Ins.image_name);

    }

    private void UpdateDataForm()
    {
        GameObject.Find("ifInsID").GetComponent<InputField>().text = _Ins.ins_id.ToString();
        GameObject.Find("ifInsDate").GetComponent<InputField>().text = _Ins.ins_date;
        GameObject.Find("ifInsInspector").GetComponent<InputField>().text = _Ins.inspector;
        GameObject.Find("ifDamageType").GetComponent<InputField>().text = _Ins.damage_type.ToString();
        GameObject.Find("ifDamageObject").GetComponent<InputField>().text = _Ins.damage_object;
        GameObject.Find("ifDamageX").GetComponent<InputField>().text = _Ins.damage_loc_x.ToString();
        GameObject.Find("ifDamageY").GetComponent<InputField>().text = _Ins.damage_loc_y.ToString();
        GameObject.Find("ifDamageZ").GetComponent<InputField>().text = _Ins.damage_loc_z.ToString();
        GameObject.Find("ifPicturePath").GetComponent<InputField>().text = _Ins.image_name;
    }

    public void ClearDataInspection()
    {
        GameObject.Find("ifInsID").GetComponent<InputField>().text = "";
        GameObject.Find("ifInsDate").GetComponent<InputField>().text = "";
        GameObject.Find("ifInsInspector").GetComponent<InputField>().text = "";
        GameObject.Find("ifDamageType").GetComponent<InputField>().text = "";
        GameObject.Find("ifDamageObject").GetComponent<InputField>().text = "";
        GameObject.Find("ifDamageX").GetComponent<InputField>().text = "";
        GameObject.Find("ifDamageY").GetComponent<InputField>().text = "";
        GameObject.Find("ifDamageZ").GetComponent<InputField>().text = "";
        GameObject.Find("ifPicturePath").GetComponent<InputField>().text = "";
    }

    private void SimsLog(string text)
    {
        strToEdit = strToEdit + text + "\n";
    }

    void OnGUI()
    {
        strToEdit = GUI.TextArea(new Rect(ta_left, ta_top, ta_width, ta_height), strToEdit);
        //strToEdit = GUI.TextArea(new Rect(10, 10, 200, 100), strToEdit);
    }

    public void OnClick_InsInsert()
    {

        UpdateDataInspection();

        SimsLog("Inspection Insert : " + _Ins.image_name);
        StartCoroutine(PostFormDataImage("inspection", "", _Ins.image_name));
    }

    public void OnClick_InsUpdate()
    {
        UpdateDataInspection();

        StartCoroutine(PostFormDataImage("inspection", _Ins.ins_id.ToString(), _Ins.image_name));

    }
    public void OnClick_InsUpdateData()
    {
        UpdateDataInspection();

        StartCoroutine(PostFormData("inspection/data", _Ins.ins_id.ToString()));
    }

    public void OnClick_InsUpdateImage()
    {
        UpdateDataInspection();
        StartCoroutine(PostFormImage("inspection/image", _Ins.ins_id.ToString(), _Ins.image_name));
    }

    public void OnClick_InsSelect()
    {
        UpdateDataInspection();

        string uri = "inspection/" + _Ins.ins_id.ToString();
        //form-data(image, key/value) ���� ��������
        StartCoroutine(this.GetMultipartformImage(uri));
    }

    public void OnClick_InsDelete()
    {
        UpdateDataInspection();

        string uri = "inspection/" + _Ins.ins_id.ToString();
        //form-data(image, key/value) ����
        StartCoroutine(this.Delete(uri)); 
    }

    public void OnClick_ModelInsert()
    {
        UpdateDataModel();

        var json = JsonConvert.SerializeObject(_model);
        StartCoroutine(this.Post("model", "", json)); //�߰�
    }

    public void OnClick_ModelSelect()
    {
        UpdateDataModel();

        string uri = "model/" + _model.model_id.ToString();
        StartCoroutine(this.Get(uri));
    }

    public void OnClick_ModelUpdate()
    {
        UpdateDataModel();

        var json = JsonConvert.SerializeObject(_model);
        StartCoroutine(this.Post("model", _model.model_id.ToString(), json)); //�߰�
    }

    public void OnClick_ModelDelete()
    {
        UpdateDataModel();

        string uri = "model/" + _model.model_id.ToString();
        StartCoroutine(this.Delete(uri, "model")); //�������� 1����
    }

    public void OnClick_ModelGetAll()
    {
        UpdateDataModel();
        StartCoroutine(this.GetAll("model")); 
    }

    private void Test()
    {
        //Company data = new Company();
        //data.companyId = 18;
        //data.name = "�ƺ���";
        //data.address = "�����̸� �����";

        //var json = JsonConvert.SerializeObject(data);
        //StartCoroutine(this.Post("company", json)); //�߰�
        //StartCoroutine(this.Get("employee/2")); //�������� 1����

        //������Ʈ
        //StartCoroutine(this.Post("employee/19", json)); //�������� 1����

        //����
        //StartCoroutine(this.Delete("employee/19")); //�������� 1����

        //��ü ��������
        //StartCoroutine(this.GetAll("employee")); //�������� 1����
        //StartCoroutine(this.GetImage("image/5")); //�������� 1����


        //form-data(image, key/value) ���� ���ε� ����
        //StartCoroutine(PostFormData("inspection", "C:/Users/Administrator/Desktop/lovely.jpg"));
        
        //form-data(image, key/value) ���� ��������
        //StartCoroutine(this.GetMultipartformImage("inspection/1000"));
        //File.WriteAllBytes("d:/simsreality.jpg", ins.image);

        //form-data(image, key/value) ������Ʈ
        //StartCoroutine(PostFormData("inspection/1000", "C:/Users/Administrator/Desktop/1.png"));

        //form-data(image, key/value) ����
        //StartCoroutine(this.Delete("inspection/4000")); 

        //form-data(image, key/value) all
        //StartCoroutine(this.GetAll("inspection"));
    }

    IEnumerator CheckPermissionAndroid()
    {
        //SimsLog("CheckPermissionAndroid");

        yield return new WaitForEndOfFrame();

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            Permission.RequestUserPermission(Permission.ExternalStorageRead);

            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(() => Application.isFocused == true);

            if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
            {
                //���̾�α׸� ���� ������ �÷������� ����߾���. �� �ڵ�� �ּ� ó����.
                //AGAlertDialog.ShowMessageDialog("���� �ʿ�", "��ũ������ �����ϱ� ���� ����� ������ �ʿ��մϴ�.",
                //"Ok", () => OpenAppSetting(),
                //"No!", () => AGUIMisc.ShowToast("����� ��û ������"));

                // ������ Ȯ�� �˾��� ����� �������� OpenAppSetting()�� �ٷ� ȣ����.
                //OpenAppSetting();
                SimsLog("����� ������ �ʿ���.");
                yield break;
            }
        }

        //string fileLocation = "/storage/emulated/0" + "/DCIM/Screenshots/"; // "mnt/sdcard/DCIM/Screenshots/";
    }
        private void ViewImage(string component_name, byte[] image_bytes)
    {
        //GameObject imageObj = GameObject.Find("demoImage");
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

    //�̹����� ������Ʈ ��.
    private IEnumerator PostFormImage(string uri, string id, string path_image)
    {
        var url = string.Format("{0}/{1}/{2}", serverPath, uri, id);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        if (_Ins.ins_id > -1)
        {
            formData.Add(new MultipartFormDataSection("ins_id", _Ins.ins_id.ToString()));
        }
        else
        {
            SimsLog("���� ID�� �Է��Ͻñ� �ٶ��ϴ�. �ٽ� Ȯ�ιٶ��ϴ�.!!");
            Debug.Log("���� ID�� �Է��Ͻñ� �ٶ��ϴ�. �ٽ� Ȯ�ιٶ��ϴ�.!!");

            yield break; ;
        }

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
            SimsLog("jpg, png ���ϸ� ������ �����մϴ�. �ٽ� Ȯ�ιٶ��ϴ�.!!");
            yield break;
        }

        SimsLog("���� ��� : " + path_image);

        formData.Add(new MultipartFormFileSection("file", img, Path.GetFileName(path_image), strImgformat));

        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        //www.SetRequestHeader("Content-Type", "multipart/form-data");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            SimsLog("���� ID " + _Ins.ins_id.ToString() + "�� ������Ʈ �����߽��ϴ�. " + www.responseCode);
            Debug.Log(www.error);
        }
        else
        {
            SimsLog("���� ID " + _Ins.ins_id.ToString() + " �̹����� ���������� ������Ʈ�� �Ǿ����ϴ�. " + www.responseCode);
            Debug.Log("Request Response: " + www.downloadHandler.text);
        }
    }
    //Inspection �����͸� ������Ʈ
    private IEnumerator PostFormData(string uri, string id)
    {
        var url = string.Format("{0}/{1}/{2}", serverPath, uri, id);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        if (_Ins.ins_id > -1)
        {
            formData.Add(new MultipartFormDataSection("ins_id", _Ins.ins_id.ToString()));
        }
        else
        {
            SimsLog("���� ID�� �Է��Ͻñ� �ٶ��ϴ�. �ٽ� Ȯ�ιٶ��ϴ�.!!");
            Debug.Log("���� ID�� �Է��Ͻñ� �ٶ��ϴ�. �ٽ� Ȯ�ιٶ��ϴ�.!!");

            yield break;;
        }

        formData.Add(new MultipartFormDataSection("ins_date", _Ins.ins_date != "" ? _Ins.ins_date:"-1"));
        formData.Add(new MultipartFormDataSection("inspector", _Ins.inspector != "" ? _Ins.inspector : "-1"));
        formData.Add(new MultipartFormDataSection("damage_type", _Ins.damage_type > -1 ? _Ins.damage_type.ToString() : "-1"));
        formData.Add(new MultipartFormDataSection("damage_object", _Ins.damage_object != "" ? _Ins.damage_object : "-1"));
        formData.Add(new MultipartFormDataSection("damage_loc_x", _Ins.damage_loc_x > -1 ? _Ins.damage_loc_x.ToString() : "-1"));
        formData.Add(new MultipartFormDataSection("damage_loc_y", _Ins.damage_loc_y > -1 ? _Ins.damage_loc_y.ToString() : "-1"));
        formData.Add(new MultipartFormDataSection("damage_loc_z", _Ins.damage_loc_z > -1 ? _Ins.damage_loc_z.ToString() : "-1"));

        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        //www.SetRequestHeader("Content-Type", "multipart/form-data");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            SimsLog("���� ID " + _Ins.ins_id.ToString() + "�� ������Ʈ �����߽��ϴ�. " + www.responseCode);
            Debug.Log(www.error);
        }
        else
        {
            SimsLog("���� ID " + _Ins.ins_id.ToString() + "�� ���������� ������Ʈ�� �Ǿ����ϴ�. " + www.responseCode);
            Debug.Log("Request Response: " + www.downloadHandler.text);

            //���ε尡 �Ϸ�Ǹ� ���� Ŭ�����Ѵ�.
            //ClearDataInsepction();
        }
    }

    //�̹����� Inspection ���� �� ������Ʈ
    private IEnumerator PostFormDataImage(string uri, string id, string path_image)
    {
        //SimsLog("PostFormDataImage");

        var url = string.Format("{0}/{1}/{2}", serverPath, uri, id);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        if (_Ins.ins_id > -1)
        {
            formData.Add(new MultipartFormDataSection("ins_id", _Ins.ins_id.ToString()));
        }
        else
        {
            SimsLog("���� ID�� �Է��Ͻñ� �ٶ��ϴ�. �ٽ� Ȯ�ιٶ��ϴ�.!!");
            Debug.Log("���� ID�� �Է��Ͻñ� �ٶ��ϴ�. �ٽ� Ȯ�ιٶ��ϴ�.!!");

            yield break;;
        }

        formData.Add(new MultipartFormDataSection("ins_date", _Ins.ins_date != "" ? _Ins.ins_date:"-1"));
        formData.Add(new MultipartFormDataSection("inspector", _Ins.inspector != "" ? _Ins.inspector : "-1"));
        formData.Add(new MultipartFormDataSection("damage_type", _Ins.damage_type > -1 ? _Ins.damage_type.ToString() : "-1"));
        formData.Add(new MultipartFormDataSection("damage_object", _Ins.damage_object != "" ? _Ins.damage_object : "-1"));
        formData.Add(new MultipartFormDataSection("damage_loc_x", _Ins.damage_loc_x > -1 ? _Ins.damage_loc_x.ToString() : "-1"));
        formData.Add(new MultipartFormDataSection("damage_loc_y", _Ins.damage_loc_y > -1 ? _Ins.damage_loc_y.ToString() : "-1"));
        formData.Add(new MultipartFormDataSection("damage_loc_z", _Ins.damage_loc_z > -1 ? _Ins.damage_loc_z.ToString() : "-1"));

        //SimsLog("PostFormDataImage:MultipartFormDataSection");

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
            SimsLog("jpg, png ���ϸ� ������ �����մϴ�. �ٽ� Ȯ�ιٶ��ϴ�.!!");
            yield break;
        }

        //SimsLog("PostFormDataImage:MultipartFormFileSection");
        //SimsLog("���� ��� : " + path_image);
        //SimsLog("����  : " + img.ToString());

        formData.Add(new MultipartFormFileSection("file", img, Path.GetFileName(path_image), strImgformat));

        //SimsLog("MultipartFormFileSection");

        UnityWebRequest www = UnityWebRequest.Post(url, formData);
        //www.SetRequestHeader("Content-Type", "multipart/form-data");
        
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            SimsLog("���� ID " + _Ins.ins_id.ToString() + "�� ������ �����߽��ϴ�. " + www.responseCode);
            Debug.Log(www.error);
        }
        else
        {
            if (id == "")
            {
                //Debug.Log("Form upload complete!");
                SimsLog("���� ID " + _Ins.ins_id.ToString() + "�� ���������� Upload(����) �Ǿ����ϴ�. " + www.responseCode);
            }
            else
            {
                SimsLog("���� ID " + _Ins.ins_id.ToString() + "�� ���������� ������Ʈ�� �Ǿ����ϴ�. " + www.responseCode);
            }

            Debug.Log("Request Response: " + www.downloadHandler.text);

            //���ε尡 �Ϸ�Ǹ� ���� Ŭ�����Ѵ�.
            ClearDataInspection();
        }
    }

    #region POST
    //bodyraw post �����ϴ� �����.
    private IEnumerator Post(string uri, string id, string data)
    {
        var url = string.Format("{0}/{1}/{2}", serverPath, uri, id);
        Debug.Log(url);
        Debug.Log(data);
        //POST������� http������ ��û�� �����ڽ��ϴ�.
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        //Debug.Log(bodyRaw.Length);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
 
        //������ ��ٸ��ϴ�.
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            SimsLog("�� ID " + _model.model_id.ToString() + "�� ���ۿ� �����߽��ϴ�. " + request.responseCode);
            Debug.Log(request.error);
        }
        else
        {

            //������ �޾ҽ��ϴ�.
            //Debug.Log(request.downloadHandler.data);
            Debug.Log(request.downloadHandler.text);
            _model = (Model)JsonConvert.DeserializeObject<Model>(request.downloadHandler.text);
            SimsLog(_model.model_id + "/" + _model.model_3dfile + "/" + _model.model_3dfile);

            if (id == "")
            {
                //Debug.Log("Form upload complete!");
                SimsLog("�� ID " + _model.model_id.ToString() + "�� ���������� Upload(����) �Ǿ����ϴ�. " + request.responseCode);
            }
            else
            {
                SimsLog("�� ID " + _model.model_id.ToString() + "�� ���������� ������Ʈ�� �Ǿ����ϴ�. " + request.responseCode);
            }

            Debug.Log("Request Response: " + request.downloadHandler.text);

            //���ε尡 �Ϸ�Ǹ� ���� Ŭ�����Ѵ�.
            ClearDataModel();
        }
 
    }
    #endregion


    #region IMAGE_GET
    private IEnumerator GetImage(string uri) // �̹��� �ϳ��� ���� ���
    {
        string token = "";

        //http������ ��û 
        var url = string.Format("{0}/{1}", serverPath, uri);
        Debug.Log(url);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            //request.SetRequestHeader("Content-Type", "application/json");
            //request.SetRequestHeader("Content-Type", "multipart/form-data");
            //request.SetRequestHeader("Authorization", "Bearer " + token);

            string path = Path.Combine(Application.persistentDataPath, "test1.jpg");
            //request.downloadHandler = new DownloadHandlerFile(path);
            Debug.Log(path);

            yield return request.SendWebRequest();

            //http�����κ��� ������ �޾Ҵ�. 
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                //����� ���ڿ��� ��� 
                //Debug.Log(request.downloadHandler.text);

                /* ������ �̹����� ���Ϸ� ����. ���������� �� �޾������� Ȯ���� ���� �ӽ� �ڵ���.
                try
                {
                    byte[] results = request.downloadHandler.data;
                    File.WriteAllBytes(path, results);
                }
                catch (System.Exception e)
                {
                    Debug.Log("Error : " + e.Message);
                }
                */
                GameObject imageObj = GameObject.Find("demoImage");
                Image image = imageObj.GetComponent<Image>();
                image.type = Image.Type.Simple;
                image.preserveAspect = true;

                Texture2D tex = new Texture2D(2, 2, TextureFormat.RGB24, false);
                tex.filterMode = FilterMode.Trilinear;
                tex.LoadImage(request.downloadHandler.data);
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.0f), 1.0f);
                Debug.Log(tex.width + ", " + tex.height);

                //sprite.
                image.sprite = sprite;
            }
        }
    }
    #endregion

    #region GETMULTIPARTFORM_IMAGE
    private IEnumerator GetMultipartformImage(string uri)
    {
        //http������ ��û 
        var url = string.Format("{0}/{1}", serverPath, uri);
        Debug.Log(url);

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            //http������ ���� ������ ��� 
            yield return www.SendWebRequest();

            //http�����κ��� ������ �޾Ҵ�. 
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                SimsLog("���� ID�� ���ų� �������� ���� �������Դϴ�. Ȯ���Ͻñ� �ٶ��ϴ�.!!");
            }
            else
            {
                //����� ���ڿ��� ��� 
                //Debug.Log(www.downloadHandler.text);

                //���̳ʸ� �����͸� ���� 
                byte[] results = www.downloadHandler.data;

                Debug.Log(results.Length);  //14 

                var message = Encoding.UTF8.GetString(results);

                Debug.Log(message);     //�����ߴ�.!

                Inspection ins = (Inspection)JsonConvert.DeserializeObject<Inspection>(message);
                if (ins != null)
                {
                    _Ins = ins;
                    SimsLog("���� ID : " + _Ins.ins_id.ToString() + " ��ȸ �Ǿ����ϴ�.");
                    UpdateDataForm();

                    //File.WriteAllBytes("d:/sims.jpg", _Ins.image);
                    ViewImage("imgView", _Ins.image);
                }
                else
                {
                    SimsLog("�����͸� �������� ���߽��ϴ�. ID�� Ȯ���ϼ���.");
                }
            }
        }
    }
    #endregion

    #region GET
    private IEnumerator Get(string uri = "")
    {
        //http������ ��û 
        var url = string.Format("{0}/{1}", serverPath, uri);
        Debug.Log(url);
 
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            //http������ ���� ������ ��� 
            yield return www.SendWebRequest();
 
            //http�����κ��� ������ �޾Ҵ�. 
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                SimsLog("�� ID : " + _model.model_id.ToString() + " ��ȸ �����߽��ϴ�.");
            }
            else
            {
                //����� ���ڿ��� ��� 
                Debug.Log(www.downloadHandler.text);
 
                //���̳ʸ� �����͸� ���� 
                byte[] results = www.downloadHandler.data;
 
                Debug.Log(results.Length);  //14 
 
                var message = Encoding.UTF8.GetString(results);
 
                Debug.Log(message);     //�����ߴ�.!

                //var result = JsonConvert.DeserializeObject<Model>(message);
                //Debug.LogFormat("{0}, {1}, {2}", result.CompanyId, result.name, result.address);

                Model model = (Model)JsonConvert.DeserializeObject<Model>(message);
                if (model != null)
                {
                    _model = model;
                    UpdateDataFormModel();
                    SimsLog("�� ID : " + _model.model_id.ToString() + " ��ȸ �Ǿ����ϴ�.");
                }
                else
                {
                    SimsLog("�����͸� �������� ���߽��ϴ�. ID�� Ȯ���ϼ���.");
                }
            }
        }
    }
    #endregion

    #region GETALL
    private IEnumerator GetAll(string uri = "")
    {
        //http������ ��û 
        var url = string.Format("{0}/{1}", serverPath, uri);
        Debug.Log(url);

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            //http������ ���� ������ ��� 
            yield return www.SendWebRequest();

            //http�����κ��� ������ �޾Ҵ�. 
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //����� ���ڿ��� ��� 
                Debug.Log(www.downloadHandler.text);

                //���̳ʸ� �����͸� ���� 
                byte[] results = www.downloadHandler.data;

                Debug.Log(results.Length);  //14 

                var message = Encoding.UTF8.GetString(results);

                Debug.Log(message);     //�����ߴ�.!

                /*var result //Inspection ��ü ����Ʈ�� �˰� ������ �Ʒ��� Ǯ�� ����ϸ� ��.
                List<Inspection> list = JsonConvert.DeserializeObject<List<Inspection>>(message);
                foreach (Inspection c in list)
                {
                    Debug.LogFormat("{0}, {1}, {2}", c.ins_id, c.ins_date, c.inspector);
                } */
                int count = 0;
                List<Model> list = JsonConvert.DeserializeObject<List<Model>>(message);
                foreach (Model c in list)
                {
                    count++;
                    SimsLog(count.ToString() + " : " + c.model_id.ToString() + "/" + c.model_3dfile + "/" + c.model_2dfile);
                }
            }
        }
    }
    #endregion

    #region DELETE
    private IEnumerator Delete(string uri, string type = "ins")
    {
        //http������ ��û 
        var url = string.Format("{0}/{1}", serverPath, uri);
        Debug.Log(url);

        using (UnityWebRequest www = UnityWebRequest.Delete(url))
        {
            //http������ ���� ������ ��� 
            yield return www.SendWebRequest();
            //http�����κ��� ������ �޾Ҵ�. 
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (type == "ins")
                    SimsLog("���� ID : " + _Ins.ins_id.ToString() + " ������ �����߽��ϴ�.");
                else
                    SimsLog("�� ID : " + _model.model_id.ToString() + " ������ �����߽��ϴ�.");
            }
            else
            { //����
                //����� ���ڿ��� ��� 
                //Debug.Log("deleted !!");
                if (type == "ins")
                {
                    SimsLog("���� ID : " + _Ins.ins_id.ToString() + " ���� �Ǿ����ϴ�.");
                    ClearDataInspection();
                }
                else
                {
                    SimsLog("�� ID : " + _model.model_id.ToString() + " ���� �Ǿ����ϴ�.");
                    ClearDataModel();
                }
            }
        } //end using
    }
    #endregion

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
