using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
	public static FPSDisplay Singleton;
	public static float FPS { get { return Singleton ? Singleton.fps : -1; } }
	public static float MSec { get { return Singleton ? Singleton.msec : -1; } }

	float fps;
	float msec;
	float deltaTime = 0.0f;

	void Awake()
	{
		Singleton = this;
	}

	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
	}

	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
		msec = deltaTime * 1000.0f;
		fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		GUI.Label(rect, text, style);
	}
}