using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace MVC.Homework.One.Models
{
    public class 客戶資料Controller : Controller
    {
        //private CustomerEntities db = new CustomerEntities();

        客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();
        // GET: 客戶
        public ActionResult Index(string 客戶查詢條件, string 客戶分類, int page = 1)
        {
            ViewBag.客戶分類 = repo.客戶分類();

            var r = repo.All();//db.客戶.AsQueryable();
            //r = r.Where(o => o.是否已刪除 == false);

            if (!string.IsNullOrEmpty(客戶分類))
            {
                r = r.Where(o => o.客戶分類 == 客戶分類);
            }

            if (!string.IsNullOrEmpty(客戶查詢條件))
            {
                r = r.Where(o => o.客戶名稱.Contains(客戶查詢條件) ||
                                 o.統一編號.Contains(客戶查詢條件) ||
                                 o.地址.Contains(客戶查詢條件) ||
                                 o.Email.Contains(客戶查詢條件) ||
                                 o.電話.Contains(客戶查詢條件)
                                 );
            }


            r = r.Include(o => o.客戶聯絡人);
            r = r.Include(o => o.客戶銀行資訊);
            foreach (var item in r)
            {
                item.客戶聯絡人.Where(p => p.是否已刪除 == false);
                item.客戶銀行資訊.Where(p => p.是否已刪除 == false);
            }

            var pagedR = r.OrderBy(o => o.Id).ToPagedList(page, 2);

            return View(pagedR);
        }

        // GET: 客戶/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //客戶 客戶 = db.客戶.Find(id);
            var 客戶資料 = repo.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶/Create
        public ActionResult Create(int? 客戶Id)
        {
            if (客戶Id == null)
            {
                ViewBag.客戶Id = new SelectList(repo.ObjectDataSet, "Id", "客戶名稱");
                //ViewBag.客戶Id = new SelectList(db.客戶, "Id", "客戶名稱");
            }
            else
            {
                var 客戶 = repo.Find(客戶Id); //db.客戶.Find(客戶Id);
                ViewBag.客戶Id = 客戶Id;
                ViewBag.客戶名稱 = 客戶.客戶名稱;
                //ViewBag.客戶Id = new SelectList(db.客戶, "Id", "客戶名稱");
            }
            return View();
        }

        // POST: 客戶/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶資料);
                repo.Commit();
                //db.客戶.Add(客戶);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            客戶資料 客戶資料 = repo.Find(id);

            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            客戶資料.密碼 = "";

            return View(客戶資料);
        }

        // POST: 客戶/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(客戶資料 客戶)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(客戶).State = EntityState.Modified;
                //db.SaveChanges();
                using (SHA256CryptoServiceProvider csp = new SHA256CryptoServiceProvider())
                {
                    var p = 客戶.密碼;
                    客戶.密碼 = BitConverter.ToString(csp.ComputeHash(Encoding.Default.GetBytes(p))).Replace("-", "");
                }

                repo.Edit(客戶);
                repo.Commit();
                return RedirectToAction("Index");
            }
            return View(客戶);
        }

        // GET: 客戶/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id);// db.客戶.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repo.Delete(repo.Find(id));
            //客戶 客戶 = db.客戶.Find(id);
            ////db.客戶.Remove(客戶);
            //客戶.是否已刪除 = true;
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Action客戶銀行資訊(int id)
        {
            var c = repo.Find(id);
            if (c == null)
            {
                return HttpNotFound();
            }
            return View(c.客戶銀行資訊);
        }
    }
}
