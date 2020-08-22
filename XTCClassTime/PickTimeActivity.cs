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
    [Activity(Label = "PickTimeActivity")]
    public class PickTimeActivity : Activity
    {
        TextView hourText, minuteText;
        int minutes = 0;

        void RefreshView()
        {
            hourText.Text = (minutes / 60).ToString() + "时";
            minuteText.Text = (minutes % 60).ToString() + "分";
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_pick_time);

            hourText = FindViewById<TextView>(Resource.Id.HourText);
            minuteText = FindViewById<TextView>(Resource.Id.MinuteText);

            FindViewById<ImageButton>(Resource.Id.AddHour).Click += (sender, e) =>
            {
                minutes += 60;
                RefreshView();
            };
            FindViewById<ImageButton>(Resource.Id.AddMinute).Click += (sender, e) => 
            {
                minutes += 1;
                RefreshView();
            };
            FindViewById<ImageButton>(Resource.Id.AddMinute).LongClick += (sender, e) =>
            {
                minutes += 10;
                RefreshView();
            };

            FindViewById<ImageButton>(Resource.Id.MinusHour).Click += (sender, e) =>
            {
                if (minutes >= 60)
                {
                    minutes -= 60;
                    RefreshView();
                }
            };
            FindViewById<ImageButton>(Resource.Id.MinusMinute).Click += (sender, e) =>
            {
                if (minutes >= 1)
                {
                    minutes -= 1;
                    RefreshView();
                }
            };
            FindViewById<ImageButton>(Resource.Id.MinusMinute).LongClick += (sender, e) =>
            {
                if (minutes >= 10)
                {
                    minutes -= 10;
                    RefreshView();
                }
            };

            FindViewById<Button>(Resource.Id.CancelPickTimeButton).Click += (sender, e) =>
            {
                this.SetResult(Result.Canceled);
                this.Finish();
            };
            FindViewById<Button>(Resource.Id.PickTimeButton).Click += (sender, e) =>
            {
                Intent.PutExtra("RETURN_MINUTES", minutes);
                this.SetResult(Result.Ok);
                this.Finish();
            };

            // Create your application here
        }
    }
}