using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;

public class BootScene : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        //UIManager.Instance.SceneList[UIManager.Instance.SceneNo].LoadScene();

        //UIManager uIManager = new UIManager();
        //uIManager.Execute();
        //SceneManager.LoadScene("SceneFIX_");
        //CreateTemplateFromScene();
    }

    //private const string SourceScenePath = "Assets/Scene/Main.unity";
    //private const string TemplatePath = "Assets/Scene/SceneFIX.scenetemplate";

    //[MenuItem("Example/CreateTemplateFromScene")]
    //private static void CreateTemplateFromScene() {
    //    // �V�[������e���v���[�g�A�Z�b�g���쐬����
    //    var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(SourceScenePath);
    //    SceneTemplateService.CreateTemplateFromScene(sceneAsset, TemplatePath);
    //}

    //[MenuItem("Example/Instantiate")]
    //private static void Instantiate() {
    //    // �e���v���[�g�A�Z�b�g����V�[�����쐬����
    //    var sceneTemplateAsset = AssetDatabase.LoadAssetAtPath<SceneTemplateAsset>(TemplatePath);
    //    SceneTemplateService.Instantiate(sceneTemplateAsset, false);
    //}
}
