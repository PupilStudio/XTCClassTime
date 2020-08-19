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
    [Activity(Label = "EditSubjectActivity")]
    public class EditSubjectActivity : Activity
    {
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
            SetContentView(Resource.Layout.activity_edit_subject);
            UpdateSubjects();
            // Create your application here
        }
    }
}