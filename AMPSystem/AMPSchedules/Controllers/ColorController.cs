using System.Collections.Generic;
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

                    var mUser = DbManager.Instance.CreateUserIfNotExists(CurrentUser.Email);
                    if (item is Lesson)
                    {
                        var room = item.Rooms.First();
                        var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
                        var mRoom = DbManager.Instance.CreateRoomIfNotExists(mBuilding, room.Floor, room.Name);
                        var mLesson = DbManager.Instance.ReturnLessonIfExists(item.Name, item.StartTime,item.EndTime);
                        if (mLesson == null)
                        {
                            DbManager.Instance.CreateLesson(item.Name, mRoom, mUser, item.Color, item.StartTime,
                            item.EndTime);
                        }
                        else
                        {
                            DbManager.Instance.SaveLessonColorChange(mLesson, item.Color);
                        }
                        
                    }
                    else if (item is OfficeHours)
                    {
                        var room = item.Rooms.First();
                        var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
                        var mRoom = DbManager.Instance.CreateRoomIfNotExists(mBuilding, room.Floor, room.Name);
                        var mOfficeHours = DbManager.Instance.ReturnOfficeHourIfExists(item.Name, item.StartTime, item.EndTime);
                        if (mOfficeHours == null)
                        {
                            DbManager.Instance.CreateOfficeHour(item.Name, mRoom, mUser, item.Color, item.StartTime,
                            item.EndTime);
                        }
                        else
                        {
                            DbManager.Instance.SaveOfficeHourColorChange(mOfficeHours, item.Color);
                        }

                    }
                    else if (item is EvaluationMoment)
                    {
                        var mRooms = new List<Room>();
                        foreach (var room in item.Rooms)
                        {
                            var mBuilding = DbManager.Instance.CreateBuildingIfNotExists(room.Building.Name);
                            mRooms.Add(DbManager.Instance.CreateRoomIfNotExists(mBuilding, room.Floor, room.Name));
                        }
                        
                        var mEvaluation = DbManager.Instance.ReturnEvaluationMomentIfExists(item.Name, item.StartTime, item.EndTime);
                        if (mEvaluation == null)
                        {
                            DbManager.Instance.CreateEvaluationMoment(item.Name, mRooms, mUser, item.Color, item.StartTime,
                            item.EndTime, item.Description);
                        }
                        else
                        {
                            DbManager.Instance.SaveEvaluationColorChange(mEvaluation, item.Color);
                        }

                    }
                    DbManager.Instance.SaveChanges();
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