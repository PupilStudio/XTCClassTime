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
    [Activity(Label = "PickColorActivity")]
    public class PickColorActivity : Activity
    {
        private const string ACTIVITY_NAME = "PickColor";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (DataController.StartedActivity.ContainsKey(ACTIVITY_NAME) && DataController.StartedActivity[ACTIVITY_NAME])
            {
                this.Finish();
                return;
            }
            DataController.StartedActivity[ACTIVITY_NAME] = true;

            SetContentView(Resource.Layout.activity_pick_color);

            FindViewById<Button>(Resource.Id.PickDefaultButton).Click += (sender, e) =>
            {
                DataController.PickedColorResource = Resource.Drawable.DefaultX;
                DataController.PickedColorName = "默认灰";
                DataController.PickedColorIndent = "Unknown";
                this.SetResult(Result.Ok);
                this.Finish();
            };
            FindViewById<Button>(Resource.Id.PickChineseButton).Click += (sender, e) =>
            {
                DataController.PickedColorResource = Resource.Drawable.ChineseX;
                DataController.PickedColorName = "橙色";
                DataController.PickedColorIndent = "Chinese";
                this.SetResult(Result.Ok);
                this.Finish();
            };
            FindViewById<Button>(Resource.Id.PickMathButton).Click += (sender, e) =>
            {
                DataController.PickedColorResource = Resource.Drawable.MathX;
                DataController.PickedColorName = "蓝色";
                DataController.PickedColorIndent = "Math";
                this.SetResult(Result.Ok);
                this.Finish();
            };
            FindViewById<Button>(Resource.Id.PickEnglishButton).Click += (sender, e) =>
            {
                DataController.PickedColorResource = Resource.Drawable.EnglishX;
                DataController.PickedColorName = "绿色";
                DataController.PickedColorIndent = "English";
                this.SetResult(Result.Ok);
                this.Finish();
            };
            FindViewById<Button>(Resource.Id.PickMusicButton).Click += (sender, e) =>
            {
                DataController.PickedColorResource = Resource.Drawable.MusicX;
                DataController.PickedColorName = "青色";
                DataController.PickedColorIndent = "Music";
                this.SetResult(Result.Ok);
                this.Finish();
            };
            FindViewById<Button>(Resource.Id.PickArtButton).Click += (sender, e) =>
            {
                DataController.PickedColorResource = Resource.Drawable.ArtX;
                DataController.PickedColorName = "紫色";
                DataController.PickedColorIndent = "Art";
                this.SetResult(Result.Ok);
                this.Finish();
            };
            FindViewById<Button>(Resource.Id.PickPEButton).Click += (sender, e) =>
            {
                DataController.PickedColorResource = Resource.Drawable.PEX;
                DataController.PickedColorName = "红色";
                DataController.PickedColorIndent = "PE";
                this.SetResult(Result.Ok);
                this.Finish();
            };
            // Create your application here
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DataController.StartedActivity[ACTIVITY_NAME] = false;
        }
    }
}