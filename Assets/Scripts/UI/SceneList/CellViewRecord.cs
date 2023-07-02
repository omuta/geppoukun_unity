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

    public CellViewRecord(string title, string subtitle) {
        _title.text = title;
        _subtitle.text = subtitle;
    }

    public void SetData(CellViewRecord data) {
        _title.text = data._title.text;
        _subtitle.text = data._subtitle.text;
    }

    private void Awake() {
        //_button.onClick.AddListener(() => onClick?.Invoke(this));
    }
}