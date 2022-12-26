using CRUD_MVC_XML.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace CRUD_MVC_XML.Controllers
{
    public class InventoryController : Controller
    {
        private  string path;
        private  Inventory model;
        public InventoryController()
        {
            this.path = "xml/Inventory.xml";
            this.model = new Inventory();
        }
        public IActionResult AddEditProject(int? id)
        {
            int Id = Convert.ToInt32(id);
            if (Id > 0)
            {
                GetById(Id); 
                model.isEdit= true;
                return View(model);
            }
            else
            {
                model.isEdit= false;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult AddEditProject(Inventory mdl)
        {
            if(mdl.Id>0) 
            {
                XDocument xmlDoc = XDocument.Load(path);
                var items = (from item in xmlDoc.Descendants("Inventory") select item).ToList();
                XElement selected = items.Where(p=>p.Element("ID").Value == mdl.Id.ToString()).FirstOrDefault();
                selected.Remove();
                xmlDoc.Save(path);
                xmlDoc.Element("Inventories").Add(new XElement("ID", mdl.Id),
                    new XElement("Name",mdl.Name),
                    new XElement("ItemSupplier",mdl.ItemSupplier),
                    new XElement("QuantityAvailable", mdl.QuantityAvailable),
                    new XElement("PriceCost", mdl.PriceCost),
                    new XElement("PriceSale", mdl.PriceSale),
                    new XElement("OrderPoint", mdl.OrederPoint));
                xmlDoc.Save(path);
                return RedirectToAction("Index", "Inventory");
            }
            else
            {
                XmlDocument oXmlDocument  = new XmlDocument();
                oXmlDocument.Load(path);
                XmlNodeList nodeList = oXmlDocument.GetElementsByTagName("Inventory");
                var x = oXmlDocument.GetElementsByTagName("ID");
                int Max = 0;
                foreach(XmlElement item in x)
                {
                    int EId = Convert.ToInt32(item.InnerText.ToString());
                    if (EId > Max)
                    {
                        Max = EId;
                    }
                    
                }
                Max = Max + 1;
                XDocument xmlDoc = XDocument.Load(path);
                xmlDoc.Element("Inventories").Add(new XElement("ID", mdl.Id),
                new XElement("Name", mdl.Name),
                new XElement("ItemSupplier", mdl.ItemSupplier),
                new XElement("QuantityAvailable", mdl.QuantityAvailable),
                new XElement("PriceCost", mdl.PriceCost),
                new XElement("PriceSale", mdl.PriceSale),
                new XElement("OrderPoint", mdl.OrederPoint));
                xmlDoc.Save(path);
                return RedirectToAction("Index", "Inventory");
            }
        }

        public ActionResult Delete(int Id)
        {
            if (Id > 0)
            {
                XDocument xmlDoc = XDocument.Load(path);
                var items =(from item in xmlDoc.Descendants("Inventory")  select item).ToList();
                XElement selected = items.Where(p=>p.Element("ID").Value==Id.ToString()).FirstOrDefault();
                selected.Remove();
                xmlDoc.Save(path);
            }
            return RedirectToAction("Index", "Inventory");
        }
        public IActionResult Index()
        {
            List<Inventory> list = new List<Inventory>();
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(path);
            DataView dvProgramms;

            if(dataSet.Tables.Count > 0)
            {

                dvProgramms = dataSet.Tables[0].DefaultView;
                dvProgramms.Sort = "ID";
                foreach (DataRowView dr in dvProgramms)
                {
                    Inventory model = new Inventory();
                    model.Id = Convert.ToInt32(dr[0]);
                    model.Name = Convert.ToString(dr[1]);
                    model.ItemSupplier = Convert.ToString(dr[2]);
                    model.QuantityAvailable = Convert.ToString(dr[3]);
                    model.PriceCost = Convert.ToInt16(dr[4]);
                    model.PriceSale = Convert.ToInt16(dr[5]);
                    model.OrederPoint = Convert.ToInt16(dr[6]);
                    list.Add(model);
                }
                if(list.Count > 0)
                {
                    return View(list);
                }
                
            }
            return View();
        }

        public void GetById(int id)
        {
            XDocument oXmlDocument = XDocument.Load(path);
            var items = (from item in oXmlDocument.Descendants("Inventory")
                where Convert.ToInt32(item.Element("ID").Value)==id
                select new InventoryItems()
                {
                    Name = item.Element("Name").Value,
                    ItemSupplier = item.Element("ItemSupplier").Value,
                    QuantityAvailable = item.Element("QuantityAvailable").Value,
                    PriceCost = Convert.ToInt32(item.Element("PriceCost").Value),
                    PriceSale = Convert.ToInt32(item.Element("PriceSale").Value),
                    OrederPoint = Convert.ToInt32(item.Element("OrderPoint").Value)

                }).SingleOrDefault();
            if(items != null)
            {
                model.Name = items.Name;
                model.ItemSupplier = items.ItemSupplier;
                model.QuantityAvailable = items.QuantityAvailable;
                model.PriceCost = items.PriceCost;
                model.PriceSale = items.PriceSale;
                model.OrederPoint = items.OrederPoint;
            }
        }
    }
    public class InventoryItems
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ItemSupplier { get; set; }
        public string QuantityAvailable { get; set;}
        public decimal PriceCost { get; set;}
        public decimal PriceSale { get; set;}
        public decimal OrederPoint { get; set; }
        public InventoryItems() { }
    }
}
