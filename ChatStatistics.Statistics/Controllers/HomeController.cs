using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using ChatStatistics.Parser.Entites;
using Newtonsoft.Json;

namespace ChatStatistics.Statistics.Controllers
{
    public class HomeController: Controller
    {
        private List<Message> Messages
        {
            get
            {
                if(Session["Messages"] == null)
                {
                    string text = System.IO.File.ReadAllText(Server.MapPath(Url.Content("~/Content/messages.json")));
                    var messages = JsonConvert.DeserializeObject<List<Message>>(text);
                    Session["Messages"] = messages;
                }
                return Session["Messages"] as List<Message>;
            }
        }

        #region public

        public ActionResult Index()
        {
            List<Message> t = Messages; //need for init Session["Messages"] before ajax requests
            return View();
        }

        [HttpPost]
        public ActionResult Login(int uid)
        {
            if(uids.Contains(uid))
            {
                return PartialView("GraphicsPartial");
            }
            return null;
        }

        public JsonResult GetGeneralStats()
        {
            int[] jsDataGeneral = GetStatsByDates(Messages);
            return Json(jsDataGeneral, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGeneralStatsByUser()
        {
            return GetStatsByUser(Messages);
        }

        public JsonResult GetUsersStats()
        {
            object result = CovertMessagesToGroupedByUserSorted(Messages);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetUsersBySymbolsCountStats()
        {
            object result = CovertMessagesToGroupedByUserBySymbolsSorted(Messages);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUsersMessageSizeStats()
        {
            var groupedByUser = Messages.GroupBy(m => m.uid,
                    (uid, msgs) => new
                    {
                        User = ReplaceIdWithUserName(uid),
                        Msgs = msgs.ToList(),
                        Count = msgs.Count(),
                    }).ToList();

            var symbolsByUser = new Dictionary<string, int>();
            foreach(var group in groupedByUser)
            {
                var c = group.Msgs.Sum(msg => Regex.Split(msg.body.ToLower(), @"\W+").Sum(word => word.Length));
                symbolsByUser[group.User] = c;
            }

            var result = new Dictionary<string, int>();
            foreach(var s in symbolsByUser)
            {
                var size = s.Value / groupedByUser.Where(g => g.User == s.Key).Select(g => g.Count).FirstOrDefault();
                result[s.Key] = size;
            }

            var siszeResult = result.Select(s => new {User = s.Key, Size = s.Value});

            var sorted = siszeResult.OrderByDescending(g => g.Size);

            int[] series = sorted.Select(g => g.Size).ToArray();
            string[] categories = sorted.Select(g => g.User).ToArray();
            return Json(new {Series = series, Categories = categories}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOnlyWithAttachments()
        {
            List<Message> msgsWithAtt = Messages.Where(m => m.attachment != null).ToList();
            object result = CovertMessagesToGroupedByUserSorted(msgsWithAtt);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWordsStatistics()
        {
            List<KeyValuePair<string, int>> words = WordStats().ToList();

            int[] series = words.Select(w => w.Value).ToArray();
            string[] categories = words.Select(w => w.Key).ToArray();

            var result = new {Series = series, Categories = categories};
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOneWordStatistics(string word)
        {
            List<Message> filtredMessages = Messages.Where(m => m.body.ToLower().Contains(word.ToLower())).ToList();
            object result = CovertMessagesToGroupedByUserSorted(filtredMessages);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOneWordStatisticsTimelineByUser(string word)
        {
            List<Message> filtredMessages = Messages.Where(m => m.body.ToLower().Contains(word.ToLower())).ToList();
            return GetStatsByUser(filtredMessages);
        }

        #endregion

        #region private

        private JsonResult GetStatsByUser(List<Message> messages)
        {
            int[] pasha = GetStatsByDates(messages.Where(m => m.uid == 92159406));
            int[] maks = GetStatsByDates(messages.Where(m => m.uid == 794049));
            int[] denis = GetStatsByDates(messages.Where(m => m.uid == 2259936));
            int[] anton = GetStatsByDates(messages.Where(m => m.uid == 4053376));
            int[] nikita = GetStatsByDates(messages.Where(m => m.uid == 31605134));
            int[] stas = GetStatsByDates(messages.Where(m => m.uid == 2482307));
            int[] dima = GetStatsByDates(messages.Where(m => m.uid == 10047660));
            int[] misha = GetStatsByDates(messages.Where(m => m.uid == 171552775));
            int[] roma = GetStatsByDates(messages.Where(m => m.uid == 3597266));
            int[] pasha2 = GetStatsByDates(messages.Where(m => m.uid == 134575936));

            var result = new[]
            {
                pasha.Any(d => d != 0) ? new {name = "Паша", data = pasha} : null,
                maks.Any(d => d != 0) ? new {name = "Макс", data = maks} : null,
                denis.Any(d => d != 0) ? new {name = "Денис", data = denis} : null,
                anton.Any(d => d != 0) ? new {name = "Антон", data = anton} : null,
                nikita.Any(d => d != 0) ? new {name = "Никита", data = nikita} : null,
                stas.Any(d => d != 0) ? new {name = "Стас", data = stas} : null,
                dima.Any(d => d != 0) ? new {name = "Дима", data = dima} : null,
                misha.Any(d => d != 0) ? new {name = "Миша", data = misha} : null,
                roma.Any(d => d != 0) ? new {name = "Рома", data = roma} : null,
                pasha2.Any(d => d != 0) ? new {name = "Паша2", data = pasha2} : null
            };

            var retval = result.Where(r => r != null).ToArray();
            return Json(retval, JsonRequestBehavior.AllowGet);
        }

        private int[] GetStatsByDates(IEnumerable<Message> messages)
        {
            var allGrouppedByDate = Messages.GroupBy(m => m.DateParsed.Date,
                    (date, msgs) => new
                    {
                        Date = date,
                        Count = msgs.Count(),
                    }).ToList();

            var grouppedByDate = messages.GroupBy(m => m.DateParsed.Date,
                    (date, msgs) => new
                    {
                        Date = date,
                        Count = msgs.Count(),
                    }).ToList();

            DateTime[] dates = allGrouppedByDate.Select(m => m.Date).ToArray();

            int[] jsDataGeneral =
                    EachDay(dates.Min(), dates.Max())
                            .Select(day => grouppedByDate.Exists(i => i.Date == day)
                                    ? grouppedByDate.Where(g => g.Date == day).Select(i => i.Count).FirstOrDefault()
                                    : 0)
                            .ToArray();
            return jsDataGeneral;
        }

        private object CovertMessagesToGroupedByUserSorted(IEnumerable<Message> messages)
        {
            var groupedByUser = messages.GroupBy(m => m.uid,
                    (uid, msgs) => new
                    {
                        User = ReplaceIdWithUserName(uid),
                        Count = msgs.Count(),
                    }).ToList();
            var sorted = groupedByUser.OrderByDescending(g => g.Count);

            int[] series = sorted.Select(g => g.Count).ToArray();
            string[] categories = sorted.Select(g => g.User).ToArray();

            var result = new {Series = series, Categories = categories};
            return result;
        }

        private object CovertMessagesToGroupedByUserBySymbolsSorted(IEnumerable<Message> messages)
        {
            var groupedByUser = messages.GroupBy(m => m.uid,
                    (uid, msgs) => new
                    {
                        User = ReplaceIdWithUserName(uid),
                        Msgs = msgs
                    }).ToList();

            var symbolsByUser = new Dictionary<string, int>();
            foreach (var group in groupedByUser)
            {
                var c = group.Msgs.Sum(msg => Regex.Split(msg.body.ToLower(), @"\W+").Sum(word => word.Length));
                symbolsByUser[group.User] = c;
            }

            var symbolsResult = symbolsByUser.Select(s => new { User = s.Key, Symbols = s.Value });

            var sorted = symbolsResult.OrderByDescending(g => g.Symbols);

            int[] series = sorted.Select(g => g.Symbols).ToArray();
            string[] categories = sorted.Select(g => g.User).ToArray();
            var result = new { Series = series, Categories = categories };
            return result;
        }

        private IEnumerable<KeyValuePair<string, int>> WordStats()
        {
            Dictionary<string, int> db = GetWordsDb();

            foreach(string key in db.Keys.Where(k => k.Length < 3).ToList())
            {
                db.Remove(key);
            }

            foreach(var kvp in db.Where(kvp => kvp.Value <= 3).ToList())
            {
                db.Remove(kvp.Key);
            }
            List<KeyValuePair<string, int>> result = db.OrderByDescending(e => e.Value).ToList();
            return result;
        }

       private Dictionary<string, int> GetWordsDb()
        {
            var db = new Dictionary<string, int>();
            foreach(Message message in Messages)
            {
                string[] words = Regex.Split(message.body.ToLower(), @"\W+");
                foreach(string word in words)
                {
                    if(db.ContainsKey(word))
                    {
                        db[word]++;
                    }
                    else
                    {
                        db[word] = 1;
                    }
                }
            }
            db.Remove(string.Empty);
            return db;
        }

        private string ReplaceIdWithUserName(int uid)
        {
            switch(uid)
            {
                case 92159406:
                    return "Паша";
                case 794049:
                    return "Макс";
                case 2259936:
                    return "Денис";
                case 4053376:
                    return "Антон";
                case 31605134:
                    return "Никита";
                case 2482307:
                    return "Стас";
                case 10047660:
                    return "Дима";
                case 171552775:
                    return "Миша";
                case 3597266:
                    return "Рома";
                case 134575936:
                    return "Паша2";
            }
            return string.Empty;
        }

        private List<int> uids = new List<int> { 92159406, 794049, 2259936, 4053376, 31605134, 2482307, 10047660, 171552775, 3597266, 134575936 };

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
        {
            for(DateTime day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }

        #endregion
    }
}