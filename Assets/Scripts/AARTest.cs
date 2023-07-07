using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class AARTest : MonoBehaviour
{
    [SerializeField] Text txtMessage;
    [SerializeField] Button btnAndroid;
    int counter = 0;

    void Start() {
        btnAndroid.onClick.AddListener(CallAndroidPlugin);
        txtMessage.text = "�����l";
    }

    /// <summary>
    /// �{�^���������ꂽ��v���O�C�����Ă�
    /// </summary>
    public void CallAndroidPlugin() {
        using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("Unity.Plugin")) {
            txtMessage.text = androidJavaClass.CallStatic<string>("FromUnity", counter.ToString());
            counter++;
        }
    }

    /// <summary>
    /// �v���O�C��������Unity���Ă�
    /// </summary>
    public void FromAndroid(string str) {
        txtMessage.text += str;
    }
}
