using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController {
    public enum Type { FIX, Scroll, List}

    public string SceneName;
    public string Title;
    public string PrevScene;
    public Type type;
    public List<Control> controlList = new List<Control>();
    readonly GameObject PanelBody = GameObject.Find("Canvas/Panel/PanelBody");

    public SceneController(string SceneName, string Title, string PrevScene, Type type, List<Control> controlList) {
        this.SceneName = SceneName;
        this.Title = Title;
        this.PrevScene = PrevScene;
        this.type = type;
        this.controlList = controlList;
    }

    public void LoadScene() {
        // �N�����̉��
        //foreach (var control in controlList) {
        //    control.Prefab.transform.SetParent(PanelBody.transform);
        //    control.Prefab.transform.parent = PanelBody.transform;
        //}
        SceneManager.LoadScene(GetName(type));
    }

    public string GetName(Type type) {
        string[] dayNames = { "SceneFIX", "SceneScroll", "SceneList" };
        return dayNames[(int)type];
    }
}