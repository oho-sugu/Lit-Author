using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadMain : MonoBehaviour
{
    public GameObject dropdownLocation;
    public GameObject dropdownChannel;

    public void PushStart(){
        StaticDatas.Location = dropdownLocation.GetComponent<Dropdown>().captionText.text;
        StaticDatas.Channel = dropdownChannel.GetComponent<Dropdown>().captionText.text;
        StaticDatas.editMode = false;

        SceneManager.LoadScene("Main");
    }
}
