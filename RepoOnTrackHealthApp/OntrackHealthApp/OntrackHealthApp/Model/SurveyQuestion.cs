using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.Model
{
    public class SurveyQuestion
    {
        public long SurveyQuestionId { get; set; }

        public long SurveyQuestionSetId { get; set; }

        public int? DisplayOrder { get; set; }

        public string SurveyQuestionText { get; set; }

        public string SurveyQuestionType { get; set; }

        public int SurveyQuestionTypeId { get; set; }

    }
}
