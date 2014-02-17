using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zcu.StudentEvaluator.DAL;

namespace Zcu.StudentEvaluator.ViewModel.Design
{
    internal class DesignData
    {
        public DesignData()
        {
            var unitOfWork = new LocalStudentEvaluationUnitOfWork();
            unitOfWork.PopulateWithData();
        }
    }
}
