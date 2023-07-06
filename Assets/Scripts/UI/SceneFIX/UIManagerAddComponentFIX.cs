using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerAddComponentFIX : MonoBehaviour {
    // Start is called before the first frame update
    [System.Obsolete]
    void Start() {
        if(UIManager.Instance.getCurrentScene() == null) {
            SceneManager.LoadScene("BootScene");
            return;
        }

        foreach (var control in UIManager.Instance.getCurrentScene().getControlList()) {
            GameObject originObject = Instantiate((GameObject)Resources.Load(control.Prefab), GameObject.Find("Canvas/Panel/PanelBody").transform);
            originObject.GetComponent<LayoutElement>().flexibleHeight = ((ButtonControl)control).weight;
            originObject.GetComponent<LayoutElement>().flexibleWidth = 1;
            originObject.GetComponentInChildren<Text>().text = ((ButtonControl)control).Title;
            originObject.GetComponent<Button>().onClick.AsObservable()
                .Subscribe(count => {
                    UIManager.CurrentScene = ((ButtonControl)control).MoveScene;
                    SceneManager.LoadScene(UIManager.Instance.getCurrentScene().GetType());
                }).AddTo(gameObject);
        }
    }
}
