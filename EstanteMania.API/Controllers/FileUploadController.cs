using EstanteMania.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EstanteMania.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class FileUploadController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<FileUploadController> logger;
         
        public FileUploadController(IWebHostEnvironment env, ILogger<FileUploadController> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<IList<UploadResult>>> PostFile([FromForm] IEnumerable<IFormFile> files)
        {
            var maxFileNumber = 3;
            long maxFileSize = 1024 * 400;
            var processedFiles = 0;
            var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}");
            List<UploadResult> uploadResults = [];

            foreach (var _file in files)
            {
                if (!VerificaExtensaoArquivo(_file))
                {
                    return BadRequest($"O arquivo não possui uma extensão ou não é uma imagem.\r\n" +
                        $"Extensões suportadas: .jpg/.png/.bmp/.jpge");
                }

                UploadResult uploadResult = new()
                {
                    FileName = _file.Name
                };

                if (processedFiles < maxFileNumber)
                {
                    if (_file.Length == 0)
                    {
                        logger.LogInformation("{FileName} tamanho é 0 (Err: 1)", _file.FileName);
                        uploadResult.ErrorCode = 1;
                    }
                    else if (_file.Length > maxFileSize)
                    {
                        logger.LogInformation("{FileName} de {Length} bytes é maior que o limite de {Limit} bytes (Err: 2)",
                            _file.FileName, _file.Length, maxFileSize);
                        uploadResult.ErrorCode = 2;
                    }
                    else
                    {
                        try
                        {
                            var path = Path.Combine(env.WebRootPath, "Images", _file.FileName);
                            await using FileStream fs = new(path, FileMode.Create);
                            await _file.CopyToAsync(fs);

                            logger.LogInformation("{FileName} salvo em {Path}", _file.FileName, path);
                            uploadResult.Uploaded = true;
                        }
                        catch (IOException ex)
                        {
                            logger.LogError("{FileName} erro ao enviar (Err: 3): {Message}",
                                _file.FileName, ex.Message);
                            uploadResult.ErrorCode = 3;
                        }
                    }
                    processedFiles++;
                }
                else
                {
                    logger.LogInformation("{FileName} não enviado pois o request excedeu {Count} files (Err: 4)",
                        _file.FileName, maxFileNumber);
                    uploadResult.ErrorCode = 4;
                }
                uploadResults.Add(uploadResult);
            }
            return new CreatedResult(resourcePath, uploadResults);
        }

        private static bool VerificaExtensaoArquivo(IFormFile file)
        {
            string[] extensoes = ["jpg", "bmp", "png", "jpge"];
            var nomeArquivoExtensao = file.FileName.Split(".")[1];

            if (string.IsNullOrEmpty(nomeArquivoExtensao) || !extensoes.Contains(nomeArquivoExtensao))
            {
                return false;
            }

            return true;
        }
    }
}
