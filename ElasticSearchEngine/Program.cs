using System;
using System.Collections.Generic;
using System.Linq;
using Nest;

namespace ElasticSearchEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            ESProvider esProvider = new ESProvider();
            MeetupEvents meetup = new MeetupEvents();
            meetup.eventid = 20180726;
            meetup.eventname = "testupdate";
            meetup.description = "desctestupdate";
            meetup.orignalid = "orignalidtestupdate";

            esProvider.PopulateIndex(meetup);

            List<MeetupEvents> list = new List<MeetupEvents>();

            MeetupEvents meetup1 = new MeetupEvents();
            meetup.eventid = 20180725-1;
            meetup.eventname = "test1";
            meetup.description = "desctest1";
            meetup.orignalid = "orignalidtest1";
            list.Add(meetup1);

            MeetupEvents meetup2 = new MeetupEvents();
            meetup.eventid = 20180725-2;
            meetup.eventname = "test2";
            meetup.description = "desctest2";
            meetup.orignalid = "orignalidtest2";
            list.Add(meetup2);

            bool result = esProvider.PopulateIndexMany(list);

            Console.WriteLine(result);

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }

    public static class Setting
    {
        public static string strConnectionString = @"http://localhost:9200";
        public static Uri Node
        {
            get
            {
                return new Uri(strConnectionString);
            }
        }
        public static ConnectionSettings ConnectionSettings
        {
            get
            {
                return new ConnectionSettings(Node).DefaultIndex("default");
            }
        }
    }

    public class MeetupEvents
    {
        public long eventid { get; set; }
        public string orignalid { get; set; }
        public string eventname { get; set; }
        public string description { get; set; }
    }

    public class ESProvider
    {
        public static ElasticClient client = new ElasticClient(Setting.ConnectionSettings);
        public static string strIndexName = @"meetup".ToLower();
        public static string strDocType = "events".ToLower();

        public bool PopulateIndex(MeetupEvents meetupevent)
        {
            var index = client.Index(meetupevent, i => i.Index(strIndexName).Type(strDocType).Id(meetupevent.eventid));
            return index.IsValid;
        }

        public bool BulkPopulateIndex(List<MeetupEvents> posts)
        {
            var bulkRequest = new BulkRequest(strIndexName, strDocType) { Operations = new List<IBulkOperation>() };
            var idxops = posts.Select(o => new BulkIndexOperation<MeetupEvents>(o) { Id = o.eventid }).Cast<IBulkOperation>().ToList();
            bulkRequest.Operations = idxops;
            var response = client.Bulk(bulkRequest);
            return response.IsValid;
        }

        public bool PopulateIndexMany(List<MeetupEvents> posts)
        {
            var index = client.IndexMany(posts, strIndexName, strDocType);
            return index.IsValid;
        }

        public object FindFilter()
        {
            
            return null;
        }
    }


}
