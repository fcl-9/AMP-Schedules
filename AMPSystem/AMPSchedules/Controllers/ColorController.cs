using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Resources;
using AMPSystem.Classes;
using AMPSystem.Classes.TimeTableItems;
using AMPSystem.DAL;
using Microsoft.Graph;
using Building = AMPSystem.Models.Building;
using Room = AMPSystem.Models.Room;
using User = AMPSystem.Models.User;

namespace Microsoft_Graph_SDK_ASPNET_Connect.Controllers
{
    public class ColorController: TemplateController
    {
        private AmpDbContext db = new AmpDbContext();

        public override ActionResult Hook(TimeTableManager manager)
        {
            //Read The Color that was sent
            string color = null;
            string itemName = null;
            foreach (var eventName in Request.QueryString)
            {
               
                if ((string) eventName != "start" && (string)eventName != "end")
                {
                    //Debug.Write("My key is " + itemName + " ");
                    itemName = (string)eventName;
                    color = Request.QueryString[itemName];
                    //Debug.Write("You're adding color " + color + " \n ");
                }
            }

            //Change the color on the items 
            foreach (var item in manager.TimeTable.ItemList)
            {
                if (item.Name == itemName)
                {
                    if (color != null)
                    {
                        item.Color = color;
                        //Debug.Write("Changing Color of items");
                    }
                    else
                    {
                        Debug.Write("No Color was Defined");
                        break;
                    }
                    if (item is Lesson)
                    {
                        var mUser = db.Users.FirstOrDefault(u => u.Email == CurrentUser.Email) ??
                                    new User {Email = CurrentUser.Email};
                        var room = item.Rooms.First();
                        var mBuilding = db.Buildings.FirstOrDefault(b => b.Name == room.Building.Name) ??
                                        new Building {Name = room.Building.Name};

                        var mRoom =
                            db.Rooms.FirstOrDefault(
                                r =>
                                    r.Building.Name == mBuilding.Name && r.Name == room.Name &&
                                    r.Floor == room.Floor) ??
                            new Room
                            {
                                Building = mBuilding,
                                Floor = item.Rooms.First().Floor,
                                Name = item.Rooms.First().Name
                            };

                        var mLesson =
                            db.Lessons.FirstOrDefault(
                                l => l.Name == item.Name && l.StartTime == item.StartTime && l.EndTime == item.EndTime);
                        if (mLesson == null)
                        {
                            mLesson = new AMPSystem.Models.Lesson
                            {
                                Color = item.Color,
                                EndTime = item.EndTime,
                                StartTime = item.StartTime,
                                Name = item.Name,
                                Room = mRoom,
                                User = mUser
                            };
                            db.Lessons.Add(mLesson);
                        }
                        else
                        {
                            mLesson.Color = item.Color;
                            db.Entry(mLesson).State = System.Data.Entity.EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                }
            }
            return base.Hook(manager);
        }
        [HttpGet]
        public async Task<ActionResult> EventColor()
        {
            try
            {
                return await TemplateMethod();
            }
            catch (ServiceException se)
            {
                if (se.Error.Message == Resource.Error_AuthChallengeNeeded) return new EmptyResult();
                return RedirectToAction($"Index", $"Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
            }
        }
    }
}