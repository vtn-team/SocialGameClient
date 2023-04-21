using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Network
{
    public class WebRequest
    {
        public delegate void GetData(byte[] result);
        public enum RequestMethod
        {
            GET,
            POST
        }

        static WebRequest Instance= new WebRequest();

        HTTPRequest[] Worker = null;
        
        static public void GetRequest(string uri, GetData dlg, HTTPRequest.Options opt = null)
        {
            if (Instance.Worker == null)
            {
                Instance.Worker = GameObject.FindObjectsOfType<HTTPRequest>();
                Debug.Log(Instance.Worker.Length);
                if (Instance.Worker == null) return;
            }

            var worker = Instance.Worker.Where(r => r.IsActive == false).First();
            worker.Request(RequestMethod.GET, uri, dlg, null, opt);
        }

        static public void PostRequest<T>(string uri, T body, GetData dlg, HTTPRequest.Options opt = null)
        {
            if (Instance.Worker == null)
            {
                Instance.Worker = GameObject.FindObjectsOfType<HTTPRequest>();
                if (Instance.Worker == null) return;
            }

            var worker = Instance.Worker.Where(r => r.IsActive == false).First();
            string json = JsonUtility.ToJson(body);
            worker.Request(RequestMethod.POST, uri, dlg, Encoding.UTF8.GetBytes(json), opt);
        }

        static public void PostRequest<T>(string uri, byte[] body, GetData dlg, HTTPRequest.Options opt = null)
        {
            if (Instance.Worker == null)
            {
                Instance.Worker = GameObject.FindObjectsOfType<HTTPRequest>();
                if (Instance.Worker == null) return;
            }

            var worker = Instance.Worker.Where(r => r.IsActive == false).First();
            worker.Request(RequestMethod.POST, uri, dlg, body, opt);
        }
    }
}
