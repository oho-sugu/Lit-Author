using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewButtonHandler : MonoBehaviour
{
    public InputField url;
    public GameObject MainCamera;
    public void OnNew(){
        var urlstr = url.text;

        var parent = MainCamera.transform.parent;

        MainCamera.transform.SetParent(StaticDatas.objectRoot.transform, true);

        var position = MainCamera.transform.localPosition + MainCamera.transform.forward * 3f;
        var rotation = MainCamera.transform.localRotation;
        var scale = MainCamera.transform.localScale;

        MainCamera.transform.SetParent(parent, true);

        GameObject.Find("Main").GetComponent<APIController>().newObject(position, rotation, scale, urlstr);

        url.text = "";
    }
}
