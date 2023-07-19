using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootScene : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        //UIManager.Instance.CreateSceneList();
        if(UIManager.Instance.getCurrentScene()  == null) {
            SceneManager.LoadScene("BootScene");
        }
        SceneManager.LoadScene(UIManager.Instance.getCurrentScene().GetType());
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
