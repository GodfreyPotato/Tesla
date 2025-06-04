using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Schema;
using System.Xml;
using tesla.Models;
using practiceQuiz.DataAccess;
using System.Data;

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

                    return Content("Error");
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
                    string query = $"insert into products(prod_name, prod_description,price) values ('{product["prod_name"].InnerText}','{product["prod_description"].InnerText}',{decimal.Parse(product["price"].InnerText)})";
                    helper.execute(query);
                }
           
        }
    }
}
