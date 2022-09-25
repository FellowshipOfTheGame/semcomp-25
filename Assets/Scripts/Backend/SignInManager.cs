using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Google;
using TMPro;
using UnityEngine.Serialization;

public class SignInManager : MonoBehaviour
{
    [SerializeField] private Button signInButton, signOutButton, playButton, rankingButton;
    [SerializeField] private RetryMenu retryMenu;
    [SerializeField] private TMP_InputField inputCode;
    // [SerializeField] private Button inputCodeConfirmButton;
    [SerializeField] private GameObject loginPanel;

    private void Awake()
    {        
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
        var authCookie = PlayerPrefs.GetString(SessionAuthRequestHandler.AuthKey, string.Empty);

        if (string.IsNullOrEmpty(authCookie))
        {
            signInButton.gameObject.SetActive(true);
            signOutButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            rankingButton.interactable = false;

            yield break;
        }
        
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
        
        // Debug.Log("Session validation failed");
    }

    public void SignIn()
    {
        WebLink.OpenURL(Endpoints.Login);
    }

    public void InputCode()
    {
        if (string.IsNullOrEmpty(inputCode.text)) return;
        loginPanel.SetActive(false);
        StartCoroutine(SignInEnumerator());
    }

    private IEnumerator SignInEnumerator()
    {
        yield return SessionAuthRequestHandler.Login(
            new SessionData(inputCode.text.Trim()),
            OnSignInSuccess,
            OnSignInFailure);
    }

    private void OnSignInSuccess()
    {
        retryMenu.Close();
        signInButton.gameObject.SetActive(false);
        signOutButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        rankingButton.interactable = true;
    }

    private void OnSignInFailure(UnityWebRequest req)
    {
        switch (req.responseCode)
        {
            case 400:
                retryMenu.InvalidLoginCode();
                break;
            default:
                retryMenu.InternetConnectionLost(SignInEnumerator(), true);
                break;
        }
    }

    public void SignOut()
    {
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
        // Debug.LogError($"Error: {req.result}");
    }
}