using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Java.Lang.Reflect;
using Android.Gestures;
using Android.Views;
using Android.Content;
using Android.Util;
using System.IO;
using Newtonsoft.Json;

namespace XTCClassTime
{
    [Activity(Label = "DeleteClassActivity")]
    public class DeleteClassActivity : AppCompatActivity
    {
        int week;
        string UUID;

        void DeleteClass(object sender, object e)
        {
            DataController.RemoveClass(week, UUID);
            this.Finish();
        }

        void CancelDelete(object sender, object e)
        {
            this.Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SupportActionBar.Hide();
            SetContentView(Resource.Layout.activity_delete_class);

            week = Intent.GetIntExtra("Week", 0);
            UUID = Intent.GetStringExtra("ClassPosition");

            (FindViewById<Button>(Resource.Id.DeleteButton)).Click += DeleteClass;
            (FindViewById<Button>(Resource.Id.ReturnButton)).Click += CancelDelete;
            // Create your application here
        }
    }
}