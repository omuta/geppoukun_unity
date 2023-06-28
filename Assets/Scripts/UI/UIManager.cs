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
    //    SceneList.Add(new SceneData(SceneMain, "月報くん", null, SceneData.Type.FIX, ControlList));
    //}

    //private static List<Control> ControlList = new List<Control>() {
    //    new ButtonControl("点検を行う", 4, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
    //    new ButtonControl("太陽光発電設備\n点検報告書", 2, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
    //    new ButtonControl("点検を行う", 2, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
    //    new ButtonControl("点検を行う", 1, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
    //};
}