using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Google;

public class AuthManager : MonoBehaviour
{
    private GoogleSignInConfiguration configuration;
    private const string WebClientID = "560143319104-d5bakq1kpie2f25cq1rkfncig5fkajsu.apps.googleusercontent.com";
    private const string ServerURL = "http://192.168.0.122:3000";

    [SerializeField] private GameObject signInButton, signOutButton;
    [SerializeField] private TextMeshProUGUI idTokenText, sessionText, responseText;

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = WebClientID,
            RequestIdToken = true,
            UseGameSignIn = false
        };

        signInButton.SetActive(string.IsNullOrEmpty(PlayerPrefs.GetString("gameSession")));
        signOutButton.SetActive(!string.IsNullOrEmpty(PlayerPrefs.GetString("gameSession")));
    }

    public void SignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }
    
    private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Error: faulted\n" + task.Exception);
        }
        else if (task.IsCanceled)
        {
            Debug.LogError("Error: canceled\n" + task.Exception);
        }
        else
        {
            StartCoroutine(SendLogin("id_token", task.Result.IdToken));
            idTokenText.text = task.Result.IdToken;
        }
    }
    
    private IEnumerator SendLogin(string key, string value)
    {
        var req = UnityWebRequest.Post($"{ServerURL}/session/login", "");
        req.SetRequestHeader(key, value);
        yield return req.SendWebRequest();
        responseText.text = req.responseCode.ToString();

        if (req.result == UnityWebRequest.Result.Success)
        {
            PlayerPrefs.SetString("gameSession", req.GetResponseHeader("gameSession"));
            sessionText.text = PlayerPrefs.GetString("gameSession");
            signInButton.SetActive(false);
        }
    }

    public void SignOut()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("gameSession"))) return;
        GoogleSignIn.DefaultInstance.SignOut();
        StartCoroutine(SendSignOut());
    }
    
    private IEnumerator SendSignOut() {
        var req = UnityWebRequest.Get($"{ServerURL}/session/logout");
        req.SetRequestHeader("gameSession", PlayerPrefs.GetString("gameSession"));
        yield return req.SendWebRequest();
        responseText.text = req.responseCode.ToString();

        if (req.result == UnityWebRequest.Result.Success)
        {
            PlayerPrefs.DeleteKey("gameSession");
            sessionText.text = "";
            signInButton.SetActive(true);
        }
    }
}