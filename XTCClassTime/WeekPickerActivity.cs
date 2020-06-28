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

namespace XTCClassTime
{
    [Activity(Label = "WeekPickerActivity")]
    public class WeekPickerActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_week_picker);
            SupportActionBar.Hide();

            string[] weeks = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            (FindViewById<ListView>(Resource.Id.WeeksListView)).Adapter
                = new ArrayAdapter(this, Resource.Layout.list_simple_xtc_layout, weeks);
            (FindViewById<ListView>(Resource.Id.WeeksListView)).ItemClick += (sender, e) =>
            {
                var intent = new Intent(this, typeof(ViewClassesActivity));
                intent.PutExtra("Weeks", e.Position);
                StartActivity(intent);
            };
            // Create your application here
        }
    }
}