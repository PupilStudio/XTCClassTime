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
        private const string ACTIVITY_NAME = "DeleteClass";

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

            if (DataController.StartedActivity.ContainsKey(ACTIVITY_NAME) && DataController.StartedActivity[ACTIVITY_NAME])
            {
                this.Finish();
                return;
            }
            DataController.StartedActivity[ACTIVITY_NAME] = true;

            SupportActionBar.Hide();
            SetContentView(Resource.Layout.activity_delete_class);

            week = Intent.GetIntExtra("Week", 0);
            UUID = Intent.GetStringExtra("ClassPosition");

            //Toast.MakeText(this, UUID, ToastLength.Long).Show();
            (FindViewById<ImageButton>(Resource.Id.DeleteButton)).Click += DeleteClass;
            (FindViewById<ImageButton>(Resource.Id.ReturnButton)).Click += CancelDelete;
            // Create your application here
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DataController.StartedActivity[ACTIVITY_NAME] = false;
        }
    }
}