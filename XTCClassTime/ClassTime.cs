using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XTCClassTime
{
    public class ClassTime : Java.Lang.Object, IComparable
    {
        public int BeginHour, BeginMinute, EndHour, EndMinute;
        public string ClassName;
        public string UUID;

        public override string ToString()
        {
            return BeginHour.ToString() + ' ' + BeginMinute.ToString() + ' ' + this.EndHour.ToString() + ' ' + this.EndMinute.ToString()
                + ' ' + ClassName + ' ' + UUID;
        }

        public int CompareTo(object obj)
        {
            ClassTime o = (ClassTime)obj;
            int lhs = BeginHour * 60 + BeginMinute;
            int rhs = o.BeginHour * 60 + o.BeginMinute;
            return lhs.CompareTo(rhs);
        }

        public ClassTime(string dataline)
        {
            string[] datas = dataline.Split(' ');
            BeginHour = int.Parse(datas[0]);
            BeginMinute = int.Parse(datas[1]);
            EndHour = int.Parse(datas[2]);
            EndMinute = int.Parse(datas[3]);
            ClassName = datas[4];
            UUID = datas[5];
        }

        public static bool operator <(ClassTime lhs, ClassTime rhs)
        {
            return lhs.BeginHour * 60 + lhs.BeginMinute < rhs.BeginHour * 60 + rhs.BeginMinute;
        }
        public static bool operator >(ClassTime lhs, ClassTime rhs)
        {
            return lhs.BeginHour * 60 + lhs.BeginMinute > rhs.BeginHour * 60 + rhs.BeginMinute;
        }
    }
}