using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;

using Orthoverse;

public class MainController : MonoBehaviour
{
    public GameObject mrtk;

    public GameObject CanvasNew;
    public GameObject CanvasEdit;

    public GameObject[] maps;
    public string[] locationNames;

    public UpdateHandler updateHandler;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(StaticDatas.Location);
        Debug.Log(StaticDatas.Channel);

        Shader.EnableKeyword("_DIRECTIONAL_LIGHT");
        Shader.EnableKeyword("_HOVER_LIGHT");
        Shader.EnableKeyword("_SPECULAR_HIGHLIGHTS");

        if(StaticDatas.Location == null) StaticDatas.Location = "TESTLOCATION";
        if(StaticDatas.Channel == null) StaticDatas.Channel = "TESTCHANNEL";

        StaticDatas.dm = GetComponent<DocumentManager>();

        // Map Load
        for(int i=0; i < locationNames.Length; i++){
            if(StaticDatas.Location.Equals(locationNames[i])){
                var map = (GameObject)Instantiate(maps[i], Vector3.zero,Quaternion.identity);
            }
        }

        // Avoid name ObjectRoot in Scene. It must only one in the scene. in the map.
        StaticDatas.objectRoot = GameObject.Find("ObjectRoot");

        // Load All Objects in Location&Channel
        GetComponent<APIController>().listsObject(StaticDatas.Location, StaticDatas.Channel);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)){
            if(StaticDatas.editMode){
                StaticDatas.editMode = false;
                mrtk.SetActive(true);
                CanvasNew.SetActive(false);
                CanvasEdit.SetActive(false);
            } else {
                StaticDatas.editMode = true;
                mrtk.SetActive(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                if(StaticDatas.selectedGO != null){
                    CanvasEdit.SetActive(true);
                    updateHandler.StartupUpdateMode();
                } else {
                    CanvasNew.SetActive(true);
                }
            }
        }
    }

    public void PointerClicked(MixedRealityPointerEventData eventData){
        if(eventData.Pointer.GetType() == typeof(MousePointer)){
            var result = eventData.Pointer.Result;
            if(result.CurrentPointerTarget == null){
                if(StaticDatas.selectedGO != null){
                    var bc = StaticDatas.selectedGO.GetComponent<BoundsControl>();
                    if(bc != null) bc.Active = false;
                }
                StaticDatas.selectedGO = null;
            } else {
                if(StaticDatas.selectedGO != null && StaticDatas.selectedGO != result.CurrentPointerTarget){
                    var bc2 = StaticDatas.selectedGO.GetComponent<BoundsControl>();
                    if(bc2 != null) bc2.Active = false;
                }
                var bc3 = result.CurrentPointerTarget.GetComponent<BoundsControl>();
                if(bc3 != null) bc3.Active = true;
                StaticDatas.selectedGO = result.CurrentPointerTarget;
            }
        }
    }
}
