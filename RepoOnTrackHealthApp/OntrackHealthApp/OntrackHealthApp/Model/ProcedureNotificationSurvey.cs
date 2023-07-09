using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.Model
{
    public class ProcedureNotificationSurvey
    {
        public long NotificationId { get; set; }

        public long SurveyQuestionSetId { get; set; }

        public int DisplayOrder { get; set; }
    }
}
