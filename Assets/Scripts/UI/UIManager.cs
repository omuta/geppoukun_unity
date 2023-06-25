using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager {
    // プレハブを格納する
    public static GameObject ButtonPrefab;
    const string SceneMain = "main";
    const string ScenejigyoujyouList = "jigyoujyouList";
    const string ResourceButton = @"Prefabs\Button";

    public void Execute() {
        CreateSceneList();
        SceneList[0].LoadScene();

        var canvas = GameObject.Find("Canvas");
        var guidReference = GameObject.Find("Canvas").GetComponent<DataReferrence>();
        var control = guidReference.m_guidReference.gameObject.AddComponent<ControlList>();
        control.controlList = SceneList[0].controlList;
        //SceneList[0].controlList;

        // 起動時の画面
        //foreach (var control in SceneList[0].controlList) {
        //    var Object = guidReference.m_guidReference;
        //    //control.Prefab.transform.SetParent(guidReference.m_guidReference);
        //}

        //var PanelBody = GameObject.Find("Canvas/Panel/PanelBody");
        // 起動時の画面
        //foreach (var control in SceneList[0].controlList) {
        //    control.Prefab.transform.parent = PanelBody.transform;
        //}
        //SceneManager.LoadScene("SceneFIX_");
    }

    List<SceneController> SceneList = new List<SceneController>();
    void CreateSceneList() {
        SceneList.Add(new SceneController(SceneMain, "月報くん", null, SceneController.Type.FIX, controlMain));
    }

    private List<Control> controlMain = new List<Control>() {
        new ButtonControl("点検を行う", 4, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
        new ButtonControl("太陽光発電設備\n点検報告書", 2, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
        new ButtonControl("点検を行う", 2, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
        new ButtonControl("点検を行う", 1, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
    };

}
