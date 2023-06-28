using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : SingletonMonoBehaviour<UIManager> {
    string CurrentScene = "main";
    const string SceneMain = "main";
    const string ScenejigyoujyouList = "jigyoujyouList";
    const string ResourceButton = @"Prefabs\Button";
    public static List<SceneData> SceneList = new List<SceneData>();

    public void CreateSceneList() {
        SceneList.Add(new SceneData(SceneMain, "月報くん", null, SceneData.Type.FIX, init));
    }

    List<Control> init = new List<Control>() {
                    new ButtonControl("点検を行う", 4, ScenejigyoujyouList, ResourceButton),
                    new ButtonControl("太陽光発電設備\n点検報告書", 2, ScenejigyoujyouList, ResourceButton),
                    new ButtonControl("点検を行う", 2, ScenejigyoujyouList, ResourceButton),
                    new ButtonControl("点検を行う", 1, ScenejigyoujyouList, ResourceButton),
        };

    public SceneData getCurrentScene() {
        SceneData SceneType = null;
        // 起動時の画面
        foreach (var sceneList in SceneList) {
            if(sceneList.SceneName == CurrentScene) {
                SceneType = sceneList;
                break;
            }
        }
        return SceneType;
    }
}