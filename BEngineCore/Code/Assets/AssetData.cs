﻿using BEngine;
using BEngineCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BEngineCore
{
	public class AssetData
	{
		private string _guid;
		private ProjectAbstraction _project;

		[JsonIgnore]
		public string GUID => _guid;
		[JsonIgnore]
		public ProjectAbstraction Project => _project;

		protected AssetData()
		{

		}

		public AssetData(string guid, ProjectAbstraction project) 
		{
			_guid = guid;
			_project = project;
		}

		public void SetForceID(string guid) => _guid = guid;
		public void SetForceProject(ProjectAbstraction project) => _project = project;

		public static void CreateTemplate<T>(string path, object[]? args = null) where T : AssetData
		{
			T? template = (T?)Activator.CreateInstance(typeof(T), args);
			if (template != null)
			{
				File.WriteAllText(path, JsonUtils.Serialize(template));
			}
		}

		public static T? ReadRaw<T>(string path) where T : AssetData
		{
			T? assetData = JsonUtils.Deserialize<T>(File.ReadAllText(path));
			return assetData;
		}
		
		public static void WriteRaw<T>(string path, T data) where T : AssetData
		{
			File.WriteAllText(path, JsonUtils.Serialize(data));
		}

		public void Save<T>() where T : AssetData
		{
			string path = _project.AssetsReader.GetAssetPath(_guid);

			if (path == string.Empty)
				return;

			try
			{
				OnPreSave();
				File.WriteAllText(path, JsonUtils.Serialize((T)this));
			}
			catch
			{

			}
		}

		public void SaveGuaranteed<T>(string unknownPath) where T : AssetData
		{
			string path = _project.AssetsReader.GetAssetPath(_guid);

			if (path == string.Empty)
				path = unknownPath;

			try
			{
				OnPreSave();
				File.WriteAllText(path, JsonUtils.Serialize((T)this));
			}
			catch
			{

			}
		}

		protected virtual void OnPreSave()
		{

		}

		public static T? Load<T>(string guid, ProjectAbstraction project) where T : AssetData
		{
			string path = project.AssetsReader.GetAssetPath(guid);

			if (path == string.Empty)
				return null;

			T? assetData = JsonUtils.Deserialize<T>(File.ReadAllText(path));
			if (assetData != null)
			{
				assetData.SetForceID(guid);
				assetData.SetForceProject(project);
				return assetData;
			}

			return null;
		}
	}
}
