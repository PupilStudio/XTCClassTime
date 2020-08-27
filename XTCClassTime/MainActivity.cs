using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Content;
using System;

// 杰哥不要

namespace XTCClassTime
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.MainActivity", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        int week;
        string[] days = new string[] { "日", "一", "二", "三", "四", "五", "六" };

        string FmtInt(int x)
        {
            if (x < 10)
                return "0" + x.ToString();
            return x.ToString();
        }        

        void UpdateClasses()
        {
            var classes = DataController.GetClasses(week);
            FindViewById<ListView>(Resource.Id.ListViewClasses).Adapter =
                new ClassTimeAdapter(this, classes);
            FindViewById<ListView>(Resource.Id.ListViewClasses).Invalidate();
            if (classes.Count == 0)
            {
                FindViewById<TextView>(Resource.Id.NoClassTextView).Visibility = ViewStates.Visible;
                FindViewById<Button>(Resource.Id.AddClassTodayButton).Visibility = ViewStates.Visible;
                FindViewById<ListView>(Resource.Id.ListViewClasses).Visibility = ViewStates.Gone;
            }
            else
            {
                FindViewById<TextView>(Resource.Id.NoClassTextView).Visibility = ViewStates.Gone;
                FindViewById<Button>(Resource.Id.AddClassTodayButton).Visibility = ViewStates.Gone;
                FindViewById<ListView>(Resource.Id.ListViewClasses).Visibility = ViewStates.Visible;
            }

            string s;
            if (week == (int)DateTime.Now.DayOfWeek)
                s = "星期" + days[week] + " 今天";
            else
                s = "星期" + days[week];
            FindViewById<Button>(Resource.Id.DayDisplay).Text = s;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            SupportActionBar.Hide();
            ForbidReceiver.Register(this);

            week = (int)DateTime.Now.DayOfWeek;
            FindViewById<Button>(Resource.Id.AddClassTodayButton).Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CreateClassActivity));
                intent.PutExtra("Week", week);
                StartActivityForResult(intent, 514);
            };
            FindViewById<Button>(Resource.Id.AnotherAddClassButton).Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CreateClassActivity));
                intent.PutExtra("Week", week);
                StartActivityForResult(intent, 514);
            };
            FindViewById<ImageButton>(Resource.Id.PrevDayButton).Click += (sender, e) =>
            {
                if (week == 0)
                    week = 6;
                else
                    --week;
                UpdateClasses();
            };
            FindViewById<ImageButton>(Resource.Id.NextDayButton).Click += (sender, e) =>
            {
                if (week == 6)
                    week = 0;
                else
                    ++week;
                UpdateClasses();
            };

            UpdateClasses();
            FindViewById<ListView>(Resource.Id.ListViewClasses).ItemClick += (sender, e) => {
                var intent = new Intent(this, typeof(CreateClassActivity));
                var curClass = (ClassTime)FindViewById<ListView>(Resource.Id.ListViewClasses).Adapter.GetItem(e.Position);
                intent.PutExtra("Week", week);
                intent.PutExtra("Edit", true); // editing
                intent.PutExtra("CurBeginHour", curClass.BeginHour);
                intent.PutExtra("CurEndHour", curClass.EndHour);
                intent.PutExtra("CurBeginMinute", curClass.BeginMinute);
                intent.PutExtra("CurEndMinute", curClass.EndMinute);
                intent.PutExtra("CurUUID", curClass.UUID);
                intent.PutExtra("CurSubject", curClass.ClassName);
                StartActivityForResult(intent, 514);
            };
            FindViewById<ListView>(Resource.Id.ListViewClasses).ItemLongClick += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(DeleteClassActivity));
                intent.PutExtra("Week", week);
                intent.PutExtra("ClassPosition",
                    ((ClassTime)FindViewById<ListView>(Resource.Id.ListViewClasses).Adapter.GetItem(e.Position)).UUID);

                StartActivityForResult(intent, 114);
            };

            FindViewById<Button>(Resource.Id.SettingsButton).Click +=
                (sender, e) =>
                {
                    //GetClassesProfile();
                    //var intent = new Intent(this, typeof(WeekPickerActivity));
                    //var intent = new Intent(this, typeof(DeleteClassActivity));
                    //StartActivityForResult(intent, 514);
                    //ShareHelper.ShareText(this, "课程表支持分享啦!");
                    var intent = new Intent(this, typeof(EditSubjectActivity));
                    StartActivityForResult(intent, 191);
                };

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            UpdateClasses();
        }
    }
}
