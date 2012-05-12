﻿using Wysnan.EIMOnline.Common.Poco;
using Wysnan.EIMOnline.IBLL;
using Wysnan.EIMOnline.Common.ViewModel;
using System.Linq;
using System;
using Wysnan.EIMOnline.Injection.Transaction;

namespace Wysnan.EIMOnline.Business
{
    public class SystemModuleModel : GenericBusinessModel<SystemModule>, ISystemModule
    {
        ISystemModuleDetailPage SystemModuleDetailPageModel { get; set; }

        public SystemModuleModel()
        {
        }

        #region 数据库查询

        [TransactionAttribute]
        public override IQueryable<SystemModule> List()
        {
            return base.List();
        }

        public IQueryable<SystemModule> GetAllSystemModule_Greedy()
        {
            return List(a => a.SystemModuleDetailPages);
        }

        public IQueryable<SystemModule> GetSecuritySystemModule()
        {
            return base.List();
        }

        #endregion

        #region 缓存查询

        

        #endregion
    }
}
