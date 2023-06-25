using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExampleTemplatePipeline : ISceneTemplatePipeline {
    // 有効なテンプレートかどうかを返す
    public bool IsValidTemplateForInstantiation(SceneTemplateAsset sceneTemplateAsset) {
        Debug.Log($"{nameof(IsValidTemplateForInstantiation)} - sceneTemplateAsset: {sceneTemplateAsset.name}");
        return true;
    }

    // テンプレートからシーンが作成される前のコールバック
    public void BeforeTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, bool isAdditive, string sceneName) {
        Debug.Log($"{nameof(BeforeTemplateInstantiation)} - isAdditive: {isAdditive} sceneName: {sceneName}");
    }

    // テンプレートからシーンが作成された後のコールバック
    public void AfterTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, UnityEngine.SceneManagement.Scene scene, bool isAdditive, string sceneName) {
        Debug.Log($"{nameof(AfterTemplateInstantiation)} - scene: {scene} isAdditive: {isAdditive} sceneName: {sceneName}");
    }
}