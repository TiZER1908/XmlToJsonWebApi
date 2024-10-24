using Microsoft.AspNetCore.Mvc;
using XmlToJsonWebApi.Repositories;
using XmlToJsonWebApi.Services;
using XmlToJsonWebApi.Share.DTOs;


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
                var idDict = _dictRepository.GetByKey(id);
                if (idDict != null)
                {
                    _dictRepository.Edit(idDict);
                    idDict.BeginDate = dict.BeginDate;
                    idDict.EndDate = dict.EndDate;
                    idDict.Code = dict.Code;
                    idDict.Name = dict.Name;
                    idDict.Comments = dict.Comments;
                    idDict.EditDate = DateTime.Now;
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
        public async Task<IActionResult> UploadFromFile([FromForm] IEnumerable<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    //IFormFile file = context.Request.Form.Files[0];
                    string path = _appEnvironment.WebRootPath + "\\Files\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string name = file.FileName.Split(new char[] { '.' })[0];

                    using (FileStream fstream = new FileStream(path + file.FileName, FileMode.OpenOrCreate))
                    {
                        await file.CopyToAsync(fstream);
                    }

                    var xmlRead = new XMlReader();
                    var listXml = xmlRead.ReadFromXml(path + file.FileName);
                    var jsonWrite = new JsonWriter();
                    var result = jsonWrite.WriteToJson(listXml, name);

                    if (jsonWrite.WriteToJson(listXml, name))
                    {
                        foreach (var dict in listXml)
                        {
                            var codeDict = _dictRepository.First(x => x.Code == dict.Code & !x.IsDeleted);
                            if (codeDict == null)
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
                                _dictRepository.Edit(codeDict);
                                codeDict.BeginDate = dict.BeginDate;
                                codeDict.EndDate = dict.EndDate;
                                codeDict.Name = dict.Name;
                                codeDict.EditDate = DateTime.Now;
                            }
                        }
                        _dictRepository.Save();
                        return Ok();


                    }
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
