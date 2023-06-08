using Outgame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEditor.SearchService;
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
    List<ViewID> _uiSceneHistory = new List<ViewID>();
    Stack<UIStackableView> _uiStack = new Stack<UIStackableView>();

    Dictionary<ViewID, GameObject> _sceneCache = new Dictionary<ViewID, GameObject>(); 

    public static void Setup(ViewID entry)
    {
        _instance._uiStack.Clear();

        //スタックビューはHomeからはいる
        var rootCanvas = GameObject.FindObjectOfType<Canvas>();
        _instance._root = rootCanvas.GetComponent<RectTransform>();
        _instance.LoadScene(entry);
    }

    public static void Back()
    {
        if (!_instance._current) return;

        _instance._current.Exit();
        if (_instance._uiStack.Count > 0)
        {
            _instance._current = _instance._uiStack.Pop();
            _instance._current.Enter();
        }
    }

    void LoadScene(ViewID next)
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

        view.Enter();
        _current = view;
        //todo:

        Addressables.Release(sceneOrigin);
    }

    public static void NextView(ViewID next)
    {
        _instance._current?.Exit();

        _instance._uiSceneHistory.Add(next);
        _instance.LoadScene(next);
    }

    public static void StackModalView(ViewID next)
    {
        _instance._current?.Exit();

        _instance._uiStack.Push(_instance._current);
        if (_instance._uiStack.Count > 0)
        {
            _instance._current = _instance._uiStack.Pop();
            _instance._current.Enter();
        }
    }

}
