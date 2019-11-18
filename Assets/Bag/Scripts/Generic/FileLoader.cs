using System.IO;
using UnityEngine;

namespace Bag.Scripts.Generic
{
	public static class FileLoader
	{
		static readonly string resources = Application.streamingAssetsPath + "/";

		const string jsonPath = "Database/";
		const string savePath = "Saves/";
		const string jsonExt = ".json";


		public static string LoadJson(string fileName)
		{
			return LoadFile(jsonPath + fileName, jsonExt);
		}

		public static void SaveJson(string fileName, string file)
		{
			WriteFile(jsonPath + fileName, file, jsonExt);
		}

		public static bool JsonExists(string fileName)
		{
			return File.Exists(resources + jsonPath + fileName + jsonExt);
		}

		public static bool SaveExists(string fileName)
		{
			return File.Exists(resources + savePath + fileName + jsonExt);
		}

		public static string LoadSave(string fileName)
		{
			return LoadFile(savePath + fileName, jsonExt);
		}

		public static void SaveSave(string fileName, string file)
		{
			WriteFile(savePath + fileName, file, jsonExt);
		}

		public static string LoadFile(string fileName, string extension = ".txt")
		{
			string path = resources + fileName + extension;
			if(File.Exists(path))
			{
				StreamReader reader = new StreamReader(path);
				string file = reader.ReadToEnd();
				reader.Close();
				Debug.Log("[FILE LOADER] Loaded file at '" + path + "':\n" + file);
				return file;
			}
			else
			{
				Debug.LogError("[FILE LOADER] ERROR: file not found at path '" + path + "', returning empty string.");
				return "";
			}
		}

		public static void WriteFile(string fileName, string file, string extension = ".txt")
		{
			string path = resources + fileName + extension;
			if(!File.Exists(path))
				File.Create(path);
			StreamWriter writer = new StreamWriter(path, false);
			writer.Write(file);
			writer.Close();
		}
	}
}
