using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    //public static List<SceneData> SceneList = new List<SceneData>();


    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    //const string SceneMain = "main";
    //const string ScenejigyoujyouList = "jigyoujyouList";
    //const string ResourceButton = @"Prefabs\Button";
    //void CreateSceneList() {
    //    SceneList.Add(new SceneData(SceneMain, "���񂭂�", null, SceneData.Type.FIX, ControlList));
    //}

    //private static List<Control> ControlList = new List<Control>() {
    //    new ButtonControl("�_�����s��", 4, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
    //    new ButtonControl("���z�����d�ݔ�\n�_���񍐏�", 2, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
    //    new ButtonControl("�_�����s��", 2, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
    //    new ButtonControl("�_�����s��", 1, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
    //};
}