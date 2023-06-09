using Outgame;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static bool Back()
    {
        if (!_instance._current) return false;

        _instance._current.Exit();
        if (_instance._uiStack.Count > 0)
        {
            _instance._current = _instance._uiStack.Pop();
            //_instance._current.Enter();
            return true;
        }
        return false;
    }

    void LoadSceneStack(ViewID next)
    {
        LoadScene(next, true);
    }

    void LoadScene(ViewID next, bool isStack = false)
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

        if (!isStack)
        {
            //todo: スタックしないケースの場合でも、スタックして後で壊すほうがいい
            GameObject.Destroy(_current);
        }

        _current = view;

        //todo:

        Addressables.Release(sceneOrigin);
    }

    public static void NextView(GameObject origin)
    {
        var scene = GameObject.Instantiate(origin, _instance._root);
        var view = scene.GetComponent<UIStackableView>();
        if (view == default)
        {
            Debug.LogError($"シーン管理スクリプトの読み込みに失敗");
            return;
        }

        _instance._current?.Exit();
        GameObject.Destroy(_instance._current);

        //ビューを直接指定する場合はヒストリには書き込まない
        //_instance._uiSceneHistory.Add(next);

        _instance._current = view;
        _instance._current?.Enter();
    }

    public static void NextView(ViewID next)
    {
        _instance._current?.Exit();

        _instance._uiSceneHistory.Add(next);
        _instance.LoadScene(next);
    }

    public static void StackView(ViewID next)
    {
        _instance._uiStack.Push(_instance._current);
        _instance.LoadSceneStack(next);
    }
}
