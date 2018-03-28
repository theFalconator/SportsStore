using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;

        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
        }

        public ViewResult Index(int productPage=1) => 
            View(repository
                .Products
                .OrderBy(p=>p.ProductId)
                .Skip((productPage-1) * PageSize)
                .Take(PageSize));
    }
}