using EnhancedUI.EnhancedScroller;
using Newtonsoft.Json;
using PreGeppou.Data;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerAddComponentJigyousyoList : MonoBehaviour {
    public static List<JigyousyoData> _data;
    // Start is called before the first frame update
    [System.Obsolete]
    void Start() {
        if(UIManager.Instance.getCurrentScene() == null) {
            SceneManager.LoadScene("BootScene");
            return;
        }

        var path = Path.Combine(Application.persistentDataPath, JigyousyoData.FileName);
        if (File.Exists(path)) {
            string readData = File.ReadAllText(path);
            _data = JsonConvert.DeserializeObject<List<JigyousyoData>>(readData);
        } else {
            _data = new List<JigyousyoData>();
            _data.Add(new JigyousyoData("���Əꖼ", "�x���x�X��", "", "", 0, 0, 0, false));
        }

        // �f�[�^���쐬
        ScrollViewManager._scroller = GameObject.Find("Canvas/Panel/ScrollArea/Scroller").GetComponent<EnhancedScroller>();
        ScrollViewManager._cellViewPrefab = Resources.Load<CellViewJgyousyoRecord>("Prefabs/JigyoujyouContent");
        //foreach (var d in _data) {
        //    ScrollViewManager._cellViewPrefab.GetComponentInChildren<Button>().onClick.AddListener(() => {
        //        Debug.Log("�N���b�N���ꂽ");
        //    });
        //}
        //ScrollViewManager._cellViewPrefab.GetComponentInChildren<Button>().onClick.AsObservable()
        //    .Subscribe(count => {
        //        SceneManager.LoadScene("Main");
        //    }).AddTo(gameObject);

        ScrollViewManager._scroller.cellViewVisibilityChanged += view => {
            if (view.active) {
                // �Z�����\����ԂɂȂ������̏���
                var cellView = (CellViewJgyousyoRecord)view;
                cellView.SetData(_data[view.dataIndex]);
            }
        };

        // �Z�����C���X�^���X�����ꂽ�Ƃ��̏���
        ScrollViewManager._scroller.cellViewInstantiated += (scroller, view) => {
            var cellView = (CellViewJgyousyoRecord)view;
            cellView.onClick = x => { 
                Debug.Log("Clicked: " + x.dataIndex); 
            };
        };

        ScrollViewManager._cellViewSize = 320;
    }
}
