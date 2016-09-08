using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChatStatistics.Parser.Entites;
using Newtonsoft.Json;

namespace ChatStatistics.Parser
{
    class Program
    {
        static void Main(string[] args)
        {
//            Vk vk = new Vk();
//            var messages = vk.ParseMessages();
//
//            var serialized = JsonConvert.SerializeObject(messages);
//            const string PATH = @"C:\Users\dvo\Downloads\messages.json";
//            using (File.Create(PATH))
//            {
//            }
//            File.WriteAllText(PATH,serialized);
//            Console.WriteLine("Done. Messages: " + messages.Count);
//            Merge();
        }


        static void Merge()
        {
            const string PATH = @"C:\Users\dvo\Downloads\messages_old.json";
            const string PATH2 = @"C:\Users\dvo\Downloads\messages.json";

            string text = System.IO.File.ReadAllText(PATH);
            var messages = JsonConvert.DeserializeObject<List<Message>>(text);

            string text2 = System.IO.File.ReadAllText(PATH2);
            var messages2 = JsonConvert.DeserializeObject<List<Message>>(text2);

            var messages3 = messages.Union(messages2).Distinct().ToList();

            var groupedByMid = messages3.GroupBy(m => m.date +"_"+ m.uid,
                   (date, msgs) => new
                   {
                       Date = date,
                       Msgs = msgs.ToList(),
                       Count = msgs.Count(),
                   }).ToList();

            var fuck = groupedByMid.Where(g => g.Count > 1).ToList();

            var serialized = JsonConvert.SerializeObject(messages3);
            const string PATH3 = @"C:\Users\dvo\Downloads\messages_new.json";
            using (File.Create(PATH3))
            {
            }
            File.WriteAllText(PATH3, serialized);
        }
    }
}
