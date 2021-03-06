﻿using System;
using System.IO;
using System.Reflection;
using PluginInterface;
using otitemeditor;

namespace Host
{
	/// <summary>
	/// Summary description for PluginServices.
	/// </summary>
	public class PluginServices : IPluginHost
	{
		/// <summary>
		/// Constructor of PluginServices
		/// </summary>
		public PluginServices()
		{
		}

		private Types.PluginCollection colAvailablePlugins = new Types.PluginCollection();

		/// <summary>
		/// A collection of all plugins found by FindPlugins()
		/// </summary>
		public Types.PluginCollection AvailablePlugins
		{
			get { return colAvailablePlugins; }
			set { colAvailablePlugins = value; }
		}

		/// <summary>
		/// Searches the Application's startup directory
		/// </summary>
		public void FindPlugins()
		{
			FindPlugins(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins"));
		}

		/// <summary>
		/// Searches the Path for plugins
		/// </summary>
		/// <param name="Path">Directory to search for Plugins in</param>
		public void FindPlugins(string Path)
		{
			colAvailablePlugins.Clear();

			foreach (string fileOn in Directory.GetFiles(Path))
			{
				FileInfo file = new FileInfo(fileOn);
				if (file.Extension.Equals(".dll"))
				{
					this.AddPlugin(fileOn);
				}
			}
		}

		/// <summary>
		/// Unloads all plugins
		/// </summary>
		public void ClosePlugins()
		{
			foreach (Types.Plugin pluginOn in colAvailablePlugins)
			{
				pluginOn.Instance.Dispose();
				pluginOn.Instance = null;
			}

			colAvailablePlugins.Clear();
		}

		private void AddPlugin(string FileName)
		{
			Assembly pluginAssembly = Assembly.LoadFrom(FileName);

			foreach (Type pluginType in pluginAssembly.GetTypes())
			{
				if (pluginType.IsPublic)
				{
					if (!pluginType.IsAbstract)
					{
						Type typeInterface = pluginType.GetInterface("PluginInterface.IPlugin", true);
						if (typeInterface != null)
						{
							Types.Plugin newPlugin = new Types.Plugin();
							newPlugin.AssemblyPath = FileName;
							newPlugin.Instance = (IPlugin)Activator.CreateInstance(pluginAssembly.GetType(pluginType.ToString()));
							newPlugin.Instance.Host = this;
							newPlugin.Instance.Initialize();
							this.colAvailablePlugins.Add(newPlugin);

							newPlugin = null;
						}

						typeInterface = null;
					}
				}
			}

			pluginAssembly = null;
		}
	}

	namespace Types
	{
		/// <summary>
		/// Collection of AvailablePlugin Type
		/// </summary>
		public class PluginCollection : System.Collections.CollectionBase
		{
			/// <summary>
			/// Add a plugin
			/// </summary>
			/// <param name="pluginToAdd">The Plugin to Add</param>
			public void Add(Types.Plugin pluginToAdd)
			{
				this.List.Add(pluginToAdd);
			}

			/// <summary>
			/// Removes a plugin
			/// </summary>
			/// <param name="pluginToRemove">The Plugin to Remove</param>
			public void Remove(Types.Plugin pluginToRemove)
			{
				this.List.Remove(pluginToRemove);
			}

			/// <summary>
			/// Search for a plugin by name
			/// </summary>
			/// <param name="pluginNameOrPath">The name or File path of the plugin to find</param>
			/// <returns>a plugin, or null if the plugin is not found</returns>
			public Types.Plugin Find(string pluginNameOrPath)
			{
				foreach (Types.Plugin plugin in this.List)
				{
					foreach (SupportedClient client in plugin.Instance.SupportedClients)
					{
						if ((client.description.Equals(pluginNameOrPath)))
						{
							return plugin;
						}
					}

					if (plugin.AssemblyPath.Equals(pluginNameOrPath))
					{
						return plugin;
					}
				}

				return null;
			}


			/// <summary>
			/// Search for a plugin by compatibility
			/// </summary>
			/// <param name="version">The otb version</param>
			/// <returns>a plugin, or null if the plugin is not found</returns>
			public Types.Plugin Find(UInt32 otbVersion)
			{
				foreach (Types.Plugin plugin in this.List)
				{
					foreach(SupportedClient client in plugin.Instance.SupportedClients)
					{
						if (client.otbVersion == otbVersion)
						{
							return plugin;
						}
					}
				}

				return null;
			}
		}

		/// <summary>
		/// Data class for plugins.
		/// Holds and instance of the loaded plugin, as well as the plugins assembly path
		/// </summary>
		public class Plugin
		{
			private IPlugin myInstance = null;
			private string myAssemblyPath = "";

			public IPlugin Instance
			{
				get { return myInstance; }
				set { myInstance = value; }
			}

			public string AssemblyPath
			{
				get { return myAssemblyPath; }
				set { myAssemblyPath = value; }
			}
		}
	}
}

