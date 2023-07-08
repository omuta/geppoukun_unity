using PaperPlaneTools;
using UnityEngine;
using UnityEngine.UI;

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
        new Alert("debug1", "CallAndroidPlugin").SetPositiveButton("OK").Show();
        using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("unity.library.UnityPlugin")) { 
        //using (var activity = androidJavaClass.GetStatic<AndroidJavaObject>("UnityPlugin")) {
            txtMessage.text = androidJavaClass.CallStatic<string>("FromUnity", counter.ToString());
            //txtMessage.text = androidJavaClass.Call<string>("getAccounts");  //.CallStatic<string>("FromUnity", counter.ToString());
            new Alert("debug2", $"txtMessage.text : {txtMessage.text}").SetPositiveButton("OK").Show();
            counter++;
        }
        new Alert("debug3", "CallAndroidPlugin").SetPositiveButton("OK").Show();
    }

    /// <summary>
    /// �v���O�C��������Unity���Ă�
    /// </summary>
    public void FromAndroid(string str) {
        txtMessage.text += str;
    }
}
