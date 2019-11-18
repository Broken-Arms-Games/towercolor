using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimerStrings
{
	static readonly string[] timerToStrings = { "D", "H", "M", "S" };

	public enum TimerFormat
	{
		Default,
		Extended,
		CompactMS,
		TotalHM
	}

	public static string GetTimerSeconds(float seconds, bool ms = true, string msSeparator = ".", string sFormat = "00", string msFormat = "[DEFAULT]", int msLength = 3)
	{
		if(ms)
		{
			if(msFormat == "[DEFAULT]")
			{
				msFormat = "";
				for(int i = 0; i < msLength; i++)
					msFormat += "0";
			}
			return string.Format("{0:" + sFormat + "}" + msSeparator + "{1:" + msFormat + "}",
				Mathf.FloorToInt(seconds),
				Mathf.FloorToInt(seconds * Mathf.Pow(10f, msLength) % Mathf.Pow(10f, msLength)));
		}
		else
			return string.Format("{0:" + sFormat + "}",
				Mathf.FloorToInt(seconds));
		//return Mathf.Floor(seconds / 60f) + ":" + (seconds - Mathf.Floor(seconds / 60f) * 60f).ToString("00.0000");
	}

	public static string GetTimerMinutes(float seconds, bool ms = false, string sSeparator = ":", string msSeparator = ".", string sFormat = "00", string msFormat = "[DEFAULT]", int msLength = 3)
	{
		if(ms)
		{
			if(msFormat == "[DEFAULT]")
			{
				msFormat = "";
				for(int i = 0; i < msLength; i++)
					msFormat += "0";
			}
			return string.Format("{0}" + sSeparator + "{1:" + sFormat + "}" + msSeparator + "{2:" + msFormat + "}",
				Mathf.FloorToInt(seconds / 60f),
				Mathf.FloorToInt(seconds % 60f),
				Mathf.FloorToInt(seconds * Mathf.Pow(10f, msLength) % Mathf.Pow(10f, msLength)));
		}
		else
			return string.Format("{0}:{1:00}",
				Mathf.FloorToInt(seconds / 60f),
				Mathf.FloorToInt(seconds % 60f));
	}


	public static string GetTimerUTCTo(DateTime t, bool withSign = true, TimerFormat format = TimerFormat.Default, string[] labels = null)
	{
		return GetTimerTo(t - DateTime.UtcNow, withSign, format, labels);
	}

	public static string GetTimerTo(DateTime t, bool withSign = true, TimerFormat format = TimerFormat.Default, string[] labels = null)
	{
		return GetTimerTo(t - DateTime.Now, withSign, format, labels);
	}

	public static string GetTimerTo(float countDown, bool withSign = true, TimerFormat format = TimerFormat.Default, string[] labels = null)
	{
		return GetTimerTo(new TimeSpan(0, 0, 0, (int)countDown), withSign, format, labels);
	}

	public static string GetTimerTo(TimeSpan timeSpan, bool withSign = true, TimerFormat format = TimerFormat.Default, string[] labels = null)
	{
		if(labels == null || labels.Length != 4)
			labels = timerToStrings;

		string timerDisplay = withSign && Mathf.Sign((int)timeSpan.TotalSeconds) >= 0 ? "- " : "";
		bool writeD = Mathf.FloorToInt((float)timeSpan.TotalDays) != 0;
		bool writeH = Mathf.FloorToInt((float)timeSpan.TotalHours) != 0;
		bool writeM = Mathf.FloorToInt((float)timeSpan.TotalMinutes) != 0;
		bool writeS = Mathf.FloorToInt((float)timeSpan.TotalSeconds) != 0;
		switch(format)
		{
			case TimerFormat.Default:
				if(writeD)
					timerDisplay += Mathf.Abs(timeSpan.Days).ToString() + labels[0] + " : " + Mathf.Abs(timeSpan.Hours).ToString() + labels[1];
				else if(writeH)
					timerDisplay += Mathf.Abs(timeSpan.Hours).ToString() + labels[1] + " : " + Mathf.Abs(timeSpan.Minutes).ToString("00") + labels[2];
				else if(writeM)
					timerDisplay += Mathf.Abs(timeSpan.Minutes).ToString() + labels[2] + " : " + Mathf.Abs(timeSpan.Seconds).ToString("00") + labels[3];
				else
					timerDisplay += Mathf.Abs(timeSpan.Seconds).ToString() + labels[3];
				break;
			case TimerFormat.Extended:
				timerDisplay +=
					(writeD ? (Mathf.Abs(timeSpan.Days).ToString() + labels[0] + " : ") : "") +
					(writeH ? (Mathf.Abs(timeSpan.Hours).ToString() + labels[1] + " : ") : "") +
					(writeM ? (Mathf.Abs(timeSpan.Minutes).ToString("00") + labels[2] + " : ") : "") +
					(!writeD ? (Mathf.Abs(timeSpan.Seconds).ToString("00") + labels[3]) : "");
				break;
			case TimerFormat.CompactMS:
				timerDisplay += Mathf.Abs(timeSpan.Minutes).ToString("00") + ":" + Mathf.Abs(timeSpan.Seconds).ToString("00");
				break;
			case TimerFormat.TotalHM:
				timerDisplay += Mathf.Abs((int)timeSpan.TotalHours).ToString("00") + labels[1].ToLower() + Mathf.Abs(timeSpan.Minutes).ToString("00") + labels[2].ToLower();
				break;
		}
		return timerDisplay;
	}
}
