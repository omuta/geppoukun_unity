using UnityEngine;
using System;

//�V���O���g���e�N���X(�p����)
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
    private static T instance;

    //�O������A�N�Z�X����邽�߂̃v���p�e�B
    public static T Instance {
        get {
            //�C���X�^���X���Ȃ����
            if (instance == null) {
                //�N���X�̌^���擾
                Type type = typeof(T);

                //�������g���C���X�^���X��
                instance = (T)FindObjectOfType(type);

                //�A�^�b�`����Ă��Ȃ���
                if (instance == null) {
                    Debug.LogError(type + " ���A�^�b�`���Ă���GameObject�����݂��܂���");
                }
            }

            return instance;
        }
    }

    virtual protected void Awake() {
        // ���̃Q�[���I�u�W�F�N�g�ɃA�^�b�`����Ă��邩���ׂ�
        // �A�^�b�`����Ă���ꍇ�͔j������B
        CheckInstance();
    }

    protected void CheckInstance() {
        //�C���X�^���X�����݂��Ă��Ȃ���΁A�������g���C���X�^���X��
        if (instance == null) { instance = this as T; return; }

        //�������g�Ȃ牽�����Ȃ�
        else if (Instance == this) { return; }

        //�������g��j������
        Destroy(this);
        return;
    }
}