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
      //  dic["stringtoai"] = "Hello";

      string urlWithFormat =
          string.Format("https://mebudy.com/openai/request.php?prompt={0}&apikey=" + APIKEY, promptText);
        UnityWebRequest www = UnityWebRequest.Get(
            urlWithFormat );
        
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
