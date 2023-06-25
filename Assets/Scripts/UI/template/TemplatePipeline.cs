using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExampleTemplatePipeline : ISceneTemplatePipeline {
    // �L���ȃe���v���[�g���ǂ�����Ԃ�
    public bool IsValidTemplateForInstantiation(SceneTemplateAsset sceneTemplateAsset) {
        Debug.Log($"{nameof(IsValidTemplateForInstantiation)} - sceneTemplateAsset: {sceneTemplateAsset.name}");
        return true;
    }

    // �e���v���[�g����V�[�����쐬�����O�̃R�[���o�b�N
    public void BeforeTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, bool isAdditive, string sceneName) {
        Debug.Log($"{nameof(BeforeTemplateInstantiation)} - isAdditive: {isAdditive} sceneName: {sceneName}");
    }

    // �e���v���[�g����V�[�����쐬���ꂽ��̃R�[���o�b�N
    public void AfterTemplateInstantiation(SceneTemplateAsset sceneTemplateAsset, UnityEngine.SceneManagement.Scene scene, bool isAdditive, string sceneName) {
        Debug.Log($"{nameof(AfterTemplateInstantiation)} - scene: {scene} isAdditive: {isAdditive} sceneName: {sceneName}");
    }
}