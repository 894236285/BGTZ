/*
 * 此文件由T4引擎自动生成, 请勿修改此文件中的代码!
 */
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Native.Core.Domain;
using Native.Sdk.Cqp;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Unity;

namespace Native.App.Export
{
	/// <summary>	
	/// 表示酷Q菜单导出的类	
	/// </summary>	
	public class CQMenuExport	
	{	
		#region --构造函数--	
		/// <summary>	
		/// 由托管环境初始化的 <see cref="CQMenuExport"/> 的新实例	
		/// </summary>	
		static CQMenuExport ()	
		{	
			
			// 调用方法进行实例化	
			ResolveBackcall ();	
		}	
		#endregion	
		
		#region --私有方法--	
		/// <summary>	
		/// 读取容器中的注册项, 进行事件分发	
		/// </summary>	
		private static void ResolveBackcall ()	
		{	
			/*	
			 * Name: 基础操作及设置	
			 * Function: Menu_OpenWindow	
			 */	
			if (AppData.UnityContainer.IsRegistered<IMenuCall> ("基础操作及设置"))	
			{	
				MenuMenu_OpenWindowHandler += AppData.UnityContainer.Resolve<IMenuCall> ("基础操作及设置").MenuCall;	
			}	
			
		}	
		#endregion	
		
		#region --导出方法--	
		/*	
		 * Name: 基础操作及设置	
		 * Function: Menu_OpenWindow	
		 */	
		public static event EventHandler<CQMenuCallEventArgs> MenuMenu_OpenWindowHandler;	
		[DllExport (ExportName = "Menu_OpenWindow", CallingConvention = CallingConvention.StdCall)]	
		public static int MenuMenu_OpenWindow ()	
		{	
			if (MenuMenu_OpenWindowHandler != null)	
			{	
				CQMenuCallEventArgs args = new CQMenuCallEventArgs (AppData.CQApi, AppData.CQLog, "基础操作及设置", "Menu_OpenWindow");	
				MenuMenu_OpenWindowHandler (typeof (CQMenuExport), args);	
			}	
			return 0;	
		}	
		
		#endregion	
	}	
}
