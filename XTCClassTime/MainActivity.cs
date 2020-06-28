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

// 杰哥不要

namespace XTCClassTime
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.MainActivity", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, GestureDetector.IOnGestureListener
    {
        string[] terms = new string[]{ "apple", "banana", "peach", "wdnmd" };        
        //public ConfigObjs configs;

        string FmtInt(int x)
        {
            if (x < 10)
                return "0" + x.ToString();
            return x.ToString();
        }        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            SupportActionBar.Hide();

            //ArrayAdapter<string> WdnmdAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, terms);
            //(FindViewById<ListView>(Resource.Id.ListViewClasses)).Adapter = WdnmdAdapter;
            ClassTimeAdapter ctAdapter = new ClassTimeAdapter(this, DataController.GetClasses((int)DateTime.Now.DayOfWeek));
            (FindViewById<ListView>(Resource.Id.ListViewClasses)).Adapter = ctAdapter;
            (FindViewById<Button>(Resource.Id.SettingsButton)).Click +=
                (sender, e) =>
                {
                    //GetClassesProfile();
                    var intent = new Intent(this, typeof(SettingsActivity));
                    //var intent = new Intent(this, typeof(DeleteClassActivity));
                    StartActivity(intent);
                };

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);


            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool OnDown(MotionEvent e)
        {
            //throw new System.NotImplementedException();
            return false;
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            if (e1.GetX() - e2.GetX() > 10)
            {
                Toast.MakeText(this, "Wdnmd", ToastLength.Long);
                Intent intent = new Intent(this, typeof(SettingsActivity));
                StartActivity(intent);
                return true;
            }
            return false;
        }

        public void OnLongPress(MotionEvent e)
        {
            //return false;
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            return false;
        }

        public void OnShowPress(MotionEvent e)
        {
            //throw new System.NotImplementedException();
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            return false;
        }
    }
}
