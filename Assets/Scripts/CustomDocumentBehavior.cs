using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;

using Orthoverse;
using Orthoverse.DOM;
using Orthoverse.DOM.Entity;

public class CustomDocumentBehavior : MonoBehaviour
{
    public DocumentManager dm;

    void Awake(){
        dm.setPostInitDocumentDelegate(PostInitDocument);
        dm.setPostRenewDocumentDelegate(PostRenewDocument);
        dm.setPostLinkAction(PostLinkAction);
    }

    public void PostInitDocument(Container con, object param){
        var od = (APIController.ObjectData)param;
        var oc = con.gameObject.AddComponent<ObjectController>();
        oc.objectData = od;
        con.gameObject.transform.localPosition = od.position;
        con.gameObject.transform.localScale = od.scale;
        con.gameObject.transform.localRotation = od.rotation;
        con.gameObject.transform.SetParent(StaticDatas.objectRoot.transform, false);
        con.gameObject.transform.hasChanged = false;

        var om = con.gameObject.AddComponent<ObjectManipulator>();
        var bc = con.gameObject.AddComponent<BoundsControl>();
        bc.Target = con.gameObject;
        bc.BoundsOverride = con.gameObject.GetComponent<BoxCollider>();
        bc.BoundsControlActivation = Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes.BoundsControlActivationType.ActivateManually;
    }

    public void PostRenewDocument(Container con){
        var bc = con.gameObject.GetComponent<BoundsControl>();
        bc.BoundsOverride = con.GetCurrent().gameObject.GetComponent<BoxCollider>();
    }

    public void PostLinkAction(EntityBase e){
        var interactable = e.gameObject.AddComponent<Interactable>();
        var newThemeType = ThemeDefinition.GetDefaultThemeDefinition<InteractableScaleTheme>().Value;
        
        newThemeType.StateProperties[0].Values = new List<ThemePropertyValue>()
        {
            new ThemePropertyValue() { Vector3 = new Vector3(1f,1f,1f)},
            new ThemePropertyValue() { Vector3 = new Vector3(1.1f,1.1f,1.1f)},
            new ThemePropertyValue() { Vector3 = new Vector3(0.9f,0.9f,0.9f)},
            new ThemePropertyValue() { Vector3 = new Vector3(1f,1f,1f)},
        };

        interactable.Profiles = new List<InteractableProfileItem>()
        {
            new InteractableProfileItem()
            {
                Themes = new List<Theme>()
                {
                    Interactable.GetDefaultThemeAsset(new List<ThemeDefinition>() { newThemeType })
                },
                Target = e.gameObject,
            },
        };
        interactable.TriggerOnClick();
        
        interactable.OnClick.AddListener(() => e.event_click());
    }
}
