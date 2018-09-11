using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSomeTh
{
    /// <summary>
    /// 2018-05-21  HashTable的小栗子！
    /// </summary>
    class HashTableProgram
    {
        public static void HashTableMain()
        {
            //创建一个Hashtable实例
            Hashtable ht = new Hashtable();
            //添加keyvalue键值对
            ht.Add("E", "e");
            ht.Add("A", "a");
            ht.Add("C", "c");
            ht.Add("B", "b");

            //ht为一个Hashtable实例
            foreach (DictionaryEntry de in ht)
            {
                //de.Key对应于keyvalue键值对key
                Console.Write(de.Key + ":");
                //de.Key对应于keyvalue键值对value
                Console.WriteLine(de.Value);
            }

            //别忘了导入System.Collections
            ArrayList akeys = new ArrayList(ht.Keys);
            //按字母顺序进行排序
            akeys.Sort();
            foreach (string skey in akeys)
            {
                Console.Write(skey + ":");
                Console.WriteLine(ht[skey]); //排序后输出
            }

            string s = (string)ht["A"];

            //判断哈希表是否包含特定键,其返回值为true或false
            if (ht.Contains("E"))
                Console.WriteLine("the E key exist");

            //移除一个keyvalue键值对
            ht.Remove("C");

            //此处输出a
            Console.WriteLine(ht["A"]);

            //移除所有元素
            ht.Clear();

            //此处将不会有任何输出
            Console.WriteLine(ht["A"]);
            Console.ReadKey();
        }
    }
}
