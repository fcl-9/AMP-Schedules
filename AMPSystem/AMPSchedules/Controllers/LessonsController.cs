using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AMPSystem.DAL;
using AMPSystem.Models;

namespace AMPSchedules.Controllers
{
    public class LessonsController : Controller
    {
        private readonly AmpDbContext db = new AmpDbContext();

        // GET: Lessons
        public ActionResult Index()
        {
            var lessons = db.Lessons.Include(l => l.Room).Include(l => l.User);
            return View(lessons.ToList());
        }

        // GET: Lessons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var lesson = db.Lessons.Find(id);
            if (lesson == null)
                return HttpNotFound();
            return View(lesson);
        }

        // GET: Lessons/Create
        public ActionResult Create()
        {
            ViewBag.RoomID = new SelectList(db.Rooms, "ID", "Name");
            ViewBag.UserID = new SelectList(db.Users, "ID", "Email");
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Color,Name,StartTime,EndTime,UserID,RoomID")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                db.Lessons.Add(lesson);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoomID = new SelectList(db.Rooms, "ID", "Name", lesson.RoomID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "Email", lesson.UserID);
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var lesson = db.Lessons.Find(id);
            if (lesson == null)
                return HttpNotFound();
            ViewBag.RoomID = new SelectList(db.Rooms, "ID", "Name", lesson.RoomID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "Email", lesson.UserID);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Color,Name,StartTime,EndTime,UserID,RoomID")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lesson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomID = new SelectList(db.Rooms, "ID", "Name", lesson.RoomID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "Email", lesson.UserID);
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var lesson = db.Lessons.Find(id);
            if (lesson == null)
                return HttpNotFound();
            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var lesson = db.Lessons.Find(id);
            db.Lessons.Remove(lesson);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}