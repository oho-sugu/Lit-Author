using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHandler : MonoBehaviour
{
    public InputField url;
    public Text uuid;

    public void StartupUpdateMode(){
        url.text = StaticDatas.selectedGO.GetComponent<ObjectController>().objectData.url;
        uuid.text = StaticDatas.selectedGO.GetComponent<ObjectController>().objectData.key;
    }

    public void OnUpdate(){
        StaticDatas.selectedGO.GetComponent<ObjectController>().objectData.url = url.text;
        GameObject.Find("Main").GetComponent<APIController>().updateObject(StaticDatas.selectedGO.GetComponent<ObjectController>().objectData, StaticDatas.selectedGO);
    }

    public void OnResetRotation(){
        StaticDatas.selectedGO.transform.rotation = Quaternion.identity;
    }

    public void OnResetScale(){
        StaticDatas.selectedGO.transform.localScale = Vector3.one;
    }

    public void OnReload(){
        var od = StaticDatas.selectedGO.GetComponent<ObjectController>().objectData;
        StaticDatas.dm.open(
            StaticDatas.selectedGO.GetComponent<Orthoverse.Container>().GetCurrent(),
            od.url, Orthoverse.OpenMode.self, od);
    }

    public void OnDelete(){
        GameObject.Find("Main").GetComponent<APIController>().deleteObject(StaticDatas.selectedGO.GetComponent<ObjectController>().objectData.key, StaticDatas.selectedGO);
    }
}
