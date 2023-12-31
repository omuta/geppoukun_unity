using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneData {
    public enum Type { FIX, Scroll, List}

    public string SceneName;
    public string Title;
    public string PrevScene;
    public Type type;
    public List<Control> controlList = new List<Control>();
    readonly GameObject PanelBody = GameObject.Find("Canvas/Panel/PanelBody");

    public SceneData(string SceneName, string Title, string PrevScene, Type type, List<Control> controlList) {
        this.SceneName = SceneName;
        this.Title = Title;
        this.PrevScene = PrevScene;
        this.type = type;
        this.controlList = controlList;
    }

    public List<Control> getControlList() {
        return controlList;
    }

    public string getSceneName() {
        // 起動時の画面
        //foreach (var control in controlList) {
        //    control.Prefab.transform.SetParent(PanelBody.transform);
        //    control.Prefab.transform.parent = PanelBody.transform;
        //}
        return GetType();
    }

    public string GetType() {
        string[] SceneNames = { "SceneFIX", "SceneScroll", "SceneList" };
        return SceneNames[(int)type];
    }
}
