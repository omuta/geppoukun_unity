using EnhancedUI.EnhancedScroller;
using Newtonsoft.Json;
using PreGeppou.Data;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollViewManager : MonoBehaviour, IEnhancedScrollerDelegate {

    [SerializeField]
    public static EnhancedScroller _scroller;
    [SerializeField]
    public static CellViewJgyousyoRecord _cellViewPrefab;
    public static Button _cellViewPrefabButton;
    public static float _cellViewSize;

    private void Awake() {
        if (UIManager.Instance.getCurrentScene() == null) {
            return;
        }
        gameObject.AddComponent<UIManagerAddComponentJigyousyoList>();
    }

    private void Start() {
        if (UIManager.Instance.getCurrentScene() == null) {
            SceneManager.LoadScene("BootScene");
            return;
        }
        // Scroller�Ƀf���Q�[�g�o�^
        _scroller.Delegate = this;
        // ReloadData������ƃr���[���X�V�����
        _scroller.ReloadData();
    }

    // �Z���̐���Ԃ�
    public int GetNumberOfCells(EnhancedScroller scroller) {
        return UIManagerAddComponentJigyousyoList._data.Count;
    }

    // �Z���̃T�C�Y�i�c��or�����j��Ԃ�
    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) {
        return _cellViewSize;
    }

    // �Z����View��Ԃ�
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex) {
        // Scroller.GetCellView()���ĂԂƐV�K����or���T�C�N���������I�ɍs����View��Ԃ��Ă����
        return scroller.GetCellView(_cellViewPrefab);
    }
}