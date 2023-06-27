// "GameScene"でGameObjectにアタッチされてる想定
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class UIManager : MonoBehaviour {
    private static bool init = false;
    private static UIManager instance;
    public static UIManager Instance => instance;

    const string CurrentScene = "main";
    const string SceneMain = "main";
    const string ScenejigyoujyouList = "jigyoujyouList";
    const string ResourceButton = @"Prefabs\Button";

    public List<SceneData> SceneList = new List<SceneData>();
    public int SceneNo;

    public int Score = 0;

    [RuntimeInitializeOnLoadMethod()]
    static void Init() {
        //シーン切替 .
        //SceneManager.LoadScene("SceneFIX");
    }

    private void Awake() {
        // instanceがすでにあったら自分を消去する。
        if (instance && this != instance) {
            Destroy(this.gameObject);
        }

        instance = this;

        if(init == false) {
            SceneManager.LoadScene("BootScene");
            CreateSceneList();
            init = true;
            SceneNo = 0;
            // シーン切り替え後のスクリプトを取得
            //var controlList = GameObject.FindWithTag("GameManager").GetComponent<ControlList>();
            //controlList.controlList = SceneList[0].controlList;
        }

        // Scene遷移で破棄されなようにする。      
        DontDestroyOnLoad(this);
    }

    private void Start() {
        //UIManager.Instance.SceneList[UIManager.Instance.SceneNo].LoadScene();
    }

    void CreateSceneList() {
        SceneList.Add(new SceneData(SceneMain, "月報くん", null, SceneData.Type.FIX, ControlList));
    }

    private List<Control> ControlList = new List<Control>() {
        new ButtonControl("点検を行う", 4, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
        new ButtonControl("太陽光発電設備\n点検報告書", 2, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
        new ButtonControl("点検を行う", 2, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
        new ButtonControl("点検を行う", 1, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
    };
}
