using UnityEngine;
using System.Runtime.InteropServices;

public static class WebLink
{
	public static void OpenLinkJSPlugin(string url)
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
