using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.ApiService.Model
{
    public class ScgPathologyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override string ToString() => Name;
    }
}
