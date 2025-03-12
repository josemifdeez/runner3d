using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class DataManager : MonoBehaviour
{
    string urlGetInfo = "http://localhost/runner/getleadboard.php";
    string urlSetInfo = "http://localhost/runner/setscore.php";
   

    public UserInfo[] users;
    public Transform parentUsers;
    public GameObject userPrefab;

    public TMPro.TMP_InputField usernameField;
    int score;
    public Text scoreText;
    public GameObject scorePanel;
    
    public void GameOver(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
        scorePanel.SetActive(true);
    }
    public void GetInfoLeadBoard()
    {
        StartCoroutine(cGetData());
    }
    public void PrintLeadBoard()
    {
        for (int i = 0; i < users.Length; i++)
        {
            GameObject temp = Instantiate(userPrefab, parentUsers);
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = users[i].username;
            temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = users[i].score.ToString();
        }
    }

    IEnumerator cGetData()
    {
        UnityWebRequest request = UnityWebRequest.Get(urlGetInfo);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            //Debug.Log(request.downloadHandler.text);
            string jsonData = request.downloadHandler.text;
            RootUsers info = JsonUtility.FromJson<RootUsers>("{\"users\":"+jsonData+"}");
            users = info.users;
            PrintLeadBoard();
        }
    }
    public void ShowScorePanel()
    {
        scorePanel.transform.Find("ScoreValue").GetComponent<TextMeshProUGUI>().text = score.ToString();
        scorePanel.SetActive(true);
    }
    public void SetScoreUser()
    {
        if (usernameField.text.Length > 3)
        {
            StartCoroutine(cSetData());
            scorePanel.SetActive(false);

        }
        
    }
    IEnumerator cSetData()
    {
        WWWForm form = new WWWForm();
        form.AddField("user", usernameField.text);
        form.AddField("score", score);

        UnityWebRequest request = UnityWebRequest.Post(urlSetInfo, form);
        yield return request.SendWebRequest();
        ChangeScene(0);
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
        
    }
    public void ChangeScene(int sceneIndex)//menu 0 game 1
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
   
}

[System.Serializable]
public class UserInfo
{
    //El nombre de las variables == nombre columnas de la base de datos

    public int id;
    public string username;
    public int score;
}
[System.Serializable]
public class RootUsers
{
    public UserInfo[] users;
}
