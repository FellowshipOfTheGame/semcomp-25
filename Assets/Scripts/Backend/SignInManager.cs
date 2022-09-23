using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Google;
using TMPro;

public class SignInManager : MonoBehaviour
{
    // private GoogleSignInConfiguration configuration;
    // private const string WebClientID = "560143319104-d5bakq1kpie2f25cq1rkfncig5fkajsu.apps.googleusercontent.com";

    [SerializeField] private Button signInButton, signOutButton, playButton, rankingButton;
    [SerializeField] private RetryMenu retryMenu;
    [SerializeField] private TMP_InputField inputCode;

    private void Awake()
    {
#if UNITY_ANDROID
        // Google sign-in plugin
        // try
        // {
        //     configuration = new GoogleSignInConfiguration
        //     {
        //         WebClientId = WebClientID,
        //         RequestIdToken = true,
        //         RequestEmail = true,
        //         UseGameSignIn = false
        //     };
        //     
        //     GoogleSignIn.Configuration = configuration;
        // }
        // catch (GoogleSignIn.SignInException e)
        // {
        //     Debug.LogError(e);
        // }
#endif
        
        signInButton.gameObject.SetActive(true);
        signOutButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        rankingButton.interactable = false;
    }

    private void Start()
    {
        StartCoroutine(ValidateCookie());
    }

    private IEnumerator ValidateCookie()
    {
#if UNITY_ANDROID
        var authCookie = PlayerPrefs.GetString(SessionAuthRequestHandler.AuthKey, string.Empty);

        if (string.IsNullOrEmpty(authCookie))
        {
            signInButton.gameObject.SetActive(true);
            signOutButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            rankingButton.interactable = false;

            yield break;
        }
#endif
        
        yield return SessionAuthRequestHandler.ValidateSession(OnValidateSuccess, OnValidateFailure);
    }

    private void OnValidateSuccess()
    {
        OnSignInSuccess();
    }

    private void OnValidateFailure(UnityWebRequest req)
    {
        switch (req.responseCode)
        {
            case 401:
                retryMenu.SessionExpiredInMenu();
                signInButton.gameObject.SetActive(true);
                signOutButton.gameObject.SetActive(false);
                playButton.gameObject.SetActive(false);
                rankingButton.interactable = false;
                break;
            default:
                retryMenu.InternetConnectionLost(ValidateCookie(), true);
                break;
        }
        
        Debug.Log("Session validation failed");
    }

    public void SignIn()
    {
        WebLink.OpenURL(Endpoints.Login);
#if UNITY_ANDROID
        // GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
#endif
    }

    public void InputCode()
    {
        if (string.IsNullOrEmpty(inputCode.text)) return;
        StartCoroutine(GetSessionEnumerator());
    }

    private IEnumerator GetSessionEnumerator()
    {
        yield return SessionAuthRequestHandler.GetSession(
            new SessionByCodeData(inputCode.text.Trim()),
            OnGetSessionSuccess,
            OnGetSessionFailure);
    }

    private void OnGetSessionSuccess()
    {
        OnSignInSuccess();
    }

    private void OnGetSessionFailure(UnityWebRequest req)
    {
        switch (req.responseCode)
        {
            case 400:
                retryMenu.InvalidLoginCode();
                break;
            default:
                retryMenu.InternetConnectionLost(GetSessionEnumerator(), true);
                break;
        }
    }
    
    // Google sign-in plugin
    private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            retryMenu.AuthenticationFaulted();
            Debug.LogError("Error: faulted\n" + task.Exception);
        }
        else if (task.IsCanceled)
        {
            Debug.LogError("Error: canceled\n" + task.Exception);
        }
        else
        {
            StartCoroutine(LoginEnumerator(task.Result.IdToken));
            Debug.Log($"Token: {task.Result.IdToken}");
        }
    }

    private IEnumerator LoginEnumerator(string idToken)
    {
        yield return SessionAuthRequestHandler.Login(
            new SessionData(idToken),
            OnSignInSuccess,
            req => OnSignInFailure(req, idToken)
        );

        GoogleSignIn.DefaultInstance.SignOut();
    }

    private void OnSignInSuccess()
    {
        retryMenu.Close();
        signInButton.gameObject.SetActive(false);
        signOutButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        rankingButton.interactable = true;
    }

    private void OnSignInFailure(UnityWebRequest req, string idToken)
    {
        retryMenu.InternetConnectionLost(LoginEnumerator(idToken), true);
        Debug.LogError($"Error: {req.result}");
    }

    public void SignOut()
    {
#if UNITY_ANDROID
        // GoogleSignIn.DefaultInstance.SignOut();
#endif
        StartCoroutine(SignOutEnumerator());
    }

    private IEnumerator SignOutEnumerator()
    {
        yield return SessionAuthRequestHandler.Logout(
            OnSignOutSuccess,
            OnSignOutFailure
        );
    }

    private void OnSignOutSuccess()
    {
        retryMenu.Close();
        signInButton.gameObject.SetActive(true);
        signOutButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        rankingButton.interactable = false;
    }

    private void OnSignOutFailure(UnityWebRequest req)
    {
        retryMenu.InternetConnectionLost(SignOutEnumerator(), true);
        Debug.LogError($"Error: {req.result}");
    }
}