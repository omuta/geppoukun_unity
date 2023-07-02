using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerAddComponent : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        if(UIManager.Instance.getCurrentScene() == null) {
            //SceneManager.LoadScene("BootScene");
            //return;
        }

        //foreach (var control in UIManager.Instance.getCurrentScene().getControlList()) {
        //    GameObject originObject = Instantiate((GameObject)Resources.Load(control.Prefab), GameObject.Find("Canvas/Panel/PanelBody").transform);
        //    originObject.GetComponent<LayoutElement>().flexibleHeight = ((ButtonControl)control).weight;
        //    originObject.GetComponent<LayoutElement>().flexibleWidth = 1;
        //    originObject.transform.Find("Text").GetComponent<Text>().text = control.Title;
        //    originObject.GetComponent<Button>().onClick.AsObservable()
        //        .Subscribe(count => {
        //            //UIManager.CurrentScene
        //            SceneManager.LoadScene(((ButtonControl)control).MoveScene);
        //        }).AddTo(gameObject);
        //}
    }
}
