using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Network
{
    /// <summary>
    /// リクエスト処理クラス
    /// </summary>
    public class WebRequest
    {
        //データ処理デリゲート
        public delegate void GetData(byte[] result);

        //メソッド
        public enum RequestMethod
        {
            GET,
            POST
        }

        //このクラスはstatic的に機能する
        static WebRequest Instance= new WebRequest();

        //リクエストを処理するワーカークラス
        HTTPRequest[] Worker = null;

        /// <summary>
        /// ワーカー設定
        /// </summary>
        static void CheckInstance()
        {
            //ワーカー存在チェック
            if (Instance.Worker != null && Instance.Worker.All(w => w != null)) return;

            //冷えらる気にあるリクエスト処理ワーカーを拾ってくる
            //NOTE: DontDestroyには無くても動作する。Taskに出来るとなおよい
            Instance.Worker = GameObject.FindObjectsOfType<HTTPRequest>();
        }

        /// <summary>
        /// GET通信をする
        /// </summary>
        /// <param name="uri">通信先のURL</param>
        /// <param name="dlg">データ受信コールバック</param>
        /// <param name="opt">ヘッダなど追加で含む情報</param>
        static public void GetRequest(string uri, GetData dlg, HTTPRequest.Options opt = null)
        {
            CheckInstance();
            var worker = Instance.Worker.Where(r => r.IsActive == false).First();
            worker.Request(RequestMethod.GET, uri, dlg, null, opt);
        }

        /// <summary>
        /// POST通信をする
        /// </summary>
        /// <param name="uri">通信先のURL</param>
        /// <param name="body">サーバに送信する内容</param>
        /// <param name="dlg">データ受信コールバック</param>
        /// <param name="opt">ヘッダなど追加で含む情報</param>
        static public void PostRequest<T>(string uri, T body, GetData dlg, HTTPRequest.Options opt = null)
        {
            CheckInstance();
            var worker = Instance.Worker.Where(r => r.IsActive == false).First();
            string json = JsonUtility.ToJson(body);
            worker.Request(RequestMethod.POST, uri, dlg, Encoding.UTF8.GetBytes(json), opt);
        }

        /// <summary>
        /// POST通信をする
        /// </summary>
        /// <param name="uri">通信先のURL</param>
        /// <param name="body">サーバに送信する内容</param>
        /// <param name="dlg">データ受信コールバック</param>
        /// <param name="opt">ヘッダなど追加で含む情報</param>
        static public void PostRequest<T>(string uri, byte[] body, GetData dlg, HTTPRequest.Options opt = null)
        {
            CheckInstance();
            var worker = Instance.Worker.Where(r => r.IsActive == false).First();
            worker.Request(RequestMethod.POST, uri, dlg, body, opt);
        }
    }
}
