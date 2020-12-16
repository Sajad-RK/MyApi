using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly UserRepository userRepository;
        private readonly Repository<Category> categoryRepository;

        public ValuesController(UserRepository userRepository, Repository<Category> categoryRepository)
        {
            this.userRepository = userRepository;
            this.categoryRepository = categoryRepository;
        }
    }
}
