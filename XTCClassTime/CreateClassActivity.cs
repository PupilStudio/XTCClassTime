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
        List<string> subjects;
        void LoadSubjects()
        {
            subjects = DataController.GetSubjects();
            subjects.Insert(0, "选择科目");
            // TODO: 用户自定义科目
            //subjects.Add("新建科目");
            FindViewById<Spinner>(Resource.Id.SpinnerChooseClass).Adapter =
                new ArrayAdapter(this, Resource.Layout.list_simple_xtc_layout, subjects);
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_create_class);
            SupportActionBar.Hide();

            LoadSubjects();
            FindViewById<Spinner>(Resource.Id.SpinnerChooseClass).ItemClick +=
                (sender, e) =>
                {
                    if(subjects[e.Position] == "新建科目")
                    {
                        // TODO: Call 
                        LoadSubjects(); //刷新列表
                    }
                };

            // Create your application here
        }
    }
}