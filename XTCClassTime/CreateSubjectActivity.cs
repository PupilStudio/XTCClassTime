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
        private const string ACTIVITY_NAME = "CreateSubject";

        string color, dispName;
        string beforeChange = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (DataController.StartedActivity.ContainsKey(ACTIVITY_NAME) && DataController.StartedActivity[ACTIVITY_NAME])
            {
                this.Finish();
                return;
            }
            DataController.StartedActivity[ACTIVITY_NAME] = true;

            SetContentView(Resource.Layout.activity_create_subject);
            color = "Unknown";

            FindViewById<EditText>(Resource.Id.ViewSubjectName).Visibility = ViewStates.Gone;
            FindViewById<EditText>(Resource.Id.InputSubjectName).Visibility = ViewStates.Visible;
            if (Intent.GetBooleanExtra("Edit", false))
            {
                FindViewById<ImageView>(Resource.Id.NewSubjectImage).SetImageResource(
                    DataController.GetClassImage(Intent.GetStringExtra("Name"), out color));
                //FindViewById<EditText>(Resource.Id.ViewSubjectName).Text = Intent.GetStringExtra("Name");
                FindViewById<EditText>(Resource.Id.InputSubjectName).Text = Intent.GetStringExtra("Name");
                //FindViewById<EditText>(Resource.Id.ViewSubjectName).Visibility = ViewStates.Visible;
                //FindViewById<EditText>(Resource.Id.InputSubjectName).Visibility = ViewStates.Gone;
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
                FindViewById<TextView>(Resource.Id.AddSubjectTextView).Text = "编辑科目";
                FindViewById<EditText>(Resource.Id.InputSubjectName).Enabled = true;
                beforeChange = Intent.GetStringExtra("Name");
            }

            FindViewById<Button>(Resource.Id.PickColorButton).Click += (sender, e) =>
            {
                StartActivityForResult(new Intent(this, typeof(PickColorActivity)), 9810);
            };
            FindViewById<Button>(Resource.Id.CancelCreateSubjectButton).Click += (sender, e) =>
            {
                this.SetResult(Result.Canceled);
                this.Finish();
                return;
            };
            FindViewById<Button>(Resource.Id.CreateSubjectButton).Click += (sender, e) =>
            {
                dispName = FindViewById<EditText>(Resource.Id.InputSubjectName).Text;
                if (dispName == "*#1919810114514#*") // TEST CODE
                {
                    Toast.MakeText(this, "进入压力测试", ToastLength.Short).Show();
                    DataController.GenerateTestData();
                    Toast.MakeText(this, "测试数据生成完成, 请返回到主界面", ToastLength.Short).Show();
                    this.Finish();
                    return;
                }
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

                if (Intent.GetBooleanExtra("Edit", false)) 
                {
                    
                    if (beforeChange != dispName)
                    {
                        var subjects = DataController.GetSubjects();
                        if (subjects.Contains(dispName))
                        {
                            Toast.MakeText(this, "科目重复了!", ToastLength.Long).Show();
                            return;
                        }
                                                
                        try
                        {
                            DataController.ModifySubjectName(beforeChange, dispName);
                        } catch (System.Exception ee)
                        {
                            Toast.MakeText(this, ee.Message, ToastLength.Long).Show();
                            return;
                        }
                    }
                    DataController.ModifySubjectColor(dispName, color);
                    Toast.MakeText(this, "修改成功!", ToastLength.Short).Show();
                }
                else
                {
                    var subjects = DataController.GetSubjects();
                    if (subjects.Contains(dispName))
                    {
                        Toast.MakeText(this, "科目重复了!", ToastLength.Long).Show();
                        return;
                    }

                    DataController.CreatedSubjectName = dispName;
                    try
                    {
                        DataController.AddSubject(dispName, color);
                    } catch (System.Exception ee)
                    {
                        Toast.MakeText(this, ee.Message, ToastLength.Long).Show();
                        return;
                    }
                    Toast.MakeText(this, "添加成功!", ToastLength.Short).Show();
                }
                this.SetResult(Result.Ok);
                this.Finish();
                return;
            };
            // Create your application here
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 9810 && resultCode == Result.Ok)
            {
                FindViewById<ImageView>(Resource.Id.NewSubjectImage).SetImageResource(DataController.PickedColorResource);
                FindViewById<TextView>(Resource.Id.NewSubjectColor).Text = DataController.PickedColorName;
                color = DataController.PickedColorIndent;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DataController.StartedActivity[ACTIVITY_NAME] = false;
        }
    }
}