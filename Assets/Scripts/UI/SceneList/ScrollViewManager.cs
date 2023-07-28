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
        // Scrollerにデリゲート登録
        _scroller.Delegate = this;
        // ReloadDataをするとビューが更新される
        _scroller.ReloadData();
    }

    // セルの数を返す
    public int GetNumberOfCells(EnhancedScroller scroller) {
        return UIManagerAddComponentJigyousyoList._data.Count;
    }

    // セルのサイズ（縦幅or横幅）を返す
    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) {
        return _cellViewSize;
    }

    // セルのViewを返す
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex) {
        // Scroller.GetCellView()を呼ぶと新規生成orリサイクルを自動的に行ったViewを返してくれる
        return scroller.GetCellView(_cellViewPrefab);
    }
}