using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.Model
{
    public class SurveyQuestionDetail
    {
        public long SurveyQuestionDetailId { get; set; }

        public long SurveyQuestionId { get; set; }

        public long? DisplayOrder { get; set; }

        public string QuestionDetailText { get; set; }

        public string OptionSuggestion { get; set; }
    }
}
