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
    // private const string ServerURL = "http://192.168.0.122:3000";

    [SerializeField] private GameObject signInButton, signOutButton;
    [SerializeField] private TextMeshProUGUI idTokenText, sessionText, responseText;
    [SerializeField] private TMP_InputField serverURLInputField;

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = WebClientID,
            RequestIdToken = true,
            RequestEmail = true,
            UseGameSignIn = false
        };

        if (string.IsNullOrEmpty(PlayerPrefs.GetString("gameSession")))
        {
            signInButton.SetActive(true);
            signOutButton.SetActive(false);
        }
        else
        {
            sessionText.text = PlayerPrefs.GetString("gameSession");
            signInButton.SetActive(false);
            signOutButton.SetActive(true);
        }
    }

    public void SignIn()
    {
        if (string.IsNullOrEmpty(serverURLInputField.text)) return;
        
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
        var req = UnityWebRequest.Post($"{serverURLInputField.text}/session/login", "");
        req.SetRequestHeader(key, value);
        yield return req.SendWebRequest();
        responseText.text = req.responseCode.ToString();

        if (req.result == UnityWebRequest.Result.Success)
        {
            foreach (var h in req.GetResponseHeaders())
            {
                Debug.Log($"{h.Key}: {h.Value}");
            }
            PlayerPrefs.SetString("gameSession", req.GetResponseHeader("Set-Cookie"));
            sessionText.text = PlayerPrefs.GetString("gameSession");
            signInButton.SetActive(false);
            signOutButton.SetActive(true);
        }
    }

    public void SignOut()
    {
        if (string.IsNullOrEmpty(serverURLInputField.text)) return;
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("gameSession"))) return;
        GoogleSignIn.DefaultInstance.SignOut();
        StartCoroutine(SendSignOut());
    }
    
    private IEnumerator SendSignOut() {
        var req = UnityWebRequest.Get($"{serverURLInputField.text}/session/logout");
        req.SetRequestHeader("Set-Cookie", PlayerPrefs.GetString("gameSession"));
        yield return req.SendWebRequest();
        responseText.text = req.responseCode.ToString();

        if (req.result == UnityWebRequest.Result.Success)
        {
            PlayerPrefs.DeleteKey("gameSession");
            sessionText.text = "";
            signOutButton.SetActive(false);
            signInButton.SetActive(true);
        }
    }
}