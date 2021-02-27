using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XTCClassTime
{
    [Activity(Label = "DeleteSubjectActivity")]
    public class DeleteSubjectActivity : Activity
    {
        private const string ACTIVITY_NAME = "DeleteSubject";

        string subjName;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (DataController.StartedActivity.ContainsKey(ACTIVITY_NAME) && DataController.StartedActivity[ACTIVITY_NAME])
            {
                this.Finish();
                return;
            }
            DataController.StartedActivity[ACTIVITY_NAME] = true;

            SetContentView(Resource.Layout.activity_delete_subject);
            subjName = Intent.GetStringExtra("SubjectName");
            FindViewById<TextView>(Resource.Id.RemoveTitle).Text =
                "要删除" + subjName + "科目\n并删除所有\n" + subjName + "课吗?";
            FindViewById<ImageButton>(Resource.Id.SubjectReturnButton).Click +=
                (sender, e) =>
                {
                    this.Finish();
                };
            FindViewById<ImageButton>(Resource.Id.SubjectDeleteButton).Click += (sender, e) =>
            {
                DataController.RemoveSubject(subjName);
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