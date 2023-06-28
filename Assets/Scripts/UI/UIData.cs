using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIData : MonoBehaviour {

    const string SceneMain = "main";
    const string ScenejigyoujyouList = "jigyoujyouList";
    const string ResourceButton = @"Prefabs\Button";
    public static List<SceneData> SceneList = new List<SceneData>();

    void CreateSceneList() {
        SceneList.Add(new SceneData(SceneMain, "���񂭂�", null, SceneData.Type.FIX, ControlList));
    }

    private static List<Control> ControlList = new List<Control>() {
        new ButtonControl("�_�����s��", 4, ScenejigyoujyouList, ResourceButton),
        new ButtonControl("���z�����d�ݔ�\n�_���񍐏�", 2, ScenejigyoujyouList, ResourceButton),
        new ButtonControl("�_�����s��", 2, ScenejigyoujyouList, ResourceButton),
        new ButtonControl("�_�����s��", 1, ScenejigyoujyouList, ResourceButton),
    };
}
