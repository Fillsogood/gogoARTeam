using UnityEngine;
using GoogleARCoreInternal;

public class Control : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        OnDisable();       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnEnable()
    {
        LifecycleManager.Instance.EnableSession();
    }
    public void OnDisable()
    {
        LifecycleManager.Instance.DisableSession();
    }
}

