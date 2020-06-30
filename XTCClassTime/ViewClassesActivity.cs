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
using Java.Net;

namespace XTCClassTime
{
    [Activity(Label = "ViewClassesActivity")]
    public class ViewClassesActivity : AppCompatActivity
    {
        //string[] terms = new string[] { "apple", "banana", "peach", "wdnmd" };
        //public ConfigObjs configs;
        int week;

        string FmtInt(int x)
        {
            if (x < 10)
                return "0" + x.ToString();
            return x.ToString();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            week = Intent.GetIntExtra("Weeks", 0);

            SupportActionBar.Hide();
            SetContentView(Resource.Layout.activity_view_classes);


            FindViewById<ListView>(Resource.Id.ViewClassesList).Adapter
                = new ClassTimeAdapter(this, DataController.GetClasses(week));

            
            FindViewById<ListView>(Resource.Id.ViewClassesList).ItemLongClick += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(DeleteClassActivity));
                intent.PutExtra("Week", week);                           
                intent.PutExtra("ClassPosition", e.Position);
                StartActivity(intent);
                FindViewById<ListView>(Resource.Id.ViewClassesList).Adapter
                 = new ClassTimeAdapter(this, DataController.GetClasses(week));
            };
            FindViewById<Button>(Resource.Id.AddClassButton).Click +=
                (sender, e) =>
                {
                    var intent = new Intent(this, typeof(CreateClassActivity));
                    intent.PutExtra("Week", week);
                    StartActivity(intent);
                };
            // Create your application here
        }
    }
}