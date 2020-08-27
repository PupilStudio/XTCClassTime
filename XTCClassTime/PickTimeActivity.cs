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
        int hours = 0, minutes = 0;

        void RefreshView()
        {
            hourText.Text = hours.ToString() + "时";
            minuteText.Text = minutes.ToString() + "分";
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_pick_time);

            hourText = FindViewById<TextView>(Resource.Id.HourText);
            minuteText = FindViewById<TextView>(Resource.Id.MinuteText);

            FindViewById<TextView>(Resource.Id.TimePickerTitle).Text = Intent.GetStringExtra("Title");
            hours = Intent.GetIntExtra("Minutes", 0) / 60;
            minutes = Intent.GetIntExtra("Minutes", 0) % 60;

            FindViewById<ImageButton>(Resource.Id.AddHour).Click += (sender, e) =>
            {
                ++hours;
                if (hours == 24)
                    hours = 0;
                RefreshView();
            };
            FindViewById<ImageButton>(Resource.Id.AddHour).LongClick += (sender, e) => {
                hours += 4;
                if (hours >= 24)
                    hours -= 24;
                RefreshView();
            };
            FindViewById<ImageButton>(Resource.Id.AddMinute).Click += (sender, e) => 
            {
                ++minutes;
                if (minutes == 60)
                    minutes = 0;
                RefreshView();
            };
            FindViewById<ImageButton>(Resource.Id.AddMinute).LongClick += (sender, e) =>
            {
                minutes += 10;
                if (minutes >= 60)
                    minutes -= 60;
                RefreshView();
            };

            FindViewById<ImageButton>(Resource.Id.MinusHour).Click += (sender, e) =>
            {
                --hours;
                if (hours < 0)
                    hours = 23;
                RefreshView();
            };
            FindViewById<ImageButton>(Resource.Id.MinusHour).LongClick += (sender, e) => {
                hours -= 4;
                if (hours < 0)
                    hours += 24;
                RefreshView();
            };
            FindViewById<ImageButton>(Resource.Id.MinusMinute).Click += (sender, e) =>
            {
                minutes -= 1;
                if (minutes < 0)
                    minutes = 59;
                RefreshView();
            };
            FindViewById<ImageButton>(Resource.Id.MinusMinute).LongClick += (sender, e) =>
            {
                minutes -= 10;
                if (minutes < 0)
                    minutes += 60;
                RefreshView();
            };

            FindViewById<Button>(Resource.Id.CancelPickTimeButton).Click += (sender, e) =>
            {
                this.SetResult(Result.Canceled);
                this.Finish();
            };
            FindViewById<Button>(Resource.Id.PickTimeButton).Click += (sender, e) =>
            {
                var intent = new Intent();
                DataController.PickedMinute = hours * 60 + minutes;
                this.SetResult(Result.Ok, intent);
                this.Finish();
            };

            RefreshView();
            // Create your application here
        }
    }
}