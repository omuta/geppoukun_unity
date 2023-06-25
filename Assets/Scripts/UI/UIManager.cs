using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager {
    // �v���n�u���i�[����
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

        // �N�����̉��
        //foreach (var control in SceneList[0].controlList) {
        //    var Object = guidReference.m_guidReference;
        //    //control.Prefab.transform.SetParent(guidReference.m_guidReference);
        //}

        //var PanelBody = GameObject.Find("Canvas/Panel/PanelBody");
        // �N�����̉��
        //foreach (var control in SceneList[0].controlList) {
        //    control.Prefab.transform.parent = PanelBody.transform;
        //}
        //SceneManager.LoadScene("SceneFIX_");
    }

    List<SceneController> SceneList = new List<SceneController>();
    void CreateSceneList() {
        SceneList.Add(new SceneController(SceneMain, "���񂭂�", null, SceneController.Type.FIX, controlMain));
    }

    private List<Control> controlMain = new List<Control>() {
        new ButtonControl("�_�����s��", 4, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
        new ButtonControl("���z�����d�ݔ�\n�_���񍐏�", 2, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
        new ButtonControl("�_�����s��", 2, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
        new ButtonControl("�_�����s��", 1, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
    };

}
