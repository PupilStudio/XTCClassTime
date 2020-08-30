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
    [Activity(Label = "MenuActivity", NoHistory = true)]
    public class MenuActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_menu);

            FindViewById<Button>(Resource.Id.GotoEditSubjectButton).Click += (source, e) =>
                StartActivity(new Intent(this, typeof(EditSubjectActivity)));
            FindViewById<Button>(Resource.Id.GotoAboutAuthorButton).Click += (sender, e) =>
                StartActivity(new Intent(this, typeof(AboutAuthorActivity)));
            FindViewById<Button>(Resource.Id.GotoAboutSoftwareButton).Click += (sender, e) =>
                StartActivity(new Intent(this, typeof(AboutSoftwareActivity)));
            // Create your application here
        }
    }
}