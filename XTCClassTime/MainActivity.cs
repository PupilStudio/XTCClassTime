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
            var classes = DataController.GetClasses((int)DateTime.Now.DayOfWeek);
            ClassTimeAdapter ctAdapter = new ClassTimeAdapter(this, classes);
            FindViewById<ListView>(Resource.Id.ListViewClasses).Adapter = ctAdapter;
            FindViewById<Button>(Resource.Id.AddClassThisWeekButton).Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CreateClassActivity));
                intent.PutExtra("Week", (int)DateTime.Now.DayOfWeek);
                StartActivityForResult(intent, 514);
            };

            if (classes.Count == 0)
            {
                FindViewById<TextView>(Resource.Id.NoClassTextView).Visibility = ViewStates.Visible;
                FindViewById<Button>(Resource.Id.AddClassThisWeekButton).Visibility = ViewStates.Visible;
                FindViewById<ListView>(Resource.Id.ListViewClasses).Visibility = ViewStates.Gone;
            } else
            {
                FindViewById<TextView>(Resource.Id.NoClassTextView).Visibility = ViewStates.Gone;
                FindViewById<Button>(Resource.Id.AddClassThisWeekButton).Visibility = ViewStates.Gone;
                FindViewById<ListView>(Resource.Id.ListViewClasses).Visibility = ViewStates.Visible;
            }

            FindViewById<Button>(Resource.Id.SettingsButton).Click +=
                (sender, e) =>
                {
                    //GetClassesProfile();
                    var intent = new Intent(this, typeof(WeekPickerActivity));
                    //var intent = new Intent(this, typeof(DeleteClassActivity));
                    StartActivityForResult(intent, 514);
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

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            var classes = DataController.GetClasses((int)DateTime.Now.DayOfWeek);
            FindViewById<ListView>(Resource.Id.ListViewClasses).Adapter = 
                new ClassTimeAdapter(this, classes);
            FindViewById<ListView>(Resource.Id.ListViewClasses).Invalidate();
            if (classes.Count == 0)
            {
                FindViewById<TextView>(Resource.Id.NoClassTextView).Visibility = ViewStates.Visible;
                FindViewById<Button>(Resource.Id.AddClassThisWeekButton).Visibility = ViewStates.Visible;
                FindViewById<ListView>(Resource.Id.ListViewClasses).Visibility = ViewStates.Gone;
            }
            else
            {
                FindViewById<TextView>(Resource.Id.NoClassTextView).Visibility = ViewStates.Gone;
                FindViewById<Button>(Resource.Id.AddClassThisWeekButton).Visibility = ViewStates.Gone;
                FindViewById<ListView>(Resource.Id.ListViewClasses).Visibility = ViewStates.Visible;
            }
        }
    }
}
