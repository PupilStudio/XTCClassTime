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
        int begHour = -1, begMinute = -1, endHour = -1, endMinute = -1;
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
                new ArrayAdapter(this, Resource.Layout.simple_spinner_xtc_item, subjects);
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


            FindViewById<TextView>(Resource.Id.SwitchBeginTimeTextView).Click +=
                (sender, e) =>
                {
                    TimePickerDialog dialog = new TimePickerDialog(this,
                        (sender, e) =>
                        {
                            FindViewById<TextView>(Resource.Id.SwitchBeginTimeTextView).Text =
                                "课程开始时间: " + FmtInt(e.HourOfDay) + ":" + FmtInt(e.Minute);
                            begHour = e.HourOfDay;
                            begMinute = e.Minute;
                        }, 0, 0, true);
                    dialog.Show();
                };
            FindViewById<TextView>(Resource.Id.SwitchEndTimeTextView).Click +=
                (sender, e) =>
                {
                    TimePickerDialog dialog = new TimePickerDialog(this,
                        (sender, e) =>
                        {
                            FindViewById<TextView>(Resource.Id.SwitchEndTimeTextView).Text =
                                "课程结束时间: " + FmtInt(e.HourOfDay) + ":" + FmtInt(e.Minute);
                            endHour = e.HourOfDay;
                            endMinute = e.Minute;
                        }, 0, 0, true);
                    dialog.Show();
                };

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
                        Toast.MakeText(this, "请选择科目!", ToastLength.Short).Show();
                        return;
                    }
                    if (begHour == -1 || begMinute == -1)
                    {
                        Toast.MakeText(this, "请选择课程开始时间!", ToastLength.Short).Show();
                        return;
                    }
                    if (endHour == -1 || endMinute == -1)
                    {
                        Toast.MakeText(this, "请选择课程结束时间!", ToastLength.Short).Show();
                        return;
                    }
                    if (begHour * 60 + begMinute >= endHour * 60 + endMinute)
                    {
                        Toast.MakeText(this, "课程开始时间必须早于课程结束时间!", ToastLength.Short).Show();
                        return;
                    }
                    ct.BeginHour = begHour;
                    ct.BeginMinute = begMinute;
                    ct.EndHour = endHour;
                    ct.EndMinute = endMinute;
                    DataController.AddClass(week, ct);
                    Toast.MakeText(this, "课程添加成功!", ToastLength.Short);
                    this.SetResult(Result.Ok);
                    this.Finish();
                    return;
                };

            // Create your application here
        }
    }
}