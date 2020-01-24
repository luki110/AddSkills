using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddSkills
{
    class Course
    {
        
        public string Title { get; set; }

        public string Level { get; set; }

        public List<string> Skills { get; set; }

        public Course()
        {
            Skills = new List<string>();
        }

        public void addSkill(string txt)
        {
            Skills.Add(txt);
        }

        public void removeSkill(string txt)
        {
            Skills.Remove(txt);
        }

    }
}
