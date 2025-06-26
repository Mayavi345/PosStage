using Stage.DAL.Repositories.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Observer;

namespace Stage.DAL
{
    public class MainDALSystem : Singleton<MainDALSystem>
    {

        public MessageSubject UIMessageBox;
        public ICompletedOrdersRepository CompletedOrdersRepository;
        public ICompletedOrdersRepository _completedOrdersRepository;

        public void Init()
        {
        }
    }
}
