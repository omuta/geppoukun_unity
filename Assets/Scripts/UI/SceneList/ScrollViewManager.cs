using EnhancedUI.EnhancedScroller;
using PreGeppou.Data;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewManager : MonoBehaviour, IEnhancedScrollerDelegate {
    private List<JigyousyoData> _data;

    [SerializeField]
    private EnhancedScroller _scroller;
    [SerializeField]
    private CellViewRecord _cellViewPrefab;

    private void Start() {
        // �f�[�^���쐬
        _data = new List<JigyousyoData>();
        for (int i = 0; i < 30; i++) {
            _data.Add(new JigyousyoData("title", "subTitle", "", "", 0, 0, 0, false));
        }

        _scroller.cellViewVisibilityChanged += view => {
            if (view.active) {
                // �Z�����\����ԂɂȂ������̏���
                var cellView = (CellViewRecord)view;
                cellView.SetData(_data[view.dataIndex]);
            }
        };

        // �Z�����C���X�^���X�����ꂽ�Ƃ��̏���
        _scroller.cellViewInstantiated += (scroller, view) => {
            var cellView = (CellViewRecord)view;
            cellView.onClick = x => Debug.Log("Clicked: " + x.dataIndex);
        };

        // Scroller�Ƀf���Q�[�g�o�^
        _scroller.Delegate = this;
        // ReloadData������ƃr���[���X�V�����
        _scroller.ReloadData();
    }

    // �Z���̐���Ԃ�
    public int GetNumberOfCells(EnhancedScroller scroller) {
        return _data.Count;
    }

    // �Z���̃T�C�Y�i�c��or�����j��Ԃ�
    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) {
        return 280;
    }

    // �Z����View��Ԃ�
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex) {
        // Scroller.GetCellView()���ĂԂƐV�K����or���T�C�N���������I�ɍs����View��Ԃ��Ă����
        return scroller.GetCellView(_cellViewPrefab);
    }
}