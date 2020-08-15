using Android.Util;
using Android.Widget;
using System.Collections.Generic;
using System.IO;

namespace XTCClassTime
{

    public static class DataController
    {
        private const string DATA_PATH = "/data/data/cn.pupilstudio.xtcclasstime/";
        private const string CLASSES_PREFIX = "weekday";

        private const string IMAGES_DEFAULT =
            "语文 Chinese\n数学 Math\n英语 English\n音乐 Music\n美术 Art\n体育 PE\n信息技术 Unknown\n班会 Unknown\n校本 Unknown\n道德与法治 Unknown\n历史 Unknown\n政治 Unknown\n地理 Unknown\n物理 Unknown\n化学 Unknown\n生物 Unknown";

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
            string dataFilePath = System.IO.Path.Combine(DATA_PATH, CLASSES_PREFIX + week.ToString() + ".config");
            if (!File.Exists(dataFilePath))
            {
                File.WriteAllText(dataFilePath, "");
            }
            File.WriteAllText(dataFilePath,
                File.ReadAllText(dataFilePath) + ct.ToString() + '\n');
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
            foreach(var i in vs)
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
                    return Resource.Drawable.Chinese;
                case "Math":
                case "English":
                case "Music":
                case "Art":
                case "PE":
                case "Unknown":
                default:
                    return Resource.Drawable.UnknownClass;
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
    }
}