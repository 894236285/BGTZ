﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Native.Sdk.Cqp.Interface;
using cqp.tian.bgtz.me.Code;

namespace Native.Core
{
	/// <summary>
	/// 酷Q应用主入口类
	/// </summary>
	public class CQMain
	{
		/// <summary>
		/// 在应用被加载时将调用此方法进行事件注册, 请在此方法里向 <see cref="IUnityContainer"/> 容器中注册需要使用的事件
		/// </summary>
		/// <param name="container">用于注册的 IOC 容器 </param>
		public static void Register (IUnityContainer unityContainer)
		{
			//应用启动时
			unityContainer.RegisterType<IAppEnable, Event_AppEnable>("应用已被启用");

			//新人入群
			unityContainer.RegisterType<IGroupMemberIncrease, Event_GroupMemberPass>("群成员增加事件处理");
		}
	}
}
