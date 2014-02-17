using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.ViewModel;
using Zcu.StudentEvaluator.DAL;

namespace Zcu.StudentEvaluator.DesignData
{
    public class DesignDataContext
    {
        public StudentListViewModel Students {get; private set;}

        public DesignDataContext()
        {
            var unitOfWork = new LocalStudentEvaluationUnitOfWork();
            unitOfWork.PopulateWithData();  //populate with default data

            this.Students = new StudentListViewModel(unitOfWork);

            this.Students.Items[0].IsSelected = true;
            this.Students.Items[0].IsFocused = true;
        }
    }
}
