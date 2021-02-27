using Android.Util;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;

namespace XTCClassTime
{

    public static class DataController
    {
        private const string DATA_PATH = "/data/data/cn.pupilstudio.xtcclasstime/";
        private const string CLASSES_PREFIX = "weekday";

        private const string IMAGES_DEFAULT =
            "语文 Chinese\n数学 Math\n英语 English\n音乐 Music\n美术 Art\n体育 PE\n科学 Unknown\n信息技术 Unknown\n班会 Unknown\n校本 Unknown\n道德与法治 Unknown\n历史 Unknown\n政治 Unknown\n地理 Unknown\n物理 Unknown\n化学 Unknown\n生物 Unknown\n";

        public static int PickedMinute = 0; // UGLY DESIGN!!
        public static string PickedSubject = "未选择"; // UGLY DESIGN!!
        public static int PickedColorResource = Resource.Drawable.DefaultX; // UGLY DESIGN!!
        public static string PickedColorName = "默认灰"; // UGLY DESIGN!!
        public static string PickedColorIndent = "Unknown"; // UGLY DESIGN!!
        public static string CreatedSubjectName = ""; // UGLY DESIGN!!
        public static Dictionary<string, bool> StartedActivity = new Dictionary<string, bool>(); // UGLY DESIGN!!

        /// <summary>
        /// 删除一节课
        /// </summary>
        /// <param name="week">这节课在星期几</param>
        /// <param name="UUID">这节课的UUID</param>
        public static void RemoveClass(int week, string UUID)
        {
            string dataFilePath = System.IO.Path.Combine(DATA_PATH, CLASSES_PREFIX + week.ToString() + ".config");
            string classesText = File.ReadAllText(dataFilePath);

            string[] classes = classesText.Split('\n');
            string fileText = "";
            foreach (var i in classes)
            {
                if (i.Trim() == "")
                    continue;
                if (i.Contains(UUID))
                    continue;
                fileText += i + '\n';
            }
            File.WriteAllText(dataFilePath, fileText);
        }

        /// <summary>
        /// 获取这天的所有课程
        /// </summary>
        /// <param name="week">查询星期几的课程</param>
        /// <returns>课程列表</returns>
        public static List<ClassTime> GetClasses(int week)
        {
            Log.Warn("GetClasses", "Start");
            string dataFilePath = Path.Combine(DATA_PATH, CLASSES_PREFIX + week.ToString() + ".config");
            if (!File.Exists(dataFilePath))
            {
                File.WriteAllText(dataFilePath, "");
            }
            string classesText = File.ReadAllText(dataFilePath);

            string[] classes = classesText.Split('\n');
            List<ClassTime> classTimes = new List<ClassTime>();
            foreach(var i in classes)
            {
                if (i.Trim() == "")
                    continue;
                classTimes.Add(new ClassTime(i));
                Log.Warn("GetClasses", i);
            }
            classTimes.Sort();
            Log.Warn("GetClasses", "End");
            return classTimes;
        }

        /// <summary>
        /// 在指定的日期添加课程
        /// </summary>
        /// <param name="week">在星期几添加</param>
        /// <param name="ct">课程的描述。UUID默认由此函数生成。</param>
        /// <param name="genUUID">是否由此函数生成UUID。默认为True.</param>
        public static void AddClass(int week, ClassTime ct, bool genUUID = true)
        {
            if (genUUID)
                ct.UUID = System.Guid.NewGuid().ToString();
            string dataFilePath = Path.Combine(DATA_PATH, CLASSES_PREFIX + week.ToString() + ".config");
            if (!File.Exists(dataFilePath))
            {
                File.WriteAllText(dataFilePath, "");
            }
            var classes = GetClasses(week);
            if (classes.Count >= 20)
            {
                throw new System.Exception("课程数量达到上限!");
            }
            foreach (var i in classes)
            {
                if ((i.BeginHour * 60 + i.BeginMinute <= ct.BeginHour * 60 + ct.BeginMinute
                    && ct.BeginHour * 60 + ct.BeginMinute <= i.EndHour * 60 + i.EndMinute) ||
                    (i.BeginHour * 60 + i.BeginMinute <= ct.EndHour * 60 + ct.EndMinute
                    && ct.EndHour * 60 + ct.EndMinute <= i.EndHour * 60 + i.EndMinute))
                {
                    throw new System.Exception("课程冲突!");
                }
            }
            File.WriteAllText(dataFilePath,
                File.ReadAllText(dataFilePath) + ct.ToString() + '\n');
        }

        /// <summary>
        /// 根据课程名称返回对应的图像
        /// </summary>
        /// <param name="name">课程名称</param>
        /// <param name="subjName">输出课程的颜色</param>
        /// <returns>对应图片的id</returns>
        public static int GetClassImage(string name, out string subjName)
        {
            List<string> classes = new List<string>(), imgids = new List<string>();
            string imgConfFilePath = System.IO.Path.Combine(DATA_PATH, "images.conf");
            if (!File.Exists(imgConfFilePath))
            {
                File.WriteAllText(imgConfFilePath, IMAGES_DEFAULT);
            }
            string imgConfig = File.ReadAllText(imgConfFilePath);

            string[] vs = imgConfig.Split('\n');
            foreach(var i in vs)
            {
                if (i.Trim() == "")
                    continue;
                string[] vss = i.Split(' ');
                classes.Add(vss[0]);
                imgids.Add(vss[1]);
            }

            string imgId = imgids[classes.FindIndex((a) => a == name)];
            subjName = imgId;
            switch (imgId)
            {
                case "Chinese":
                    return Resource.Drawable.ChineseX;
                case "Math":
                    return Resource.Drawable.MathX;
                case "English":
                    return Resource.Drawable.EnglishX;
                case "Music":
                    return Resource.Drawable.MusicX;
                case "Art":
                    return Resource.Drawable.ArtX;
                case "PE":
                    return Resource.Drawable.PEX;
                case "Unknown":
                default:
                    return Resource.Drawable.DefaultX;
            }
        }

        /// <summary>
        /// 根据课程名称返回对应的图像
        /// </summary>
        /// <param name="name">课程名称</param>
        /// <returns>对应图片的id</returns>
        public static int GetClassImage(string name)
        {
            List<string> classes = new List<string>(), imgids = new List<string>();
            string imgConfFilePath = System.IO.Path.Combine(DATA_PATH, "images.conf");
            if (!File.Exists(imgConfFilePath))
            {
                File.WriteAllText(imgConfFilePath, IMAGES_DEFAULT);
            }
            string imgConfig = File.ReadAllText(imgConfFilePath);

            string[] vs = imgConfig.Split('\n');
            foreach (var i in vs)
            {
                if (i.Trim() == "")
                    continue;
                string[] vss = i.Split(' ');
                classes.Add(vss[0]);
                imgids.Add(vss[1]);
            }

            string imgId = imgids[classes.FindIndex((a) => a == name)];
            switch (imgId)
            {
                case "Chinese":
                    return Resource.Drawable.ChineseX;
                case "Math":
                    return Resource.Drawable.MathX;
                case "English":
                    return Resource.Drawable.EnglishX;
                case "Music":
                    return Resource.Drawable.MusicX;
                case "Art":
                    return Resource.Drawable.ArtX;
                case "PE":
                    return Resource.Drawable.PEX;
                case "Unknown":
                default:
                    return Resource.Drawable.DefaultX;
            }
        }

        /// <summary>
        /// 获取有记录的所有课程
        /// </summary>
        /// <returns>所有课程的名称</returns>
        public static List<string> GetSubjects()
        {
            string imgConfFilePath = System.IO.Path.Combine(DATA_PATH, "images.conf");
            if (!File.Exists(imgConfFilePath))
            {
                File.WriteAllText(imgConfFilePath, IMAGES_DEFAULT);
            }
            string imgConfig = File.ReadAllText(imgConfFilePath);

            string[] vs = imgConfig.Split('\n');
            List<string> subjects = new List<string>();
            foreach (var i in vs)
            {
                if (i.Trim() == "")
                    continue;
                string[] vss = i.Split(' ');
                subjects.Add(vss[0]);
            }
            return subjects;
        }

        public static List<ClassImage> GetSubjectsImage()
        {
            string imgConfFilePath = System.IO.Path.Combine(DATA_PATH, "images.conf");
            if (!File.Exists(imgConfFilePath))
            {
                File.WriteAllText(imgConfFilePath, IMAGES_DEFAULT);
            }
            string imgConfig = File.ReadAllText(imgConfFilePath);

            string[] vs = imgConfig.Split('\n');
            List<ClassImage> images = new List<ClassImage>();
            foreach (var i in vs)
            {
                if (i.Trim() == "")
                    continue;
                string[] vss = i.Split(' ');
                ClassImage ci = new ClassImage();
                ci.DisplayName = vss[0];
                ci.Name = vss[1];
                images.Add(ci);
            }
            return images;
        }

        public static void AddSubject(string subjectName, string color)
        {
            string imgConfFilePath = System.IO.Path.Combine(DATA_PATH, "images.conf");
            if (!File.Exists(imgConfFilePath))
            {
                File.WriteAllText(imgConfFilePath, IMAGES_DEFAULT);
            }
            var subjects = GetSubjects();
            if (subjects.Contains(subjectName))
            {
                throw new System.Exception("科目重复了!");
            }
            if (subjects.Count >= 50)
            {
                throw new System.Exception("科目数量达到上限!");
            }
            File.WriteAllText(imgConfFilePath,
                File.ReadAllText(imgConfFilePath) + subjectName + " " + color + "\n");
        }

        public static void ModifySubjectColor(string subjectName, string color)
        {
            string imgConfFilePath = System.IO.Path.Combine(DATA_PATH, "images.conf");
            if (!File.Exists(imgConfFilePath))
            {
                File.WriteAllText(imgConfFilePath, IMAGES_DEFAULT);
            }
            string imgConfig = File.ReadAllText(imgConfFilePath);

            string[] vs = imgConfig.Split('\n');
            string content = "";
            foreach (var i in vs)
            {
                if (i.Trim() == "")
                    continue;
                string[] vss = i.Split(' ');
                string cur = i;
                if (vss[0] == subjectName)
                {
                    cur = subjectName + " " + color;
                }
                content += cur + "\n";
            }
            File.WriteAllText(imgConfFilePath, content);
        }

        public static void ModifySubjectName(string currentName, string modifiedName)
        {
            var items = GetSubjectsImage();
            foreach (var i in items)
            {
                if (i.DisplayName == modifiedName)
                {
                    throw new System.Exception("科目重复了!");
                }
            }

            string content = "";
            foreach (var i in items)
            {
                if (i.DisplayName == currentName)
                {
                    i.DisplayName = modifiedName;
                }
                content = content + i.DisplayName + " " + i.Name + "\n";
            }

            string imgConfFilePath = System.IO.Path.Combine(DATA_PATH, "images.conf");
            File.WriteAllText(imgConfFilePath, content);

            for (int i = 0; i != 7; ++i)
            {
                string path = System.IO.Path.Combine(DATA_PATH, "weekday" + i.ToString() + ".config");
                content = "";
                var vs = GetClasses(i);
                foreach (var j in vs)
                {
                    if (j.ClassName == currentName)
                        j.ClassName = modifiedName;
                    content += j.ToString() + "\n";
                }
                File.WriteAllText(path, content);
            }
        }

        public static void RemoveSubject(string subjectName)
        {
            var items = GetSubjectsImage();
            string content = "";
            foreach (var i in items)
            {
                if (i.DisplayName == subjectName) continue;
                content = content + i.DisplayName + " " + i.Name + "\n";
            }

            string imgConfFilePath = System.IO.Path.Combine(DATA_PATH, "images.conf");
            File.WriteAllText(imgConfFilePath, content);

            for (int i = 0; i != 7; ++i)
            {
                string path = System.IO.Path.Combine(DATA_PATH, "weekday" + i.ToString() + ".config");
                content = "";
                var vs = GetClasses(i);
                foreach (var j in vs)
                {
                    if (j.ClassName == subjectName) continue;
                    content += j.ToString() + "\n";
                }
                File.WriteAllText(path, content);
            }
        }
        /// <summary>
        /// Only for testing.
        /// </summary>
        public static void GenerateTestData() // ONLY FOR TEST
        {
            Random r = new Random();
            var colors = new string[] {"Chinese", "Math", "English", "Art", "Music", "PE", "Unknown" };
            while (true)
            {
                try
                {
                    DataController.AddSubject(System.Guid.NewGuid().ToString().Substring(0, 4), colors[r.Next(0, 7)]);
                }
                catch (Exception)
                {
                    break;
                }
            }
            var subjects = DataController.GetSubjects();            
            for (int i = 0; i < 7; ++i)
            {
                for (int j = 0; j < 20; ++j)
                {
                    ClassTime ct = new ClassTime();
                    ct.BeginHour = 2 * j / 60;
                    ct.BeginMinute = 2 * j % 60;
                    ct.EndHour = (2 * j + 1) / 60;
                    ct.EndMinute = (2 * j + 1) % 60;
                    ct.ClassName = subjects[r.Next(0, subjects.Count)];
                    try
                    {
                        DataController.AddClass(i, ct);
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            }
        }
    }
}