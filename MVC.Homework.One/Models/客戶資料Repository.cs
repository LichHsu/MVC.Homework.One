using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVC.Homework.One.Models
{
    public class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        public override IQueryable<客戶資料> All()
        {
            var r = base.All().Where(o => !o.是否已刪除);
            return r;
        }

        public override void Delete(客戶資料 entity)
        {
            entity.是否已刪除 = true;
            Commit();
            //base.Delete(entity);
        }

        public CustomerEntities Context
        {
            get { return (CustomerEntities)this.UnitOfWork.Context; }
        }

        public SelectList 客戶分類()
        {
            var items = from o in base.All()
                        group o by o.客戶分類 into g
                        select g.Key;

            return new SelectList(items);
        }
    }

    public interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}