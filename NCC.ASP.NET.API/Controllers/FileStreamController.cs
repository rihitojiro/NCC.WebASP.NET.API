using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;

namespace ASP.NETCore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class FileStreamController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please select a file to upload");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("File uploaded successfully");
        }


        [HttpGet("Download_json_excel")]
        public async Task<IActionResult> Download(string fileName)
        {
            // Đường dẫn tới file JSON
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);

            // Kiểm tra file JSON có tồn tại hay không
            if (!System.IO.File.Exists(jsonFilePath))
            {
                return NotFound();
            }

            // Đọc nội dung của file JSON
            string jsonContent = await System.IO.File.ReadAllTextAsync(jsonFilePath);

            // Chuyển đổi nội dung của file JSON thành đối tượng
            dynamic data = JsonConvert.DeserializeObject(jsonContent);

            // Tạo một StringBuilder để lưu nội dung của file CSV
            StringBuilder csvBuilder = new StringBuilder();

            // Viết tiêu đề của file CSV
            csvBuilder.AppendLine("Name,Age,Sex");

            // Lặp qua các phần tử trong mảng JSON và viết vào file CSV
            foreach (var person in data.People)
            {
                csvBuilder.AppendLine($"{person.Name},{person.Age}");
            }

            // Tạo đường dẫn file CSV mới
            string csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "minh.csv");

            // Lưu nội dung của file CSV vào file mới
            await System.IO.File.WriteAllTextAsync(csvFilePath, csvBuilder.ToString());

            // Đọc file CSV và trả về nó cho client
            byte[] csvData = await System.IO.File.ReadAllBytesAsync(csvFilePath);
            return File(csvData, "text/csv", "minh.csv");
        }





        [HttpGet]
        [Route("download/{filename}")]
        public async Task<IActionResult> Downloadfile(string filename)
        {
            // Lấy đường dẫn đầy đủ của file cần download
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", filename);

            // Kiểm tra xem file có tồn tại hay không
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            // Đọc dữ liệu của file và trả về một FileStreamResult
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out string contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}


