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
    [Activity(Label = "AboutAuthorActivity")]
    public class AboutAuthorActivity : Activity
    {
        private const string ACTIVITY_NAME = "AboutAuthor";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (DataController.StartedActivity.ContainsKey(ACTIVITY_NAME) && DataController.StartedActivity[ACTIVITY_NAME])
            {
                this.Finish();
                return;
            }
            DataController.StartedActivity[ACTIVITY_NAME] = true;

            SetContentView(Resource.Layout.activity_about_author);
            // Create your application here
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DataController.StartedActivity[ACTIVITY_NAME] = false;
        }
    }
}