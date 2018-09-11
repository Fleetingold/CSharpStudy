using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSomeTh
{
    class CrossAppDomainDelegateProgram
    {
        static void CrossAppDomainDelegateMain(string[] args)
        {
            Console.WriteLine("CurrentAppDomain start!");
            //建立新的应用程序域对象
            AppDomain newAppDomain = AppDomain.CreateDomain("newAppDomain");

            //绑定CrossAppDomainDelegate的委托方法
            CrossAppDomainDelegate crossAppDomainDelegate = new CrossAppDomainDelegate(MyCallBack);

            //绑定DomainUnload的事件处理方法
            newAppDomain.DomainUnload += (obj, e) =>
            {
                Console.WriteLine("NewAppDomain unload!");
            };

            //调用委托
            newAppDomain.DoCallBack(crossAppDomainDelegate);
            AppDomain.Unload(newAppDomain);
            Console.ReadKey();
        }

        static public void MyCallBack()
        {
            string name = AppDomain.CurrentDomain.FriendlyName;
            for (int n = 0; n < 4; n++)
                Console.WriteLine(string.Format("  Do work in {0}........", name));
        }
    }
}
