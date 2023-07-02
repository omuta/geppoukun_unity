using EnhancedUI.EnhancedScroller;
using PreGeppou.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellViewRecord : EnhancedScrollerCellView {
    [SerializeField]
    private Text _title;
    [SerializeField]
    private Text _subtitle;

    public Action<CellViewRecord> onClick;

    public void SetData(JigyousyoData data) {
        _title.text = data.textTitle;
        _subtitle.text = data.textSubTitle;
    }

    private void Awake() {
        //_button.onClick.AddListener(() => onClick?.Invoke(this));
    }
}