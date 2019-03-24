using Students_Site.Models.Home;
using System.Collections.Generic;

namespace Students_Site.Models.Student
{
    public class StudentIndexModel : IndexModel
    {
        public IEnumerable<StudentModel> StudentModels;
    }
}
