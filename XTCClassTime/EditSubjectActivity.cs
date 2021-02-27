﻿using System;
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
    [Activity(Label = "EditSubjectActivity")]
    public class EditSubjectActivity : Activity
    {
        private const string ACTIVITY_NAME = "EditSubject";

        void UpdateSubjects()
        {
            var subjects = DataController.GetSubjectsImage();
            FindViewById<ListView>(Resource.Id.SubjectsList).Adapter =
                new ClassImageAdapter(this, subjects);
            FindViewById<ListView>(Resource.Id.SubjectsList).Invalidate();
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

            SetContentView(Resource.Layout.activity_edit_subject);

            FindViewById<Button>(Resource.Id.AddSubjectButton).Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CreateSubjectActivity));
                StartActivityForResult(intent, 981);
            };

            if (Intent.GetBooleanExtra("Select", false))
            {
                FindViewById<TextView>(Resource.Id.MainCaptionTextView).Text = "选择科目";
                FindViewById<ListView>(Resource.Id.SubjectsList).ItemClick += (sender, e) =>
                {
                    DataController.PickedSubject =
                        ((ClassImage)FindViewById<ListView>(Resource.Id.SubjectsList).Adapter.GetItem(e.Position)).DisplayName;
                    this.SetResult(Result.Ok);
                    this.Finish();
                };
            }
            else
            {
                FindViewById<ListView>(Resource.Id.SubjectsList).ItemClick += (sender, e) =>
                {
                    var intent = new Intent(this, typeof(CreateSubjectActivity));
                    intent.PutExtra("Edit", true);
                    intent.PutExtra("Name",
                        ((ClassImage)FindViewById<ListView>(Resource.Id.SubjectsList).Adapter.GetItem(e.Position)).DisplayName);
                    StartActivityForResult(intent, 364);
                };
                FindViewById<ListView>(Resource.Id.SubjectsList).ItemLongClick += (sender, e) =>
                {
                    Intent intent = new Intent(this, typeof(DeleteSubjectActivity));
                    intent.PutExtra("SubjectName",
                        ((ClassImage)FindViewById<ListView>(Resource.Id.SubjectsList).Adapter.GetItem(e.Position)).DisplayName);
                    StartActivityForResult(intent, 443);
                };
            }
            
            UpdateSubjects();
            // Create your application here
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            UpdateSubjects();
            if (Intent.GetBooleanExtra("Select", false) && resultCode == Result.Ok)
            {
                DataController.PickedSubject = DataController.CreatedSubjectName;
                this.SetResult(Result.Ok);
                this.Finish();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DataController.StartedActivity[ACTIVITY_NAME] = false;
        }
    }
}