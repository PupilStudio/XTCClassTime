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
using Java.Lang;

namespace XTCClassTime
{
    [Activity(Label = "CreateSubjectActivity")]
    public class CreateSubjectActivity : Activity
    {
        string color, dispName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_create_subject);
            color = "Unknown";
            FindViewById<Button>(Resource.Id.CancelCreateSubjectButton).Click += (sender, e) =>
            {
                this.SetResult(Result.Canceled);
                this.Finish();
                return;
            };
            FindViewById<Button>(Resource.Id.CreateSubjectButton).Click += (sender, e) =>
            {
                dispName = FindViewById<EditText>(Resource.Id.InputSubjectName).Text;
                if (dispName.Length > 5)
                {
                    Toast.MakeText(this, "科目名称太长了, 换一个吧!", ToastLength.Long).Show();
                    return;
                }
                if (dispName == "选择科目" || dispName == "新建科目")
                {
                    Toast.MakeText(this, "此名称不允许使用!", ToastLength.Long).Show();
                    return;
                }

                var subjects = DataController.GetSubjects();
                if (subjects.Contains(dispName))
                {
                    Toast.MakeText(this, "科目重复了!", ToastLength.Long).Show();
                    return;
                }

                DataController.AddSubject(dispName, color);
                Toast.MakeText(this, "添加成功!", ToastLength.Short).Show();
                this.SetResult(Result.Ok);
                this.Finish();
                return;
            };
            // Create your application here
        }
    }
}