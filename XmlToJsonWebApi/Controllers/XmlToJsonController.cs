using Microsoft.AspNetCore.Mvc;

using System.Text;
using XmlToJsonWebApi.Repositories;
using XmlToJsonWebApi.Services;
using XmlToJsonWebApi.Data.Model;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using XmlToJsonWebApi.Share;
using System.IO.Pipes;
using XmlToJsonWebApi.Share.DTOs;
using System.Xml.Linq;


namespace XmlToJsonWebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class XmlToJsonController : Controller
    {

        private readonly IDictionaryRepository _dictRepository;
        IWebHostEnvironment _appEnvironment;

        public XmlToJsonController(IWebHostEnvironment appEnvironment, IDictionaryRepository dictRepository)
        {
            _dictRepository = dictRepository;
            _appEnvironment = appEnvironment;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_dictRepository.GetAll().Select(x => new DictionaryDTO()
            {
                Id = x.Id,
                BeginDate = x.BeginDate,
                EndDate = x.EndDate,
                Code = x.Code,
                Name = x.Name,
                Comments = x.Comments
            }));
        }

        [HttpGet("{id}")]
        public IActionResult GetbyId(int id)
        {
            var dict = _dictRepository.GetByKey(id);
            if (dict != null)
            {
                return Ok(dict);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] DictionaryDTO dict)
        {
            try {
                _dictRepository.Add(new()
                {
                    BeginDate = dict.BeginDate,
                    EndDate = dict.EndDate,
                    Code = dict.Code,
                    Name = dict.Name,
                    Comments = dict.Comments
                });
                _dictRepository.Save();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] DictionaryDTO dict)
        {
            try
            {
                var iddict = _dictRepository.GetByKey(id);
                if (iddict != null)
                {
                    _dictRepository.Edit(iddict);
                    iddict.BeginDate = dict.BeginDate;
                    iddict.EndDate = dict.EndDate;
                    iddict.Code = dict.Code;
                    iddict.Name = dict.Name;
                    iddict.Comments = dict.Comments;
                    iddict.EditDate = DateTime.Now;
                    _dictRepository.Save();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return BadRequest();
            }
        }
    

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var dict = _dictRepository.GetByKey(id);
            if (dict != null)
            {
                _dictRepository.VirtualDelete(dict, id);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFromFile(IFormFile file)
        {
            try
            {
                //IFormFile file = context.Request.Form.Files[0];
                string filepath = _appEnvironment.WebRootPath + "\\Files\\" + file.FileName;

                string name = file.FileName.Split(new char[] { '.' })[0];

                using (FileStream fstream = new FileStream(filepath, FileMode.OpenOrCreate))
                {
                   await file.CopyToAsync(fstream);
                }

                var xmlread = new XMlReader();
                var listxml = xmlread.ReadFromXml(filepath);
                var jsonwrt = new JsonWriter();
                var resdict = jsonwrt.WriteToJson(listxml, name);

                if (jsonwrt.WriteToJson(listxml, name))
                {
                    foreach (var dict in listxml)
                    {
                        var codedict = _dictRepository.First(x => x.Code == dict.Code & !x.IsDeleted);
                        if (codedict == null)
                        {
                            _dictRepository.Add(new()
                            {
                                BeginDate = dict.BeginDate,
                                EndDate = dict.EndDate,
                                Code = dict.Code,
                                Name = dict.Name,
                                Comments = String.Empty
                            });
                        }
                        else
                        {
                            _dictRepository.Edit(codedict);
                            codedict.BeginDate = dict.BeginDate;
                            codedict.EndDate = dict.EndDate;
                            codedict.Name = dict.Name;
                            codedict.EditDate = DateTime.Now;
                        }
                    }
                    _dictRepository.Save();
                    return Ok();
                    

                }
                else
                {
                    return BadRequest();
                }
                
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
