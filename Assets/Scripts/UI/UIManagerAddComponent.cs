using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerAddComponent : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        foreach (var control in UIManager.Instance.getCurrentScene().getControlList()) {
           // TODO
            ((GameObject)Resources.Load(control.Prefab)).transform.parent = GameObject.Find("Canvas/Panel/PanelBody").transform;
        }
    }
}
