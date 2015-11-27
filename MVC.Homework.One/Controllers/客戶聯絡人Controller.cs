using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC.Homework.One.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Omu.ValueInjecter;

namespace MVC.Homework.One.Controllers
{
    public class 客戶聯絡人Controller : Controller
    {
        private CustomerEntities db = new CustomerEntities();

        // GET: 客戶聯絡人
        public ActionResult Index(int? id, string 職稱)
        {
            var items = from o in db.客戶聯絡人.AsQueryable()
                        group o by o.職稱 into g
                        select g.Key;

            ViewBag.職稱 = new SelectList(items);

            var 客戶 = db.客戶資料.Find(id);
            ViewBag.客戶Id = id;
            ViewBag.客戶名稱 = 客戶.客戶名稱;

            var r = db.客戶聯絡人.Where(o => o.客戶Id == id.Value);

            if (!string.IsNullOrEmpty(職稱))
            {
                r = r.Where(o => o.職稱 == 職稱);
            }

            return View(r);
            //var 客戶聯絡人 = db.客戶聯絡人.Include(客 => 客.客戶);
            //return View(客戶聯絡人.ToList());
        }

        [HttpPost]
        public ActionResult Index(int? id, string 職稱, List<客戶聯絡人> data)
        {
            var items = from o in db.客戶聯絡人.AsQueryable()
                        group o by o.職稱 into g
                        select g.Key;

            ViewBag.職稱 = new SelectList(items);

            if (ModelState.IsValid)
            {
                客戶聯絡人 p = null;
                foreach (var item in data)
                {
                    p = db.客戶聯絡人.Find(item.Id);
                    p.InjectFrom(item);
                    db.Entry(p).State = EntityState.Modified;
                }

                db.SaveChanges();
            }

            return Index(id, 職稱);
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create(int? 客戶Id)
        {
            if (客戶Id == null)
            {
                ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱");
            }
            else
            {
                var 客戶 = db.客戶資料.Find(客戶Id);
                ViewBag.客戶Id = 客戶Id;
                ViewBag.客戶名稱 = 客戶.客戶名稱;
                //ViewBag.客戶Id = new SelectList(db.客戶, "Id", "客戶名稱");
            }
            return View();
        }

        // POST: 客戶聯絡人/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                db.客戶聯絡人.Add(客戶聯絡人);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Details", "客戶", new { id = 客戶聯絡人.客戶Id });
            }

            var 客戶 = db.客戶資料.Find(客戶聯絡人.客戶Id);
            ViewBag.客戶Id = 客戶聯絡人.客戶Id;
            ViewBag.客戶名稱 = 客戶.客戶名稱;
            return View(客戶聯絡人);
            //ViewBag.客戶Id = new SelectList(db.客戶, "Id", "客戶名稱", 客戶聯絡人.客戶Id);

            //return RedirectToAction("Details", "客戶", new { id = 客戶聯絡人.客戶Id });
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                db.Entry(客戶聯絡人).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "客戶", new { id = 客戶聯絡人.客戶Id });
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            //db.客戶聯絡人.Remove(客戶聯絡人);
            客戶聯絡人.是否已刪除 = true;
            db.SaveChanges();
            return RedirectToAction("Details", "客戶", new { id = 客戶聯絡人.客戶Id });

            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult CreateExcelFile()
        {
            //建立Excel 2003檔案
            IWorkbook wb = new HSSFWorkbook();
            ISheet ws = wb.CreateSheet("客戶聯絡人");

            ws.CreateRow(0);//第一行為欄位名稱
            ws.GetRow(0).CreateCell(0).SetCellValue("職稱");
            ws.GetRow(0).CreateCell(1).SetCellValue("姓名");
            ws.GetRow(0).CreateCell(2).SetCellValue("Email");
            ws.GetRow(0).CreateCell(3).SetCellValue("手機");
            ws.GetRow(0).CreateCell(4).SetCellValue("電話");

            var r = db.客戶聯絡人.ToList();
            客戶聯絡人 c = null;
            for (int i = 0, j = 1, cnt = r.Count; i < cnt; i++, j++)
            {
                c = r[i];
                ws.CreateRow(j);//第二行之後為資料
                ws.GetRow(j).CreateCell(0).SetCellValue(c.職稱);
                ws.GetRow(j).CreateCell(1).SetCellValue(c.姓名);
                ws.GetRow(j).CreateCell(2).SetCellValue(c.Email);
                ws.GetRow(j).CreateCell(3).SetCellValue(c.手機);
                ws.GetRow(j).CreateCell(4).SetCellValue(c.電話);
            }

            var f = System.IO.Path.GetTempFileName();
            FileStream file = new FileStream(f, FileMode.Create);//產生檔案
            wb.Write(file);
            file.Close();

            //var sf = Server.MapPath(f);
            var contentTypte = "application/vnd.ms-excel";
            return File(f, contentTypte, "無題 1.xls");            
        }
    }
}
