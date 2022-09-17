using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Google;

public class SignInManager : MonoBehaviour
{
    private GoogleSignInConfiguration configuration;
    private const string WebClientID = "560143319104-d5bakq1kpie2f25cq1rkfncig5fkajsu.apps.googleusercontent.com";

    [SerializeField] private Button signInButton, signOutButton, playButton;
    [SerializeField] private RetryMenu retryMenu;

    private void Awake()
    {
#if UNITY_ANDROID
        try
        {
            configuration = new GoogleSignInConfiguration
            {
                WebClientId = WebClientID,
                RequestIdToken = true,
                RequestEmail = true,
                UseGameSignIn = false
            };
            
            GoogleSignIn.Configuration = configuration;
        }
        catch (GoogleSignIn.SignInException e)
        {
            Debug.LogError(e);
        }
#endif

        signInButton.onClick.AddListener(SignIn);
        signOutButton.onClick.AddListener(SignOut);
        
        signInButton.gameObject.SetActive(true);
        signOutButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(ValidateCookie());
    }

    private IEnumerator ValidateCookie()
    {
        RaycastBlockEvent.Invoke(true);
        
#if UNITY_ANDROID
        var authCookie = PlayerPrefs.GetString(SessionAuthRequestHandler.AuthKey, string.Empty);

        if (string.IsNullOrEmpty(authCookie))
        {
            signInButton.gameObject.SetActive(true);
            signOutButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            
            RaycastBlockEvent.Invoke(false);
            
            yield break;
        }
#endif
        
        yield return SessionAuthRequestHandler.ValidateSession(OnValidateSuccess, OnValidateFailure);
        
        RaycastBlockEvent.Invoke(false);
    }

    private void OnValidateSuccess()
    {
        OnSignInSuccess();
    }

    private void OnValidateFailure(UnityWebRequest req)
    {
        signInButton.gameObject.SetActive(true);
        signOutButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);

        switch (req.responseCode)
        {
            case 401:
                retryMenu.SessionExpiredInMenu();
                break;
            default:
                retryMenu.InternetConnectionLost(ValidateCookie(), true);
                break;
        }
        
        Debug.Log("Session validation failed");
    }

    private void SignIn()
    {
#if UNITY_ANDROID
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
#elif UNITY_WEBGL
        WebLink.OpenLinkJSPlugin(Endpoints.Login);
#endif
    }
    
    private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            retryMenu.AuthenticationFaulted();
            Debug.LogError("Error: faulted\n" + task.Exception);
        }
        else if (task.IsCanceled)
        {
            retryMenu.AuthenticationFaulted();
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
        RaycastBlockEvent.Invoke(true);
        
        yield return SessionAuthRequestHandler.Login(
            new SessionData(idToken),
            OnSignInSuccess,
            req => OnSignInFailure(req, idToken)
        );
        
        RaycastBlockEvent.Invoke(false);
        
        GoogleSignIn.DefaultInstance.SignOut();
    }

    private void OnSignInSuccess()
    {
        signInButton.gameObject.SetActive(false);
        signOutButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
    }

    private void OnSignInFailure(UnityWebRequest req, string idToken)
    {
        retryMenu.InternetConnectionLost(LoginEnumerator(idToken), true);
        Debug.LogError($"Error: {req.result}");
    }

    private void SignOut()
    {
#if UNITY_ANDROID
        GoogleSignIn.DefaultInstance.SignOut();
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
        signInButton.gameObject.SetActive(true);
        signOutButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
    }

    private void OnSignOutFailure(UnityWebRequest req)
    {
        retryMenu.InternetConnectionLost(SignOutEnumerator(), true);
        Debug.LogError($"Error: {req.result}");
    }
}