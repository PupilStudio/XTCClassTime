using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Java.Lang.Reflect;
using Android.Gestures;
using Android.Views;
using Android.Content;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Android.Util;
using System;
using System.Linq;
using System.Collections.Generic;

namespace XTCClassTime
{
    [Activity(Label = "CreateClassActivity")]
    public class CreateClassActivity : AppCompatActivity
    {
        int week;
        List<string> subjects;

        private string FmtInt(int x)
        {
            string s = x.ToString();
            if (s.Length == 1)
            {
                s = '0' + s;
            }
            return s;
        }

        void LoadSubjects()
        {
            subjects = DataController.GetSubjects();
            subjects.Insert(0, "选择科目");
            // TODO: 用户自定义科目
            //subjects.Add("新建科目");
            FindViewById<Spinner>(Resource.Id.SpinnerChooseClass).Adapter =
                new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, subjects);
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_create_class);
            SupportActionBar.Hide();

            week = Intent.GetIntExtra("Week", 0);

            string[] hours = new string[24], minutes = new string[60];
            for (int i = 0; i != 60; ++i)            {
                
                minutes[i] = FmtInt(i) + "分";
            }
            for (int i = 0; i != 24; ++i)
            {
                hours[i] = FmtInt(i) + "时";
            }

            FindViewById<Spinner>(Resource.Id.SpinnerBeginHour).Adapter
                //= FindViewById<Spinner>(Resource.Id.SpinnerBeginMinute).Adapter
                = FindViewById<Spinner>(Resource.Id.SpinnerEndHour).Adapter
                //= FindViewById<Spinner>(Resource.Id.SpinnerEndMinute).Adapter
                = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, hours);
            FindViewById<Spinner>(Resource.Id.SpinnerBeginMinute).Adapter
                = FindViewById<Spinner>(Resource.Id.SpinnerEndMinute).Adapter
                = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, minutes);

            LoadSubjects();
            FindViewById<Spinner>(Resource.Id.SpinnerChooseClass).ItemSelected +=
                (sender, e) =>
                {
                    if(subjects[e.Position] == "新建科目")
                    {
                        // TODO: Call 
                        LoadSubjects(); //刷新列表
                    }
                };
            FindViewById<Button>(Resource.Id.CancelCreateClassButton).Click +=
                (sender, e) =>
                {
                    this.Finish();
                };
            FindViewById<Button>(Resource.Id.CreateClassButton).Click +=
                (sender, e) =>
                {
                    ClassTime ct = new ClassTime();
                    ct.ClassName = FindViewById<Spinner>(Resource.Id.SpinnerChooseClass).SelectedItem.ToString();
                    if (ct.ClassName == "选择科目")
                    {
                        Toast.MakeText(this, "请选择科目!", ToastLength.Short);
                        return;
                    }
                    ct.BeginHour = 
                        int.Parse(FindViewById<Spinner>(Resource.Id.SpinnerBeginHour).SelectedItem.ToString().Substring(0,2));
                    ct.BeginMinute = 
                        int.Parse(FindViewById<Spinner>(Resource.Id.SpinnerBeginMinute).SelectedItem.ToString().Substring(0, 2));
                    ct.EndHour = 
                        int.Parse(FindViewById<Spinner>(Resource.Id.SpinnerEndHour).SelectedItem.ToString().Substring(0, 2));
                    ct.EndMinute = 
                        int.Parse(FindViewById<Spinner>(Resource.Id.SpinnerEndMinute).SelectedItem.ToString().Substring(0, 2));
                    DataController.AddClass(week, ct);
                    Toast.MakeText(this, "课程添加成功!", ToastLength.Short);
                    this.Finish();
                };

            // Create your application here
        }
    }
}