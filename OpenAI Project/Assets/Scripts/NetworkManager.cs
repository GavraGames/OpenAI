using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{

    [ContextMenu(("Send Test"))]
    void SendTest()
    {
        StartCoroutine(SendTextToServer());
    }

    IEnumerator SendTextToServer()
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic["stringtoai"] = "Hello";
        UnityWebRequest www = UnityWebRequest.Post(@"http://www.mebudy.com:3000/", dic);
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
            Debug.Log(www.downloadHandler.text); 
        }
    }
}
