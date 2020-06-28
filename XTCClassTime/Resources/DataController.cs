using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util.Functions;
using Android.Graphics;

namespace XTCClassTime
{
   
    public static class DataController
    {
        private const string DATA_PATH = "/data/data/cn.pupilstudio.xtcclasstime/";
        private const string CLASSES_PREFIX = "weekday";
        
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
            List<ClassTime> classTimes = new List<ClassTime>();
            foreach (var i in classes)
            {
                if (i.Trim() == "")
                    continue;
                if (i.Contains(UUID))
                    continue;
                classTimes.Add(new ClassTime(i));
            }
            classTimes.Sort();
        }

        /// <summary>
        /// 获取这天的所有课程
        /// </summary>
        /// <param name="week">查询星期几的课程</param>
        /// <returns>课程列表</returns>
        public static List<ClassTime> GetClasses(int week)
        {
            string dataFilePath = System.IO.Path.Combine(DATA_PATH, CLASSES_PREFIX + week.ToString() + ".config");
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
            }
            classTimes.Sort();
            return classTimes;
        }

        /// <summary>
        /// 在指定的日期添加课程
        /// </summary>
        /// <param name="week">在星期几添加</param>
        /// <param name="ct">课程的描述。UUID由此函数生成。</param>
        public static void AddClass(int week, ClassTime ct)
        {
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
                File.WriteAllText(imgConfFilePath, "");
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
                case "Math":
                case "English":
                case "Music":
                case "Art":
                case "PE":
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
                File.WriteAllText(imgConfFilePath, "");
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