namespace OntrackHealthApp.ApiService.ViewModel
{
    public class ResourceDetailViewModel
    {
        public int PatientPortalResourceDetailId { get; set; }

        public int PatientPortalResourceId { get; set; }

        public string ResourceCategoryName { get; set; }

        public string PatientPortalResourceName { get; set; }

        public string ResourceContentCombineId { get; set; }

        public string PatientPortalResourceContent { get; set; }

        public string PatientPortalResourceContentCustom
        {
            get {
                var str = "<html>"
                    + " <head>"
                    + " <meta charset=\"utf-8\" />"
                    + " <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">"
                    + " <style type=\"text/css\">"
                    + " @font-face {"
                    + " font-family: georgia;"
                    + " src: url('Fonts/georgia.ttf');"
                    + " } body{font-family: georgia; font-size:16px;padding:0; margin:0;}"
                    + " * {box-sizing: border-box;-moz-box-sizing: border-box;-webkit-box-sizing: border-box;}"
                    + " ul { margin:0px; margin-bottom: 10px; padding:0px; margin-left:15px} li{list-style: disc;margin-bottom: 10px;}"
                    + " ul li ul{ margin:0px; margin-bottom: 10px; padding:0px; margin-left:30px}"
                    + " h2{margin:0px; padding:0px; font-size:18px;margin-bottom:15px;} "
                    + " h3{margin:0px; padding:0px; font-size:17px;margin-bottom:15px;}  "
                    + " h4{margin:0px; padding:0px; font-size:16px;margin-bottom:15px;} "
                    + " .row {margin-right:0px; margin-left:0px;} .row:after{clear: both;} .col-md-12{float: left; width: 100%; position: relative; min-height: 1px;padding-right: 15px;padding-left: 15px}"
                    + " table{border-collapse: collapse;}"
                    + " td{border-bottom:1px solid #777;vertical-align: middle;padding-top:6px; padding-bottom:6px;}"
                    + " td:first-child{padding-right:4px} td:last-child{padding-left:4px}"
                    + " .img-responsive{display: block; max-width: 100%; height: auto; vertical-align: middle; border: 0;}"
                    + " </style>" 
                    + " </head>"
                    + " <body>"
                    + PatientPortalResourceContent 
                    + " </body></html>";
                str = str.Replace("<br/>", "");
                str = str.Replace("<br />", "");
                return str.Replace("<p>&nbsp;</p>", "");
            }
        }
    }
}
