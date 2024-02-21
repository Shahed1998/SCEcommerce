using entity.Business_Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repository.Interfaces.Helper
{
    public interface IHelperFileHandler
    {
        Task<string> UploadImage(FileHandlerDTO? image, string folder = "User");
    }
}
