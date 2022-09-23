using System.Runtime.InteropServices;
using UnityEngine;

public class WebLink : MonoBehaviour
{
	public static void OpenURL(string url)
	{
#if UNITY_WEBGL && !UNITY_EDITOR
		openWindow(url);
#else
		Application.OpenURL(url);
#endif
	}

#if UNITY_WEBGL && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void openWindow(string url);
#endif
}
