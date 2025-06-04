using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Schema;
using System.Xml;
using tesla.Models;

namespace tesla.Controllers
{
    public class XMLController : Controller
    {
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
                using (XmlReader reader = XmlReader.Create(stream, settings))
                {
                    while (reader.Read()) { }
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
            }
            else
            {
                ViewBag.Message = $"XML file is invalid. Error: {validationMessage}";
            }
            return View();
        }
    }
}
