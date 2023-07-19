using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : SingletonMonoBehaviour<UIManager> {
    private static bool IsBoot = false;
    public static string CurrentScene = "main";
    const string SceneMain = "main";
    const string ScenejigyoujyouList = "jigyoujyouList";
    const string ScenejigyoujyouMenu = "jigyoujyouMenu";
    const string ResourceButton = @"Prefabs\Button";
    public static List<SceneData> SceneList = new List<SceneData>();

    virtual protected void Awake() {
        //if (UIManager.Instance.getCurrentScene() == null) {
        //    CreateSceneList();
        //    SceneManager.LoadScene("BootScene");
        //    return;
        //}

        base.Awake();
        CreateControlList();
        CreateSceneList();
    }

    public void CreateSceneList() {
        SceneList.Add(new SceneData(SceneMain, "���񂭂�", null, SceneData.Type.FIX, main));
        SceneList.Add(new SceneData(ScenejigyoujyouList, "���Ə�ꗗ", SceneMain, SceneData.Type.List, jigyoujyouList));
    }

    List<Control> main;
    List<Control> jigyoujyouList;
    public void CreateControlList() {
        main = new List<Control>() {
                    new ButtonControl("�_�����s��", 4, ScenejigyoujyouList, ResourceButton),
                    new ButtonControl("���z�����d�ݔ�\n�_���񍐏�", 2, ScenejigyoujyouList, ResourceButton),
                    new ButtonControl("�N���_�����s��", 2, ScenejigyoujyouList, ResourceButton),
                    new ButtonControl("�e��ݒ�", 1, ScenejigyoujyouList, ResourceButton),
        };

        jigyoujyouList = new List<Control>() {
                    new ListControl(ScenejigyoujyouMenu, ResourceButton),
        };
    }

    public SceneData getCurrentScene() {
        SceneData SceneType = null;
        if (IsBoot == false) {
            IsBoot = true;
            return SceneType;
        }
        // �N�����̉��
        foreach (var scene in SceneList) {
            if(scene.SceneName == CurrentScene) {
                SceneType = scene;
                break;
            }
        }
        return SceneType;
    }
}