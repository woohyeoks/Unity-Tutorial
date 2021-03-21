using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


[System.Serializable]
public class GoogleData
{
    public string order, result, msg, value;

}

public class GoogleSheetManager : MonoBehaviour
{
    // 구글 스프라이트 url에서 edit 이전 가져온다
    // 가져올 범위 정하기 &range=A2:B
    // const string URL = "https://docs.google.com/spreadsheets/d/1rAwkICocl8blZJTw9S0Dd8R0ZiRaIv6kjPycbVa4CjE/export?format=tsv";
    const string URL = "https://script.google.com/macros/s/AKfycbx-zeQhPmWps0oR8DMn6wPJAqMlI1IXFC9nvBGMVvuE2uEPJLVX/exec";
    public GoogleData GD;
    public InputField IDInput, PassInput, ValueInput;
    string id, pass;


    /*IEnumerator Start()
    {
        // UnityWebRequest www = UnityWebRequest.Get(URL);
        //UnityWebRequest www = UnityWebRequest.Post(URL, "");


        WWWForm form = new WWWForm();
        form.AddField("value", "값");

        UnityWebRequest www = UnityWebRequest.Post(URL, form);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
    }*/

    bool SetIDPass()
    {
        id = IDInput.text.Trim(); // 공백 제거
        pass = PassInput.text.Trim();

        if (id == "" || pass == "") return false;
        return true;
    }

    public void Login()
    {
        if (!SetIDPass())
        {
            print("아이디 또는 비밀번호가 비어있습니다");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", id);
        form.AddField("pass", pass);

        StartCoroutine(Post(form));
    }

    private void OnApplicationQuit()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "logout");
        StartCoroutine(Post(form));
    }

    public void Register()
    {
        if (!SetIDPass())
        {
            print("아이디 또는 비밀번호가 비어있습니다.");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", id);
        form.AddField("pass", pass);
       
        StartCoroutine(Post(form));
    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) // 반드시 using 써야한다.
        {
            yield return www.SendWebRequest(); // 보내는 시간동안 대기

            if (www.isDone)
                Response(www.downloadHandler.text);
            else
                print("웹의 응답이 없습니다.");
        }
    }

    void Response(string json)
    {
        if (string.IsNullOrEmpty(json))
            return;
        GD = JsonUtility.FromJson<GoogleData>(json);

        if (GD.result == "ERROR") ;
        {
            print(GD.order + "를 싱핼 할 수 없습니다. 에러 메시지 : " + GD.msg);
        }

        print(GD.order + "을 실행했습니다. 메시지 :" + GD.msg);

        if (GD.order == "getValue")
        {
            ValueInput.text = GD.value;
        }
    }


    public void SetValue()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "setValue");
        form.AddField("value", ValueInput.text);
        StartCoroutine(Post(form));
    }


    public void GetValue()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "getValue");
        StartCoroutine(Post(form));
    }
}



