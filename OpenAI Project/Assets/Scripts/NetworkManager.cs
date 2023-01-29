using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent<string> onCharacterPromptSent;
    [HideInInspector] public UnityEvent<string> onCharacterResponseRecieved;

    [SerializeField] private bool isEcho = false;

    private const string APIKEY = "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";

    public void SendPromptToServer(string promptToSend)
    {
        StartCoroutine(SendTextToServer(promptToSend));
    }

    [ContextMenu(("Send Test"))]
    void SendTest()
    {
        StartCoroutine(SendTextToServer("hello"));
    }

    IEnumerator SendTextToServer(string promptText)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic["prompt"] = "hello";
        dic["apikey"] = APIKEY;

        string urlWithFormat =
            string.Format("https://mebudy.com/openai/request.php?prompt={0}" +
                          "&apikey={1}&model=text-davinci-003&temp=0&max_tokens=300&echo={2}", promptText, APIKEY,
                isEcho);
        UnityWebRequest www = UnityWebRequest.Get(
            urlWithFormat);
        // WWWForm form = new WWWForm();
        // form.AddField("prompt", "hello");
        // form.AddField("apikey", APIKEY);
        // UnityWebRequest www = UnityWebRequest.Post(@"https://mebudy.com/openai/request.php",form);

        onCharacterPromptSent.Invoke(promptText);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            //    Debug.Log(www.GetResponseHeaders(""));
            Debug.Log(www.error);
        }
        else
        {
            // Dictionary<string, string> responseHeaders = www.GetResponseHeaders();
            Debug.Log("Form upload complete!");
            //    Debug.Log(www.downloadHandler.text);

            int firstIndex = www.downloadHandler.text.IndexOf("\"id\"", StringComparison.Ordinal) - 1;
            int secondIndex = www.downloadHandler.text.LastIndexOf('}');
            string jsonString = www.downloadHandler.text.Substring(firstIndex, secondIndex + 1 - firstIndex);
            JObject stuff = JsonConvert.DeserializeObject<JObject>(jsonString);
            JArray secondStuff = JsonConvert.DeserializeObject<JArray>(stuff["choices"].ToString());
            JObject firstTextObject = JsonConvert.DeserializeObject<JObject>(secondStuff[0].ToString());
            // Debug.Log(firstTextObject["text"]);
            onCharacterResponseRecieved.Invoke((firstTextObject["text"].ToString().Trim()));
        }
    }
}