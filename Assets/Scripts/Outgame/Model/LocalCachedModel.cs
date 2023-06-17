using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Outgame
{
    public abstract class LocalCachedModel<T,D> where T : LocalCachedModel<T,D>, new()
    {
        static protected T _instance = new T();
        public static T Instance { get { return _instance; } }
        protected LocalCachedModel() { Setup(); }

        protected D _data = default(D);
        protected string _dataName = null;

        //データの公開メソッドは派生先で宣言してください
        //////

        static public bool HasData => _instance.hasData;
        static public D Load() => _instance.load();
        //static public async UniTask<D> LoadAsync() => await _instance.loadAsync();
        static public void Save() => _instance.save();


        protected virtual D load()
        {
            if (hasData) return _data;

            _data = LocalData.Load<D>(_dataName, GameSetting.SavePath, true);
            if (!hasData)
            {
                //データがなかった場合
            }

            return _data;
        }

        protected virtual async UniTask<D> loadAsync()
        {
            if (hasData) return _data;

            _data = await LocalData.LoadAsync<D>(_dataName, GameSetting.SavePath, true);
            if (!hasData)
            {
                //データがなかった場合
            }

            return _data;
        }

        protected virtual void save()
        {
            LocalData.Save<D>(_dataName, _data, GameSetting.SavePath, true);
        }

        public virtual bool hasData => _data != null;

        /// <summary>
        /// NOTE: _dataNameを定義しつつ、必要な情報を初期化してください
        /// </summary>
        protected abstract void Setup();
    }
}
