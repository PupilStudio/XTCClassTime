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
using Org.Apache.Http.Entity;

namespace XTCClassTime
{
    [Activity(Label = "CreateClassActivity")]
    public class CreateClassActivity : AppCompatActivity
    {
        int begHour = -1, begMinute = -1, endHour = -1, endMinute = -1;
        string chgUUID, chgSubject;
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
                
        private int GetDisplayTime(int time)
        {
            return (time == -1) ? 0 : time;
        }
        void LoadSubjects()
        {
            subjects = DataController.GetSubjects();
            subjects.Insert(0, "选择科目");
            // TODO: 用户自定义科目
            subjects.Add("新建科目");
            FindViewById<Spinner>(Resource.Id.SpinnerChooseClass).Adapter =
                new ArrayAdapter(this, Resource.Layout.simple_spinner_xtc_item, subjects);
            if (Intent.GetBooleanExtra("Edit", false) 
                && FindViewById<Spinner>(Resource.Id.SpinnerChooseClass).SelectedItem.ToString() == "选择科目")
            {
                for (int i = 0; i < subjects.Count; ++i)
                {
                    if (subjects[i] == chgSubject)
                    {
                        FindViewById<Spinner>(Resource.Id.SpinnerChooseClass).SetSelection(i);
                        break;
                    }
                }
            }
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_create_class);
            SupportActionBar.Hide();

            week = Intent.GetIntExtra("Week", 0);

            if (Intent.GetBooleanExtra("Edit", false))
            {
                begHour = Intent.GetIntExtra("CurBeginHour", 0);
                begMinute = Intent.GetIntExtra("CurBeginMinute", 0);
                endHour = Intent.GetIntExtra("CurEndHour", 0);
                endMinute = Intent.GetIntExtra("CurEndMinute", 0);
                chgUUID = Intent.GetStringExtra("CurUUID");
                chgSubject = Intent.GetStringExtra("CurSubject");
                FindViewById<TextView>(Resource.Id.BeginTimeText).Text = FmtInt(begHour) + " : " + FmtInt(begMinute);
                FindViewById<TextView>(Resource.Id.EndTimeText).Text = FmtInt(endHour) + " : " + FmtInt(endMinute);
                FindViewById<Button>(Resource.Id.CreateClassButton).Text = "修改";
                FindViewById<TextView>(Resource.Id.AddClassTextView).Text = "修改课程";
            }

            string[] hours = new string[24], minutes = new string[60];
            for (int i = 0; i != 60; ++i)            {
                
                minutes[i] = FmtInt(i) + "分";
            }
            for (int i = 0; i != 24; ++i)
            {
                hours[i] = FmtInt(i) + "时";
            }


            FindViewById<TextView>(Resource.Id.SwitchBeginTimeButton).Click +=
                (sender, e) =>
                {
                    var intent = new Intent(this, typeof(PickTimeActivity));
                    intent.PutExtra("Title", "选择课程开始时间");
                    intent.PutExtra("Minutes", GetDisplayTime(begHour) * 60 + GetDisplayTime(begMinute));
                    StartActivityForResult(intent, 4);
                };
            FindViewById<TextView>(Resource.Id.SwitchEndTimeButton).Click +=
                (sender, e) =>
                {
                    var intent = new Intent(this, typeof(PickTimeActivity));
                    intent.PutExtra("Title", "选择课程结束时间");
                    intent.PutExtra("Minutes", GetDisplayTime(endHour) * 60 + GetDisplayTime(endMinute));
                    StartActivityForResult(intent, 5);
                };

            LoadSubjects();
            FindViewById<Spinner>(Resource.Id.SpinnerChooseClass).ItemSelected +=
                (sender, e) =>
                {
                    if(subjects[e.Position] == "新建科目")
                    {
                        var intent = new Intent(this, typeof(CreateSubjectActivity));
                        StartActivityForResult(intent, 9198);                        
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
                    if (Intent.GetBooleanExtra("Edit", false))
                    {
                        DataController.RemoveClass(week, chgUUID);
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

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 4 && resultCode == Result.Ok)
            {
                begHour = DataController.PickedMinute / 60;
                begMinute = DataController.PickedMinute % 60;
                FindViewById<TextView>(Resource.Id.BeginTimeText).Text = FmtInt(begHour) + " : " + FmtInt(begMinute);
            }
            if (requestCode == 5 && resultCode == Result.Ok)
            {
                endHour = DataController.PickedMinute / 60;
                endMinute = DataController.PickedMinute % 60;
                FindViewById<TextView>(Resource.Id.EndTimeText).Text = FmtInt(endHour) + " : " + FmtInt(endMinute);
            }
            if (requestCode == 9198)
            {
                LoadSubjects(); //刷新列表
            }
        }
    }
}