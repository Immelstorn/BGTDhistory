using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using ChatStatistics.Parser.Entites;
using Newtonsoft.Json;

namespace ChatStatistics.Parser
{
    public class Vk
    {
        #region vars

                private const string AccessToken = "";
//        private const string AccessToken = "";
                private const int ChatId = 6;
//        private const int ChatId = 3;

        private string responseUri;
        private string stream;
        private readonly NameValueCollection qs = new NameValueCollection();
        private readonly CookieContainer cookieContainer = new CookieContainer();
        private readonly List<Message> parsedMessages = new List<Message>();
        private int offset = 0;
        private bool stop = false;
        private int prevcount = 0;
        #endregion

        public List<Message> ParseMessages()
        {
            while (!stop)
            {
                prevcount = parsedMessages.Count;
                qs["offset"] = offset.ToString();
                qs["count"] = "200";
                qs["chat_id"] = ChatId.ToString();
                qs["rev"] = "1";
                offset += 200;

                string result = ExecuteCommandJson("messages.getHistory", qs);
                Response<List<object>> rsp = JsonConvert.DeserializeObject<Response<List<object>>>(result);
                rsp.response.RemoveAt(0);
                var linqrsp = rsp.response.Select(r => JsonConvert.DeserializeObject<Message>(r.ToString())).ToList();
                parsedMessages.AddRange(linqrsp);
                Console.WriteLine(parsedMessages.Count);
                Thread.Sleep(1000);
                stop = parsedMessages.Count == prevcount;
            }
            return parsedMessages;
        }

       private string ExecuteCommandJson(string name, NameValueCollection qs)
        {
            string url = String.Format("https://api.vk.com/method/{0}?access_token={1}&{2}", name, AccessToken,
                String.Join("&", qs.AllKeys.Select(item => item + "=" + qs[item])));
            GetRequestAndResponse(url, out responseUri, out stream);
            string res = stream;
            qs.Clear();
            return res;
        }

        private void GetRequestAndResponse(string uri, out string responseUri, out string stream,
            string method = "GET")
        {
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            if (request != null)
            {
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0";
                request.CookieContainer = cookieContainer;
                request.CookieContainer.Add(new Uri(uri), new Cookie("remixsid", ""));
                request.CookieContainer.Add(new Uri(uri), new Cookie("remixlang", "0"));
                request.CookieContainer.Add(new Uri(uri), new Cookie("remixflash", "0.0.0"));
                request.CookieContainer.Add(new Uri(uri), new Cookie("remixdt", "-3600"));

                request.Method = method;
            }
            stream = "";
            responseUri = "";
            if (request != null)
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null)
                    {
                        using (Stream receiveStream = response.GetResponseStream())
                        {
                            if (receiveStream != null)
                            {
                                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                                stream = readStream.ReadToEnd();
                            }
                        }
                        responseUri = response.ResponseUri.ToString();
                        cookieContainer.Add(response.Cookies);
                    }
                }
            }
        }
    }
}