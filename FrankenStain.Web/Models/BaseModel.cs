using System;
using System.IO;

namespace FrankenStain.Web.Models
{
    public class BaseModel
    {
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string TOTAL { get; set; }
        public string URL
        {
            get
            {
                var file = File.ReadAllBytes($@"C:\Images\{Nome}");
                string base64String = Convert.ToBase64String(file, 0, file.Length);
                return  "data:image/png;base64," + base64String;
            }
        }

        public string URL2
        {
            get
            {
                try
                {
                    var file = File.ReadAllBytes($@"C:\Images\New\{Nome}");
                    if (file == null)
                        return null;

                    string base64String = Convert.ToBase64String(file, 0, file.Length);
                    return "data:image/png;base64," + base64String;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public string Color
        {
            get
            {
                if (Tipo == "POSITIVO")
                    return "green";

                return "red";
            }
        }
    }
}
