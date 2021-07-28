using System;
using System.Reflection;
using System.Security.Principal;
using System.Threading;

namespace CSharpStudy
{
	class AppDomainProgram
	{
		static void AppDomainMain(string[] args)
		{
			// Create a new thread with a generic principal.
			Thread t = new Thread(new ThreadStart(PrintPrincipalInformation));
			t.Start();
			t.Join();

			// Set the principal policy to WindowsPrincipal.
			// 2018-05-16 12:32 获取当前Thread 的当前应用程序域。
			AppDomain currentDomain = AppDomain.CurrentDomain;
			//currentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
			currentDomain.SetPrincipalPolicy(PrincipalPolicy.UnauthenticatedPrincipal);

			// The new thread will have a Windows principal representing the
			// current user.
			t = new Thread(new ThreadStart(PrintPrincipalInformation));
			t.Start();
			t.Join();

			// Create a principal to use for new threads.
			IIdentity identity = new GenericIdentity("NewUser");
			IPrincipal principal = new GenericPrincipal(identity, null);
			currentDomain.SetThreadPrincipal(principal);

			// Create a new thread with the principal created above.
			t = new Thread(new ThreadStart(PrintPrincipalInformation));
			t.Start();
			t.Join();

			// Wait for user input before terminating.
			Console.ReadLine();
		}

		static void PrintPrincipalInformation()
		{
			IPrincipal curPrincipal = Thread.CurrentPrincipal;
			if (curPrincipal != null)
			{
				Console.WriteLine("Type: " + curPrincipal.GetType().Name);
				Console.WriteLine("Name: " + curPrincipal.Identity.Name);
				Console.WriteLine("Authenticated: " +
					curPrincipal.Identity.IsAuthenticated);
				Console.WriteLine();
			}
		}
	}

	class Module1
	{
		public static void Module1Main()
		{
			// Get and display the friendly name of the default AppDomain.
			string callingDomainName = Thread.GetDomain().FriendlyName;
			Console.WriteLine(callingDomainName);

			// Get and display the full name of the EXE assembly.
			string exeAssembly = Assembly.GetEntryAssembly().FullName;
			Console.WriteLine(exeAssembly);

			// Construct and initialize settings for a second AppDomain.
			// 2018-05-16 AppDomainSetup类为一个封闭类，表示可以添加到System.AppDomain的实例的程序集绑定信息。
			AppDomainSetup ads = new AppDomainSetup();
			ads.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;

			ads.DisallowBindingRedirects = false;
			ads.DisallowCodeDownload = true;
			ads.ConfigurationFile =
				AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            // Create the second AppDomain.
            // CreateDomain：使用指定的名称、证据和应用程序域设置信息创建新的应用程序域。
			AppDomain ad2 = AppDomain.CreateDomain("AD #2", null, ads);

            // Create an instance of MarshalbyRefType in the second AppDomain. 
            // A proxy to the object is returned.
            // 任何对象只能属于一个AppDomain。
            // AppDomain用来隔离对象，不同AppDomain之间的对象必须通过Proxy (reference type)或者Clone(value type)通信。
            // 引用类型需要继承System.MarshalByRefObject才能被Marshal / UnMarshal(Proxy)。
            // 值类型需要设置Serializable属性才能被Marshal / UnMarshal(Clone)。
			MarshalByRefType mbrt =
				(MarshalByRefType)ad2.CreateInstanceAndUnwrap(
					exeAssembly,
					typeof(MarshalByRefType).FullName
				);

			// Call a method on the object via the proxy, passing the 
			// default AppDomain's friendly name in as a parameter.
			mbrt.SomeMethod(callingDomainName);

			// Unload the second AppDomain. This deletes its object and 
			// invalidates the proxy object.
			AppDomain.Unload(ad2);
			try
			{
				// Call the method again. Note that this time it fails 
				// because the second AppDomain was unloaded.
				mbrt.SomeMethod(callingDomainName);
				Console.WriteLine("Sucessful call.");
			}
			catch (AppDomainUnloadedException)
			{
				Console.WriteLine("Failed call; this is expected.");
			}
		}
	}

	// Because this class is derived from MarshalByRefObject, a proxy 
	// to a MarshalByRefType object can be returned across an AppDomain 
	// boundary.
	public class MarshalByRefType : MarshalByRefObject
	{
		//  Call this method via a proxy.
		public void SomeMethod(string callingDomainName)
		{
			// Get this AppDomain's settings and display some of them.
			AppDomainSetup ads = AppDomain.CurrentDomain.SetupInformation;
			Console.WriteLine("AppName={0}, AppBase={1}, ConfigFile={2}",
				ads.ApplicationName,
				ads.ApplicationBase,
				ads.ConfigurationFile
			);

			// Display the name of the calling AppDomain and the name 
			// of the second domain.
			// NOTE: The application's thread has transitioned between 
			// AppDomains.
			Console.WriteLine("Calling from '{0}' to '{1}'.",
				callingDomainName,
				Thread.GetDomain().FriendlyName
			);
		}
	}

	/* This code produces output similar to the following: 

	AppDomainX.exe
	AppDomainX, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
	AppName=, AppBase=C:\AppDomain\bin, ConfigFile=C:\AppDomain\bin\AppDomainX.exe.config
	Calling from 'AppDomainX.exe' to 'AD #2'.
	Failed call; this is expected.
	 */
}
