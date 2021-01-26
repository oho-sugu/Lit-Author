using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;

public class APIController : MonoBehaviour
{
    [Serializable]
    public class ListObjects {
        public List<ObjectData> objects;
    }
    [Serializable]
    public class ObjectData {
        public string key;
        public string location;
        public string channel;
        public string url;
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
    }

    private const string siteurl = "lit-server-test.an.r.appspot.com";

    // All Transform parameter are world space
    public void newObject(Vector3 position, Quaternion rotation, Vector3 scale, string url){
        StartCoroutine(_newObject(position, rotation, scale, url) );
    }
    IEnumerator _newObject(Vector3 position, Quaternion rotation, Vector3 scale, string url){
        string posturl = "http://"+siteurl+"/new";
        ObjectData data = new ObjectData();
        data.channel = StaticDatas.Channel;
        data.location = StaticDatas.Location;
        data.url = url;
        data.key = Guid.NewGuid().ToString();
        data.position = position;
        data.scale = scale;
        data.rotation = rotation;

        string json = JsonUtility.ToJson(data);
        Debug.Log(json);
        byte[] postdata = Encoding.UTF8.GetBytes(json);
        
        var request = new UnityWebRequest(posturl, UnityWebRequest.kHttpVerbPOST){
            uploadHandler = new UploadHandlerRaw(postdata),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if(request.isNetworkError || request.isHttpError){
            Debug.Log(request.error);
        } else {
            Debug.Log(request.downloadHandler.text);
            if(request.downloadHandler.text.Equals("Success")){
                ObjectInstanciate(data);
            }
        }
    }

    public void updateObject(ObjectData original, GameObject root){
        StartCoroutine(_updateObject(original, root) );
    }
    IEnumerator _updateObject(ObjectData original, GameObject root){
        string posturl = "http://"+siteurl+"/update";

        original.position = root.transform.localPosition;
        original.scale = root.transform.localScale;
        original.rotation = root.transform.localRotation;

        string json = JsonUtility.ToJson(original);
        Debug.Log(json);
        byte[] postdata = Encoding.UTF8.GetBytes(json);
        
        var request = new UnityWebRequest(posturl, UnityWebRequest.kHttpVerbPOST){
            uploadHandler = new UploadHandlerRaw(postdata),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if(request.isNetworkError || request.isHttpError){
            Debug.Log(request.error);
        } else {
            root.transform.hasChanged = false;
            Debug.Log(request.downloadHandler.text);
        }
    }

    public void listsObject(string location,string channel){
        StartCoroutine(_listsObject(location, channel));
    }

    IEnumerator _listsObject(string location, string channel){
        string geturl = "http://"+siteurl+"/lists?location="+location+"&channel="+channel;

        var request = new UnityWebRequest(geturl, UnityWebRequest.kHttpVerbGET){
            downloadHandler = new DownloadHandlerBuffer()
        };
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if(request.isNetworkError || request.isHttpError){
            Debug.Log(request.error);
        } else {
            Debug.Log(request.downloadHandler.text);
            var datas = (ListObjects)JsonUtility.FromJson<ListObjects>(request.downloadHandler.text);
            Debug.Log(datas.objects.Count);

            foreach(var data in datas.objects){
                Debug.Log(data);
                ObjectInstanciate(data);
            }
        }
    }

    public void ObjectInstanciate(ObjectData data){
        StaticDatas.dm.open(null, data.url, Orthoverse.OpenMode.blank, data);
/*        var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);

        var ocon = obj.AddComponent<ObjectController>();
        ocon.objectData = data;
        obj.transform.localPosition = data.position;
        obj.transform.localScale = data.scale;
        obj.transform.localRotation = data.rotation;
        obj.transform.SetParent(StaticDatas.objectRoot.transform, false);
        obj.transform.hasChanged = false;

        var om = obj.AddComponent<ObjectManipulator>();
        var bc = obj.AddComponent<BoundsControl>();
        bc.Target = obj;
        bc.BoundsOverride = obj.GetComponent<BoxCollider>();
        bc.BoundsControlActivation = Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes.BoundsControlActivationType.ActivateManually;*/
    }

    public void deleteObject(string key, GameObject obj){
        StartCoroutine(_deleteObject(key, obj));
    }

    IEnumerator _deleteObject(string key, GameObject obj){
        string geturl = "http://"+siteurl+"/delete?key="+key;

        var request = new UnityWebRequest(geturl, UnityWebRequest.kHttpVerbGET){
            downloadHandler = new DownloadHandlerBuffer()
        };
        yield return request.SendWebRequest();

        if(request.isNetworkError || request.isHttpError){
            Debug.Log(request.error);
        } else {
            Debug.Log(request.downloadHandler.text);
            if(request.downloadHandler.text.Equals("Success")){
                Destroy(obj);
            }
        }
    }
}
