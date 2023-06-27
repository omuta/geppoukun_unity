// "GameScene"��GameObject�ɃA�^�b�`����Ă�z��
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class UIManager : MonoBehaviour {
    private static bool init = false;
    private static UIManager instance;
    public static UIManager Instance => instance;

    static string CurrentScene = "main";
    const string SceneMain = "main";
    const string ScenejigyoujyouList = "jigyoujyouList";
    const string ResourceButton = @"Prefabs\Button";

    public static List<SceneData> SceneList = new List<SceneData>();
    public int SceneNo;

    public int Score = 0;

    [RuntimeInitializeOnLoadMethod()]
    static void Init() {
        //�V�[���ؑ� .
        //SceneManager.LoadScene("BootScene");
    }

    private void Awake() {
        // instance�����łɂ������玩������������B
        if (instance && this != instance) {
            Destroy(this.gameObject);
        }

        instance = this;

        if (init == false) {
            SceneManager.LoadScene("BootScene");
            CreateSceneList();
            init = true;
            SceneNo = 0;
        }

        //if(init == false) {
        //    SceneManager.LoadScene("BootScene");
        //    CreateSceneList();
        //    init = true;
        //    SceneNo = 0;
        //    // �V�[���؂�ւ���̃X�N���v�g���擾
        //    //var controlList = GameObject.FindWithTag("GameManager").GetComponent<ControlList>();
        //    //controlList.controlList = SceneList[0].controlList;
        //}

        // Scene�J�ڂŔj������Ȃ悤�ɂ���B      
        DontDestroyOnLoad(this);
    }

    private void Start() {
        string currentScene = getCurrentScene();
        if (currentScene == string.Empty) {
            return;
        }
        //SceneManager.LoadScene(currentScene);
        //UIManager.Instance.SceneList[UIManager.Instance.SceneNo].LoadScene();
    }

    public static string getCurrentScene() {
        foreach (SceneData sceneData in SceneList) {
            if(CurrentScene == sceneData.SceneName) {
                return sceneData.getSceneName();
            }
        }
        return string.Empty;
    }

    void CreateSceneList() {
        SceneList.Add(new SceneData(SceneMain, "���񂭂�", null, SceneData.Type.FIX, ControlList));
    }

    private List<Control> ControlList = new List<Control>() {
        new ButtonControl("�_�����s��", 4, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
        new ButtonControl("���z�����d�ݔ�\n�_���񍐏�", 2, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
        new ButtonControl("�_�����s��", 2, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
        new ButtonControl("�_�����s��", 1, ScenejigyoujyouList, (GameObject)Resources.Load (ResourceButton)),
    };
}
