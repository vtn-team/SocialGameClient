using Outgame;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class UIManager
{
    static UIManager _instance = new UIManager();
    //public static UIManager Instance => _instance;
    private UIManager() { }

    RectTransform _root;
    UIStackableView _current = null;
    Stack<UIInformationBase> _uiSceneHistory = new Stack<UIInformationBase>();
    Stack<UIStackableView> _uiStack = new Stack<UIStackableView>();

    Dictionary<ViewID, GameObject> _sceneCache = new Dictionary<ViewID, GameObject>(); 


    public static void Setup(ViewID entry)
    {
        _instance._uiStack.Clear();

        var rootCanvas = GameObject.FindObjectOfType<Canvas>();
        _instance._root = rootCanvas.GetComponent<RectTransform>();
        _instance.LoadScene(entry, null);
    }

    public static bool Back()
    {
        if (!_instance._current) return false;

        _instance._current.Exit();

        if (_instance._uiStack.Count > 0)
        {
            GameObject.Destroy(_instance._current.gameObject);

            _instance._current = _instance._uiStack.Pop();
            //_instance._current.Enter();
            return true;
        }
        else
        {
            if (_instance._uiSceneHistory.Count >= 2)
            {
                //今いたページのページ履歴を消す
                _instance._uiSceneHistory.Pop();

                //その前のページに戻る 
                var info = _instance._uiSceneHistory.Pop();
                NextView(info.ViewID, info);
            }
            else
            {
                NextView(ViewID.Home);
            }
        }

        return false;
    }

    void LoadSceneStack(ViewID next, UIInformationBase info)
    {
        LoadScene(next, info, true);
    }

    void LoadScene(ViewID next, UIInformationBase info, bool isStack = false)
    {
        GameObject sceneOrigin = null;
        if (_sceneCache.ContainsKey(next))
        {
            sceneOrigin = _sceneCache[next];
        }
        else
        {
            sceneOrigin = Addressables.LoadAssetAsync<GameObject>(string.Format("Assets/Scenes/Game/UI/{0}.prefab", next.ToString())).WaitForCompletion();
        }

        if (sceneOrigin == default)
        {
            Debug.LogError($"{next.ToString()}: シーンの読み込みに失敗");
            return;
        }

        var scene = GameObject.Instantiate(sceneOrigin, _root);
        var view = scene.GetComponent<UIStackableView>();
        if (view == default)
        {
            Debug.LogError($"{next.ToString()}: シーン管理スクリプトの読み込みに失敗");
            return;
        }
        
        CreateUIParts(next);

        view.Enter();

        if (!isStack && _current)
        {
            //todo: スタックしないケースの場合でも、スタックして後で壊すほうがいい
            GameObject.Destroy(_current.gameObject);
        }

        _current = view;
        _current.SetInformation(info);

        //todo:

        Addressables.Release(sceneOrigin);
    }

    public static void NextView(GameObject origin, UIInformationBase info = null)
    {
        var scene = GameObject.Instantiate(origin, _instance._root);
        var view = scene.GetComponent<UIStackableView>();
        if (view == default)
        {
            Debug.LogError($"シーン管理スクリプトの読み込みに失敗");
            return;
        }

        _instance._current?.Exit();
        GameObject.Destroy(_instance._current.gameObject);

        //ビューを直接指定する場合はヒストリには書き込まない
        //_instance._uiSceneHistory.Add(next);

        _instance._current = view;
        _instance._current?.Enter();
    }

    public static void NextView(ViewID next, UIInformationBase info = null)
    {
        _instance._current?.Exit();

        if (info != null)
        {
            info.ViewID = next;
            _instance._uiSceneHistory.Push(info);
        }
        else
        {
            _instance._uiSceneHistory.Push(new UIInformationBase() { ViewID = next});
        }
        _instance.LoadScene(next, info);
    }

    public static void StackView(ViewID next, UIInformationBase info = null)
    {
        _instance._uiStack.Push(_instance._current);
        _instance.LoadSceneStack(next, info);
    }

    void CreateUIParts(ViewID next)
    {
        switch(next)
        {
            case ViewID.Home:
                if(UIStatusBar.IsNull)
                {
                    var stOrigin = Addressables.LoadAssetAsync<GameObject>("Assets/Scenes/Game/UI/Status.prefab").WaitForCompletion();
                    GameObject.Instantiate(stOrigin, _root);
                    Addressables.Release(stOrigin);
                }
                break;
        }
    }
}
