using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VillageGame.Models;


namespace VillageGame.Controllers
{
    public class HomeController : Controller
    {
        public VillageGameEntities2 db = new VillageGameEntities2();
        public Random ran = new Random();

        public bool EndGame()
        {

            if (db.Resources.Find(1).Food < 0 || db.Resources.Find(1).Water < 0)
            {
                return true;
            }
            return false;
        }

        public bool GotRaided()
        {
            int chance = ran.Next(1, 101);
            if(chance <= 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BuildAction(string type)
        {
            if(type == "house")
            {
                db.Buildings.Find(1).Houses++;
                db.Resources.Find(1).Wood -= 5;
                
                Session["ActionMessage"] = "You built a house!";
            }
            else if(type == "well")
            {
                db.Buildings.Find(1).Wells++;
                db.Resources.Find(1).Wood -= 6;
                Session["ActionMessage"] = "You built a well!";
            }
            else if(type == "farm")
            {
                db.Buildings.Find(1).Farms++;
                db.Resources.Find(1).Wood -= 8;
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ResourceAction(string type)
        {
            int quantityWood = ran.Next(1, 4);
            int quantityFood = ran.Next(1, 4);
            int quantityWater = ran.Next(1, 4);

            if (type == "wood")
            {
                db.Resources.Find(1).Wood += quantityWood;
                Session["ActionMessage"] = "You found " + quantityWood + " wood!";
            }
            else if(type == "food")
            {
                db.Resources.Find(1).Food += quantityFood;
                Session["ActionMessage"] = "You found " + quantityFood + " food!";
            }
            else if(type == "water")
            {
                db.Resources.Find(1).Water += quantityWater;
                Session["ActionMessage"] = "You found " + quantityWater + " Water!";
            }
            db.GameDatas.Find(1).Actions--;
            db.SaveChanges();
            return RedirectToAction("index");
        }
        
        public ActionResult GameView()
        {
            return View();
        }

        public ActionResult CreateVillager()
        {
            return View();
        }

        [HttpPost] 
        public ActionResult CreateVillager(string Name)
        {
            Villager villager = new Villager();
            villager.Name = Name;
            db.Villagers.Add(villager);
            Session["ActionMessage"] = "New villager " + villager.Name + " has entered the town!";
            db.GameDatas.Find(1).Actions++;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult NextDay()
        {
            int VillagerCount = db.Villagers.ToArray().Length;
            var buildings = db.Buildings.Find(1);
            var Game = db.GameDatas.Find(1);
            var resources = db.Resources.Find(1);
            if(EndGame())
            {
                return RedirectToAction("GameOver");
            }
            else if(db.GameDatas.Find(1).Actions > 0)
            {
                Session["ActionMessage"] = "You sill have Actions left!";
                return RedirectToAction("Index");
            }
            else
            {
                Session["ActionMessage"] = "";
                resources.Food -= VillagerCount - buildings.Farms;
                resources.Water -= VillagerCount - buildings.Wells;
                Game.Actions = VillagerCount;
                Game.Days++;
                db.SaveChanges();
                if (GotRaided())
                {
                    return RedirectToAction("Raided");
                }
                return RedirectToAction("Index");
            }
        }

        public ActionResult YouSure(string type)
        {
            List<Villager> villagers = db.Villagers.ToList();
            db.Resources.Find(1).Food = 6;
            db.Resources.Find(1).Water = 6;
            db.Resources.Find(1).Wood = 0;
            db.Buildings.Find(1).Wells = 0;
            db.Buildings.Find(1).Houses = 1;
            db.Buildings.Find(1).Farms = 0;
            db.GameDatas.Find(1).Days = 0;
            db.GameDatas.Find(1).Actions = 0;
            for(int i = 0; i < villagers.Count; i++)
            {
                db.Villagers.Remove(villagers[i]);
            }
            db.SaveChanges();


            return View();
        }

        
        public ActionResult Raided()
        {
            int foodRaided = ran.Next(1, 4);
            int waterRaided = ran.Next(1, 4);
            int villagerCount = db.Villagers.ToArray().Length;
            int VillagerIndex = ran.Next(1, villagerCount);
            var resource = db.Resources.Find(1);
            Villager unluckyGuy = db.Villagers.Find(VillagerIndex);

            if(resource.Food < foodRaided)
            {
                resource.Food = 0;
            }
            else
            {
                db.Resources.Find(1).Food -= foodRaided;
            }
            if(resource.Water < waterRaided)
            {
                resource.Water = 0;
            }
            else
            {
                db.Resources.Find(1).Water -= waterRaided;
            }
            Session["LostFood"] = "You've lost " + foodRaided + " food";
            Session["LostWater"] = "You've lost " + waterRaided + " water";
            Session["LostVillager"] = unluckyGuy.Name + "has died and their house was burned to the ground";

            db.Buildings.Find(1).Houses--;
            db.Villagers.Remove(unluckyGuy);
            db.SaveChanges();


            return View();
        }

        public ActionResult GameOver()
        {

            return View();
        }

    }
}