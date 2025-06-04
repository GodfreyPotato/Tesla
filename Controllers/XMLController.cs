using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Schema;
using System.Xml;
using tesla.Models;
using practiceQuiz.DataAccess;
using System.Data;
using System.Xml.Linq;

namespace tesla.Controllers
{
    public class XMLController : Controller
    { DatabaseHelper helper;
        public XMLController()
        {
            helper = new DatabaseHelper();
        }
        [HttpGet]
        public IActionResult UploadXML()
        {
            return View();
        }

        private bool ValidateXml(IFormFile file, string xsdPath, out string validationMessage)
        {
            validationMessage = string.Empty;
            bool isValid = true;
            string errorMessage = string.Empty;
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add("", xsdPath);
            using (var stream = file.OpenReadStream())
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas = schemaSet;
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += (sender, args) =>
                {
                    isValid = false;
                    errorMessage = args.Message;
                };
                try
                {
                    using (XmlReader reader = XmlReader.Create(stream, settings))
                    {
                        while (reader.Read()) { }
                    }
                }
                catch(Exception e)
                {
                    return false;
                }
            }
            validationMessage = errorMessage;
            return isValid;
        }



        [HttpPost]
        public IActionResult UploadXML(IFormFile file)
        {
            if (file == null || file.Length == 0 || Path.GetExtension(file.FileName).ToLower() != ".xml")
            {
                ViewBag.Message = "Invalid! Please select a valid XML file.";
                return View();
            }
            string schemaPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Schemas", "Products.xsd");
            bool isValid = ValidateXml(file, schemaPath, out string validationMessage);
            if (isValid)
            {
                ViewBag.Message = "XML file is valid.";
                //add to database
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "XMLUploads");
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                string filePath = Path.Combine(uploadFolder, Path.GetFileName(file.FileName));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                try
                {

                    XMLToDb(filePath);

                    return RedirectToAction("AddProduct", "Product");
                }
                catch(Exception e)
                {

                    return Content($"Error {e.Message}");
                }
              
            }
            else
            {
                ViewBag.Message = $"Invalid XML file, Error: {validationMessage}";
            }
            return View();
        }

        private void XMLToDb(string xmlPath) {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNodeList products = doc.SelectNodes("/Products/Product");
                foreach (XmlNode product in products)
                {
                string prodName = product["prod_name"].InnerText;
                string prodDesc = product["prod_description"].InnerText;
                decimal price = decimal.Parse(product["price"].InnerText);
                string img = product["prod_img"].InnerText != "" ? product["prod_img"].InnerText : null;
                string cat_id = product["cat_id"].InnerText != "" ? product["cat_id"].InnerText  : null;
                if(cat_id != null)
                {
                    string query = $"insert into products(prod_name, prod_description,price, prod_img, cat_id) values ('{prodName}','{prodDesc}',{price},'{img}', {cat_id})";
                    helper.execute(query); 
                }
                else
                {
                    string query = $"insert into products(prod_name, prod_description,price, prod_img) values ('{prodName}','{prodDesc}',{price},'{img}')";
                    helper.execute(query);
                }
                }
        }

        public IActionResult ExportXML()
        {
            DataTable products = helper.read("SELECT * FROM products");
            List<Product> prods = new List<Product>();

            foreach(DataRow dr in products.Rows)
            {
                prods.Add(new Product
                {
                    prod_name = dr["prod_name"].ToString(),
                    prod_description = dr["prod_description"].ToString(),
                    price = decimal.Parse(dr["price"].ToString()),
                    prod_img = string.IsNullOrWhiteSpace(dr["prod_img"].ToString()) ? "" : dr["prod_img"].ToString(),
                    cat_id = string.IsNullOrWhiteSpace(dr["cat_id"].ToString()) ? null : int.Parse(dr["cat_id"].ToString())
                });
            }

            WriteProductsToXml(prods);
            return Content("XML Exported");
        }



        private void WriteProductsToXml(List<Product> products)
        {
            XDocument xmlDoc = new XDocument(new XElement("Products",
                products.Select(p => new XElement(
                    "Product",
                        new XElement("prod_name", p.prod_name),
                        new XElement("prod_description", p.prod_description),
                        new XElement("price", p.price),
                         new XElement("prod_img", string.IsNullOrWhiteSpace(p.prod_img) ? "" : p.prod_img ),
                          new XElement("cat_id", string.IsNullOrWhiteSpace (p.cat_id.ToString()) ? "" : p.cat_id)
                        ))));


            xmlDoc.Save(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Schemas","Products.xml"));
        }
    }
}
