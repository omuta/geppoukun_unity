using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control {
    public string Title;
    public GameObject Prefab;

    public Control(string Title, GameObject Prefab) {
        this.Title = Title;
        this.Prefab = Prefab;
    }
}

public class ButtonControl : Control {
    public int weight;
    public string MoveScene;

    public ButtonControl(string Title, int weight, string MoveScene, GameObject Prefab) : base(Title, Prefab) {
        this.weight = weight;
        this.MoveScene = MoveScene;
    }
}

public class JigyoujyouControl : Control {
    public string SubTitle;
    public string MoveScene;

    public JigyoujyouControl(string Title, string SubTitle, string MoveScene, GameObject Prefab) : base(Title, Prefab) {
        this.SubTitle = SubTitle;
        this.MoveScene = MoveScene;
    }
}