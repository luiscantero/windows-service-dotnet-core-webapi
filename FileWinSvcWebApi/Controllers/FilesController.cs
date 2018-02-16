using Microsoft.AspNetCore.Mvc;
using FileWinSvcWebApi.Filters;
using System.IO;

namespace FileWinSvcWebApi.Controllers
{
    [Route("api/[controller]")]
    [OnlyLocalHost]
    public class FilesController : Controller
    {
        const string FOLDER = @"C:\Temp\";

        // GET api/files/test.txt
        [HttpGet("{id}")]
        public string Get(string id)
        {
            string file = Path.Combine(FOLDER, id);
            string contents = "";

            if (System.IO.File.Exists(file))
            {
                // Read file.
                contents = System.IO.File.ReadAllText(file);
            }

            return contents;
        }
    }
}
