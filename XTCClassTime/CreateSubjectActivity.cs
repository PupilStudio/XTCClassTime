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

            if (Intent.GetBooleanExtra("Edit", false))
            {
                FindViewById<ImageView>(Resource.Id.NewubjectImage).SetImageResource(
                    DataController.GetClassImage(Intent.GetStringExtra("Name"), out color));
                FindViewById<EditText>(Resource.Id.InputSubjectName).Text = Intent.GetStringExtra("Name");
                string dispColor;
                switch (color)
                {
                    case "Chinese":
                        dispColor = "橙色";
                        break;
                    case "Math":
                        dispColor = "蓝色";
                        break;
                    case "English":
                        dispColor = "绿色";
                        break;
                    case "PE":
                        dispColor = "红色";
                        break;
                    case "Art":
                        dispColor = "紫色";
                        break;
                    case "Music":
                        dispColor = "青色";
                        break;
                    default:
                        dispColor = "默认灰";
                        break;
                }
                FindViewById<TextView>(Resource.Id.NewSubjectColor).Text = dispColor;
                FindViewById<Button>(Resource.Id.CreateSubjectButton).Text = "修改";
            }

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
                if (dispName.Trim().Length == 0)
                {
                    Toast.MakeText(this, "请输入科目名称!", ToastLength.Long).Show();
                    return;
                }
                if (dispName.Contains(' '))
                {
                    Toast.MakeText(this, "科目名称不允许含有空格!", ToastLength.Long).Show();
                    return;
                }
                if (dispName == "未选择" || dispName == "新建科目")
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