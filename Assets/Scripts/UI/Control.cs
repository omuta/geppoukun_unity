using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control {
    public string Prefab;

    public Control(string Prefab) {
        this.Prefab = Prefab;
    }
}

public class ButtonControl : Control {
    public string Title;
    public int weight;
    public string MoveScene;

    public ButtonControl(string Title, int weight, string MoveScene, string Prefab) : base(Prefab) {
        this.Title = Title;
        this.weight = weight;
        this.MoveScene = MoveScene;
    }
}

public class ListControl : Control {
    public string sourceFile;
    public string MoveScene;

    public ListControl(string MoveScene, string Prefab) : base(Prefab) {
        this.MoveScene = MoveScene;
    }
}

public class JigyoujyouControl : ListControl {

    public JigyoujyouControl(string MoveScene, string Prefab) : base(MoveScene, Prefab) {
    }
}
