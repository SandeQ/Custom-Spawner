using System;
using System.Collections.Generic;

using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;
using MySqlConnector;
//using HarmonyLib;

namespace CustomSpawner
{
	public class CustomSpawner : Plugin<Config>
	{
		public EventHandler Handler;

		public static CustomSpawner Singleton;

		public override string Name { get; } = "CustomSpawner";
		public override string Author { get; } = "Kognity";
		public override string Prefix { get; } = "CustomSpawner";
		public override Version RequiredExiledVersion { get; } = new Version(2, 8, 0);
		public override Version Version { get; } = new Version(1, 4, 0);
		internal static readonly object DbLock = new object();
		public static MySqlConnectionStringBuilder connstr = new MySqlConnectionStringBuilder();
		public static DatabaseConnInfo mysqlconninfo = new DatabaseConnInfo();
		public static MySqlConnection conn;
		public override void OnEnabled()
		{
			Singleton = this;
			Handler = new EventHandler(this);

			Exiled.Events.Handlers.Player.Verified += Handler.OnVerified;
			Exiled.Events.Handlers.Server.RoundStarted += Handler.OnRoundStart;
			Exiled.Events.Handlers.Server.WaitingForPlayers += Handler.OnWaitingForPlayers;
			Exiled.Events.Handlers.Player.PickingUpItem += Handler.OnPickingUp;
			try
			{
				connstr.Server = Config.DbServer;
				connstr.Database = Config.DbName;
				connstr.UserID = Config.DbUserId;
				connstr.Password = Config.DbPass;
				connstr.Port = Config.DbPort;
				connstr.TlsVersion = "TLSv1.2";
				connstr.CharacterSet = "utf8_mb4";
				Log.Debug("1");
				conn = new MySqlConnection(connstr.ToString());
			}
			catch (Exception e)
			{
				//This try catch is redundant, as EXILED will throw an error before this block can, but is here as an example of how to handle exceptions/errors
				Log.Error($"There was an error loading the plugin: {e}");
			}
			base.OnEnabled();



		}
	

		public override void OnDisabled()
		{
			Exiled.Events.Handlers.Player.Verified -= Handler.OnVerified;
			Exiled.Events.Handlers.Server.RoundStarted -= Handler.OnRoundStart;
			Exiled.Events.Handlers.Server.WaitingForPlayers -= Handler.OnWaitingForPlayers;
			Exiled.Events.Handlers.Player.PickingUpItem -= Handler.OnPickingUp;

			Handler = null;
			base.OnDisabled();
		}
	}
}
