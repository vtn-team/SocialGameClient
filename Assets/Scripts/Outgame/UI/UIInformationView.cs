using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Outgame
{
    public class UIInformationView : UIStackableView
    {
        public enum ViewMode
        {
            ListView,
            ContentView,
        }

        [SerializeField] TMPro.TextMeshProUGUI _title;
        [SerializeField] GameObject _contentRect;
        [SerializeField] UIView _backButton;

        [SerializeField] GameObject _listPrefab;

        ViewMode _viewMode = ViewMode.ListView;
        List<GameObject> _infoList = new List<GameObject>();

        private void Start()
        {
            CreateListView();
        }

        void CreateListView()
        {
            _infoList.ForEach(info => GameObject.Destroy(info));
            _backButton.Disactive();

            _title.text = "お知らせ"; //TODO

            UniTask.RunOnThreadPool(async () =>
            {
                var list = await InformationModel.LoadListAsync();

                UniTask.Post(() =>
                {
                    _infoList.Clear();
                    foreach (var info in list.list)
                    {
                        var listItem = GameObject.Instantiate(_listPrefab, _contentRect.transform);
                        var script = listItem.GetComponent<InformationListItem>();
                        script.Setup(info, SelectInfo);
                        _infoList.Add(listItem);
                    }
                });
            }).Forget();
        }

        void SelectInfo(string id)
        {
            _infoList.ForEach(info => GameObject.Destroy(info));
            _backButton.Active();
            UniTask.RunOnThreadPool(async () =>
            {
                var content = await InformationModel.LoadContentAsync(id);

                UniTask.Post(() =>
                {
                    _title.text = content.title;

                    _infoList.Clear();
                    foreach (var item in content.contents)
                    {
                        _infoList.Add(NotionBlock.CreateBlock(item, _contentRect.transform));
                    }
                });
            }).Forget();
        }

        public void GoBack()
        {
            CreateListView();
        }

        public void CloseInfo()
        {
            UIManager.Back();
        }
    }
}
